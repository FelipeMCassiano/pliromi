using AutoMapper;
using Communication.Requests;
using Exceptions.ExceptionsBase;
using Microsoft.Extensions.Options;
using Pliromi.Application.UseCase.User.Register;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Services.LoggedUser;
using Validot;

namespace Pliromi.Application.UseCase.Transaction;

public class RegisterTransactionUseCase : IRegisterTransactionUseCase
{
	private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
	private readonly ITransactionWriteOnlyRepository _transactionWriteOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly HttpClient _httpClient;
	private readonly IValidator<RequestRegisterTransaction> _validator;
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;

	public RegisterTransactionUseCase(ITransactionWriteOnlyRepository transactionWriteOnlyRepository, IUnitOfWork unitOfWork, HttpClient httpClient, IValidator<RequestRegisterTransaction> validator, IMapper mapper, ILoggedUser loggedUser, IUserUpdateOnlyRepository userUpdateOnlyRepository)
	{
		_transactionWriteOnlyRepository = transactionWriteOnlyRepository;
		_unitOfWork = unitOfWork;
		_httpClient = httpClient;
		_validator = validator;
		_mapper = mapper;
		_loggedUser = loggedUser;
		_userUpdateOnlyRepository = userUpdateOnlyRepository;
	}

	public async Task Execute(RequestRegisterTransaction request)
	{
		Validate(request);
		
		var loggedUser = await _loggedUser.User();
		
		var senderUser = await _userUpdateOnlyRepository.GetUser(loggedUser); 
		
		var receiverData = _mapper.Map<ReceiverDataForTransaction>(request);
		
		var receiverUser = await _userUpdateOnlyRepository.GetReceiver(receiverData);
		if (receiverUser is null)
		{
			// not found receiver
			throw new NotFoundUserException("not found receiver");
		}

		if ((senderUser.Balance - request.Value) < 0)
		{
			throw new InsufficientBalanceException("insufficient balance");
		}

		senderUser.Balance =- request.Value;
		receiverUser.Balance += request.Value;

		var transaction = new Domain.Entities.Transaction()
		{
			Value = request.Value,
			SenderId = senderUser.Id,
			ReceiverId = receiverUser.Id,
		};
		await _transactionWriteOnlyRepository.AddAsync(transaction);
	
		await AuthorizeRequest();	
	
	    await _unitOfWork.Commit();	
	    
	    // implement a real service
	    await _httpClient.PostAsync("https://util.devi.tools/api/v1/notify", null);
	}


	private  async Task AuthorizeRequest()
	{
			var responseAuthorization = await _httpClient.GetAsync("https://util.devi.tools/api/v2/authorize");
		if (!responseAuthorization.IsSuccessStatusCode)
		{
			//unauthorized transaction
			throw new UnauthorizedTransactionException("unauthorized transaction");
		}
		
	}
	private void Validate(RequestRegisterTransaction request)
	{
		var validationResult = _validator.Validate(request);
		if (validationResult.AnyErrors)
		{
			throw new ErrorOnValidationException(validationResult.ToString()!);
		}
	}
}