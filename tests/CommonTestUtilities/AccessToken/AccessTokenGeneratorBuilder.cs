using Pliromi.Domain.Security.Tokens;
using Pliromi.Infrastructure.Security.Token.Access.Generator;

namespace CommonTestUtilities.AccessToken;

public static class AccessTokenGeneratorBuilder
{
	public static IAccessTokenGenerator Build()
	{
		var signingKey = "aBcDeFgHiJkLmNoPqRsTuVwXyZ0123456789+/==";
		return new JwtTokenGenerator(signingKey: signingKey);
	}
}