using AutoMapper;
using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Exceptions.ExceptionsBase;
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
		
		var existingUser = await _userReadOnlyRepository.ActiveUserWithCpfOrEmailOrCnpj(user);
		
		if (existingUser is not null){
			VerifyWhichFieldIsDuplicated(existingUser, user);
		}
		
		user.UserIdentifier = Guid.NewGuid();
		user.Password = _passwordEncrypter.Encrypt(request.Password);
		
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
		
		if (!string.IsNullOrEmpty(existingUser.Cpf) && existingUser.Cpf == requestUser.Cpf)
		{
			errors.Add(PliromiUserMessagesErrors.AlreadyRegisteredCpf);
		}

		if (!string.IsNullOrEmpty(existingUser.Cnpj) && existingUser.Cnpj == requestUser.Cnpj)
		{
			errors.Add(PliromiUserMessagesErrors.AlreadyRegisteredCnpj);
		}

		if (!string.IsNullOrEmpty(existingUser.Email)&& existingUser.Email == requestUser.Email)
		{
			errors.Add(PliromiUserMessagesErrors.AlreadyRegisteredEmail);
		}

		if (errors.Count != 0)
		{
			throw new AlreadyRegisteredException(errors);
		}
		
	}
}