using Pliromi.Domain.Security.Cryptography;
using Pliromi.Infrastructure.Security.Cryptography;

namespace CommonTestUtilities.Cryptography;

public static class PasswordEncrypterBuilder
{
	public static IPasswordEncrypter Build()
	{
		return new BcryptNet();
	}
	
}