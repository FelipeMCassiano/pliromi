namespace Pliromi.Domain.Security.Tokens;

public interface IAccessTokenValidator
{
	Guid ValidateAndGetUserIdentifier(string token);
	
}