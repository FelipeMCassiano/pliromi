using AutoMapper;
using Communication.Requests;
using Exceptions;
using Exceptions.ExceptionsBase;
using Microsoft.Extensions.Options;
using Pliromi.Application.UseCase.User.Register;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Entities.Events;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Services.LoggedUser;
using Pliromi.Domain.Services.ServiceBus;
using Validot;

namespace Pliromi.Application.UseCase.Transaction;

public class RegisterTransactionUseCase : IRegisterTransactionUseCase
{
	private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
	private readonly ITransactionWriteOnlyRepository _transactionWriteOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ISendEmailQueue _sendEmailQueue;
	private readonly IValidator<RequestRegisterTransaction> _validator;
	private readonly IMapper _mapper;
	private readonly ILoggedUser _loggedUser;

	public RegisterTransactionUseCase(ITransactionWriteOnlyRepository transactionWriteOnlyRepository, IUnitOfWork unitOfWork, IValidator<RequestRegisterTransaction> validator, IMapper mapper, ILoggedUser loggedUser, IUserUpdateOnlyRepository userUpdateOnlyRepository, ISendEmailQueue sendEmailQueue)
	{
		_transactionWriteOnlyRepository = transactionWriteOnlyRepository;
		_unitOfWork = unitOfWork;
		_validator = validator;
		_mapper = mapper;
		_loggedUser = loggedUser;
		_userUpdateOnlyRepository = userUpdateOnlyRepository;
		_sendEmailQueue = sendEmailQueue;
	}

	public async Task Execute(RequestRegisterTransaction request)
	{
		Validate(request);
		
		var loggedUser = await _loggedUser.User();
		
		var senderUser = await _userUpdateOnlyRepository.GetUser(loggedUser); 
		
		var receiverUser = await _userUpdateOnlyRepository.GetReceiverByPliromiKey(request.PliromiKey);
		
		if (receiverUser is null)
		{
			throw new NotFoundUserException(PliromiTransactionMessagesErrors.ReceiverNotFound);
		}

		if ((senderUser.Balance - request.Value) < 0)
		{
			throw new InsufficientBalanceException(PliromiTransactionMessagesErrors.InsufficientBalance);
		}

		senderUser.Balance -= request.Value;
		receiverUser.Balance += request.Value;

		var transaction = new Domain.Entities.Transaction()
		{
			Value = request.Value,
			SenderId = senderUser.Id,
			ReceiverId = receiverUser.Id,
		};
		await _transactionWriteOnlyRepository.AddAsync(transaction);
	
	    await _unitOfWork.Commit();	
	    
	    var eventMessage = _mapper.Map<TransactedEventConsumer>(transaction);
	    eventMessage.Key = receiverUser.PliromiKey.Key;
	    
	    await _sendEmailQueue.SendMessage(eventMessage);
	}


	private void Validate(RequestRegisterTransaction request)
	{
		var validationResult = _validator.Validate(request);
		
		if (!validationResult.AnyErrors) return;
		
		var errors = validationResult.MessageMap.Values.SelectMany(list => list).Distinct().ToList();
		
		throw new ErrorOnValidationException(errors);
	}
}