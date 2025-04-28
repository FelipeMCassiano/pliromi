using Bogus;
using Bogus.Extensions.Brazil;
using Communication.Enums;
using Communication.Requests;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterUserStoreBuilder
{
	
	
	public static RequestRegisterUser Build()
	{
		return new Faker<RequestRegisterUser>()
		       .RuleFor(r => r.FullName, f => f.Person.FullName)
		       .RuleFor(r => r.Email, (f, r) => f.Internet.Email(r.FullName).ToLower() )
		       .RuleFor(r => r.Password, f => f.Internet.Password())
		       .RuleFor(r => r.Balance, _ => 10000)
		       .RuleFor(r => r.IsStore, _ => true)
		       .RuleFor(r => r.PliromiKeyType, PliromiKeyType.Cnpj)
		       .RuleFor(r => r.Cnpj, f => new string(f.Company.Cnpj().Where(char.IsDigit).ToArray()))
		       .Generate()
		       ;
	}
	
}