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

	public RegisterUserUseCase(IUserWriteOnlyRepository userWriteOnlyRepository, IUnitOfWork unitOfWork, IMapper mapper, IValidator<RequestRegisterUser> validator, IAccessTokenGenerator accessTokenGenerator, IPasswordEncrypter passwordEncrypter)
	{
		_userWriteOnlyRepository = userWriteOnlyRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_validator = validator;
		_accessTokenGenerator = accessTokenGenerator;
		_passwordEncrypter = passwordEncrypter;
	}

	public async Task<ResponseRegister> Execute(RequestRegisterUser request)
	{
		Validate(request);
		
		var user = _mapper.Map<Domain.Entities.User>(request);
		
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
}