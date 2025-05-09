using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using AutoMapper;
using Communication.Enums;
using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Exceptions.ExceptionsBase;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Security.Cryptography;
using Pliromi.Domain.Security.Tokens;
using Validot;

namespace Pliromi.Application.UseCase.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
	private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IValidator<RequestRegisterUser> _validator;
	private readonly IAccessTokenGenerator _accessTokenGenerator;
	private readonly IPasswordEncrypter _passwordEncrypter;
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;

	public RegisterUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<RequestRegisterUser> validator, IAccessTokenGenerator accessTokenGenerator, IPasswordEncrypter passwordEncrypter, IUserReadOnlyRepository userReadOnlyRepository)
	{
		_userWriteOnlyRepository = userWriteOnlyRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_validator = validator;
		_accessTokenGenerator = accessTokenGenerator;
		_passwordEncrypter = passwordEncrypter;
		_userReadOnlyRepository = userReadOnlyRepository;
	}

	public async Task<ResponseRegister> Execute(RequestRegisterUser request)
	{
		Validate(request);
		
		var user = _mapper.Map<Domain.Entities.User>(request);
		
		var existingUser = await _userReadOnlyRepository.GetActiveUserByIdentifiersAsync(user);
		
		if (existingUser is not null){
			VerifyWhichFieldIsDuplicated(existingUser, user);
		}
		
		user.UserIdentifier = Guid.NewGuid();
		user.Password = _passwordEncrypter.Encrypt(request.Password);
		RegisterPliromiKey(user, request);
		
		await _userWriteOnlyRepository.AddAsync(user);
		
		await _unitOfWork.Commit();

		return new ResponseRegister()
		{
			Name = user.Fullname,
			Tokens = [_accessTokenGenerator.Generate(user.UserIdentifier)]
		};
	}

	private  void Validate(RequestRegisterUser request)
	{
		var validationResult = _validator.Validate(request);

		if (!validationResult.AnyErrors) return;
		
		var errors = validationResult.MessageMap.Values.SelectMany(list => list).Distinct().ToList();
		throw new ErrorOnValidationException(errors);
	}

	private static void VerifyWhichFieldIsDuplicated(Domain.Entities.User existingUser,
		Domain.Entities.User requestUser)
	{
		
	var errors = new List<string>();

	if ( existingUser.Cpf is not null && existingUser.Cpf.Equals(requestUser.Cpf))
	{
		errors.Add(PliromiUserMessagesErrors.CpfAlreadyRegistered);
	}

	if (existingUser.Cnpj != null && existingUser.Cnpj.Equals(requestUser.Cnpj))
	{
		errors.Add(PliromiUserMessagesErrors.CnpjAlreadyRegistered);
	}

	if (existingUser.Email.Equals(requestUser.Email))
	{
		errors.Add(PliromiUserMessagesErrors.EmailAlreadyRegistered);
	}

    throw new AlreadyRegisteredException(errors);
	}

	private void RegisterPliromiKey(Domain.Entities.User user, RequestRegisterUser request)
	{
		switch (request.PliromiKeyType)
		{
			case PliromiKeyType.Cpf:
			{
				if (request.Cpf is null)
				{
					throw new ErrorOnRegisteringPliromiKey(PliromiUserMessagesErrors.CannotRegisterAEmptyCpfAsPliromiKey);
				}

				user.PliromiKey = new PliromiKey()
				{
					Key = request.Cpf,
					Type = Domain.Enums.PliromiKeyType.Cpf,

				};
				return;
			}
			case PliromiKeyType.Cnpj:
			{
				if (request.Cnpj is null)
				{
					throw new ErrorOnRegisteringPliromiKey(PliromiUserMessagesErrors.CannotRegisterAEmptyCnpjAsPliromiKey);
				}

				user.PliromiKey = new PliromiKey()
				{
					Type = Domain.Enums.PliromiKeyType.Cnpj,
					Key = request.Cnpj
				};
				return;
			}
			case PliromiKeyType.Email:
				user.PliromiKey = new PliromiKey()
				{
					Type = Domain.Enums.PliromiKeyType.Email,
					Key = request.Email
				};
				return;
			case PliromiKeyType.Random:
				user.PliromiKey = new PliromiKey()
				{
					Type = Domain.Enums.PliromiKeyType.Random,
					Key = Guid.NewGuid().ToString()
				};
				break;
		}
	}
}