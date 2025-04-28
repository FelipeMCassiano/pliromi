using Bogus;
using Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserPersonBuilder
{
	public static RequestRegisterUser Build()
	{
		return new Faker<RequestRegisterUser>()
			.RuleFor(r => r.Email, f => f.Person.Email)
	}
	
}