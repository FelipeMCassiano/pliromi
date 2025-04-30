using Communication.Requests;
using Exceptions;
using Validot;

namespace Pliromi.Application.UseCase.Login;

public static class LoginSpecification
{
	public static Specification<RequestLogin> CreateSpecification()
	{
		Specification<RequestLogin> s = s => s
		                                     .Member(m => m.Email, m => m
		                                                                .NotEmpty().WithMessage(
			                                                                PliromiUserMessagesErrors.InvalidEmail)
		                                                                .Email().WithMessage(PliromiUserMessagesErrors
			                                                                .InvalidEmail)
		                                                                .LengthBetween(1, 255)
		                                                                .WithMessage(PliromiUserMessagesErrors
			                                                                .InvalidEmail)
		                                     )
		                                     .And()
		                                     .Member(m => m.Password, m => m
		                                                                   .LengthBetween(1, 255)
		                                                                   .WithMessage(PliromiUserMessagesErrors
			                                                                   .PasswordLengthInvalid)
		                                     );

		return s;
	}
	
}