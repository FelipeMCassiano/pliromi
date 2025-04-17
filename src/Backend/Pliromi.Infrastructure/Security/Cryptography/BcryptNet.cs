using Pliromi.Domain.Security.Cryptography;

namespace Pliromi.Infrastructure.Security.Cryptography;

public class BcryptNet : IPasswordEncrypter
{
	public string Encrypt(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password);
	}

	public bool IsValid(string password, string passwordHash)
	{
		return BCrypt.Net.BCrypt.Verify(password, passwordHash);
	}
}