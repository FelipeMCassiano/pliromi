using Communication.Requests;
using Validot;

namespace Pliromi.Application.UseCase.Login;

public static class LoginSpecification
{
	public static Specification<RequestLogin> CreateSpecification()
	{
		Specification<RequestLogin> s = s => s
		                                     .Member(m => m.Email, m => m.Email().And().NotEmpty())
		                                     .And()
		                                     .Member(m => m.Password, m => m.NotEmpty().And().MinLength(6));

		return s;
	}
	
}