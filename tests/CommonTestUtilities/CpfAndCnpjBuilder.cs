using Bogus;
using Bogus.Extensions.Brazil;

namespace CommonTestUtilities;

public static class CpfAndCnpjBuilder
{

	public static string Cpf()
	{
		return new Faker<string>().CustomInstantiator( f => new string(f.Person.Cpf().Where(char.IsDigit).ToArray()));

	}

	public static string Cnpj()
	{
		return new Faker<string>().CustomInstantiator(f => new string(f.Company.Cnpj().Where(char.IsDigit).ToArray()));
	}
}