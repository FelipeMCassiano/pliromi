using Bogus;
using Bogus.Extensions.Brazil;
using Communication.Enums;
using Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserPersonBuilder
{
	public static RequestRegisterUser Build()
	{
		return new Faker<RequestRegisterUser>()
		       .RuleFor(r => r.FullName, f => f.Person.FullName)
		       .RuleFor(r => r.Email, (f, r) => f.Internet.Email(r.FullName).ToLower() )
		       .RuleFor(r => r.Password, f => f.Internet.Password())
		       .RuleFor(r => r.Cpf, f => new string(f.Person.Cpf().Where(char.IsDigit).ToArray()))
		       .RuleFor(r => r.Balance, _ => PlirmoiTestConstants.UserBalance)
		       .RuleFor(r => r.IsStore, _ => false)
		       .RuleFor(r => r.PliromiKeyType, PliromiKeyType.Cpf)
		       .Generate()
		       ;
	}
	
}