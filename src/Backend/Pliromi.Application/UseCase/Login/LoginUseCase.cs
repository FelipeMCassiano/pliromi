using Communication.Requests;
using Communication.Responses;
using Exceptions;
using Exceptions.ExceptionsBase;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Security.Cryptography;
using Pliromi.Domain.Security.Tokens;
using Validot;

namespace Pliromi.Application.UseCase.Login;

public class LoginUseCase : ILoginUseCase
{
	private readonly IUserReadOnlyRepository _userReadOnlyRepository;
	private readonly IAccessTokenGenerator _accessTokenGenerator;
	private readonly IPasswordEncrypter _passwordEncrypter;
	private readonly IValidator<RequestLogin> _validator;

	public LoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, IAccessTokenGenerator accessTokenGenerator, IPasswordEncrypter passwordEncrypter, IValidator<RequestLogin> validator)
	{
		_userReadOnlyRepository = userReadOnlyRepository;
		_accessTokenGenerator = accessTokenGenerator;
		_passwordEncrypter = passwordEncrypter;
		_validator = validator;
	}

	public async Task<ResponseRegister> Execute(RequestLogin request)
	{
		Validate(request);
		
		var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
		if (user == null ||!_passwordEncrypter.IsValid(request.Password, user.Password))
		{
			throw new InvalidLoginException(PliromiLoginMessagesErrors.InvalidCredentials);
		}

		return new ResponseRegister()
		{
			Name = user.Fullname,
			Tokens = [_accessTokenGenerator.Generate(user.UserIdentifier)]
		};
	}

	private void Validate(RequestLogin request)
	{
		var validationResult = _validator.Validate(request);
		if (!validationResult.AnyErrors) return;
		
		var errors = validationResult.MessageMap.Values.SelectMany(list => list).Distinct().ToList();
		throw new ErrorOnValidationException(errors);
	}
}