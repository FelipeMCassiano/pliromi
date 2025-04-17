namespace Pliromi.Domain.Security.Cryptography;

public interface IPasswordEncrypter
{
	string Encrypt(string password);
	bool IsValid(string password, string passwordHash);
}