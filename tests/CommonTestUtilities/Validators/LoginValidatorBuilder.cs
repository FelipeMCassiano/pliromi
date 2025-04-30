using Communication.Requests;
using Pliromi.Application.UseCase.Login;
using Validot;

namespace CommonTestUtilities.Validators;

public static class LoginValidatorBuilder
{
	public static IValidator<RequestLogin> Build()
	{
		return Validator.Factory.Create(LoginSpecification.CreateSpecification());
	}
	
}