using Bogus;
using Bogus.Extensions.Brazil;
using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Repositories;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class UserBuilder
{
	
	public static (User user, string Password) BuildPerson()
	{
		var password = new Faker().Internet.Password();


		var user = BuildCommonsFields(password) 
		           .RuleFor(u => u.Id, Guid.Parse(PlirmoiTestConstants.UserId))
		           .RuleFor(u => u.IsStore, false)
		           .RuleFor(u => u.Balance, PlirmoiTestConstants.UserBalance)
		           .RuleFor(u => u.Cpf, _ => CpfAndCnpjBuilder.Cpf())
		           .RuleFor(u => u.PliromiKey, (_, u) => PliromiKeyBuilder.Build(u.Cpf!, PliromiKeyType.Cpf));
			
		
		return (user, password);
	}

	private static Faker<User> BuildCommonsFields(string password)
	{
		
		var passwordEncrypter = PasswordEncrypterBuilder.Build();

		var baseUser = new Faker<User>()
		               .RuleFor(u => u.Fullname, f => f.Person.FullName)
		               .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Fullname).ToLower())
		               .RuleFor(u => u.Password, passwordEncrypter.Encrypt(password))
		               .RuleFor(u => u.UserIdentifier, Guid.NewGuid());
		return baseUser;
	}

	public static (User user, string password) BuildStore()
	{
		var password = new Faker().Internet.Password();
			var user = BuildCommonsFields(password) 
		           .RuleFor(u => u.Id, Guid.Parse(PlirmoiTestConstants.StoreId))
		           .RuleFor(u => u.IsStore, true)
		           .RuleFor(u => u.Balance, PlirmoiTestConstants.UserBalance)
		           .RuleFor(u => u.Cnpj, _ => CpfAndCnpjBuilder.Cnpj())
		           .RuleFor(u => u.PliromiKey, (_, u) => PliromiKeyBuilder.Build(u.Cnpj!, PliromiKeyType.Cnpj));
		return (user, password);
	}
	
}