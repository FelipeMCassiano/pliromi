using Communication.Requests;
using Pliromi.Application.UseCase.User.Register;
using Validot;

namespace CommonTestUtilities.Validators;

public static class UserRegisterValidatorBuilder
{
	public static IValidator<RequestRegisterUser> Build()
	{
		var specification = RegisterUserSpecification.CreateSpecification();
		return  Validator.Factory.Create(specification);
	}
	
} 