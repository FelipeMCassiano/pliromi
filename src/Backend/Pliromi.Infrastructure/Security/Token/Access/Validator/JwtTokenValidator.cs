using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Pliromi.Domain.Security.Tokens;

namespace Pliromi.Infrastructure.Security.Token.Access.Validator;

public class JwtTokenValidator  : JwtTokenHandler, IAccessTokenValidator
{
	private readonly string _signingKey;

	public JwtTokenValidator(string signingKey)
	{
		_signingKey = signingKey;
	}

	public Guid ValidateAndGetUserIdentifier(string token)
	{
		var validationParameter = new TokenValidationParameters()
		{
			ValidateAudience = false,
			ValidateIssuer = false,
			IssuerSigningKey = SecurityKey(_signingKey),
			ClockSkew = TimeSpan.Zero
		};
		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler.ValidateToken(token, validationParameter, out _);
		var userIdentifier = principal.Claims.First(c => c.Type == ClaimTypes.Sid).Value;
		
		return Guid.Parse(userIdentifier);

	}
}