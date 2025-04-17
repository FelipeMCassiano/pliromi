using AutoMapper;
using Communication.Requests;
using Communication.Responses;
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
		
		var existUser = await _userReadOnlyRepository.ActiveUserWithCpfOrEmailOrCnpj(user);
		
		if (existUser is not null){
			VerifyWhichFieldIsDuplicated(existUser, user);
		}
		Console.WriteLine(request.Password);
		
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
		
		if (validationResult.AnyErrors)
		{ 
			throw new ErrorOnValidationException(validationResult.ToString()!);
		}
	}

	private void VerifyWhichFieldIsDuplicated(Domain.Entities.User existingUser,
		Domain.Entities.User requestUser)
	{
		var errors = new List<string>();
		
		// these messages need to be a universal constant
		if (!string.IsNullOrEmpty(existingUser.Cpf)&& existingUser.Cpf == requestUser.Cpf)
		{
			errors.Add("Already Registered Cpf");
		}

		if (!string.IsNullOrEmpty(existingUser.Cnpj) && existingUser.Cnpj == requestUser.Cnpj)
		{
			errors.Add("Already Registered Cpnj");
		}

		if (!string.IsNullOrEmpty(existingUser.Email)&& existingUser.Email == requestUser.Email)
		{
			errors.Add("Already Registered Email");
		}

		if (errors.Count != 0)
		{
			throw new AlreadyRegisteredException(errors);
		}
		
	}
}