using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Pliromi.Domain.Security.Tokens;

namespace Pliromi.Infrastructure.Security.Token.Access.Generator;

public class JwtTokenGenerator: JwtTokenHandler, IAccessTokenGenerator
{
	private readonly string _signingKey;

	public JwtTokenGenerator(string signingKey)
	{
		_signingKey = signingKey;
	}

	public string Generate(Guid userIdentifier)
	{
		var claims = new List<Claim>
		{
			new(ClaimTypes.Sid, userIdentifier.ToString())
		};
		var tokenDescriptor = new SecurityTokenDescriptor()
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(5),
			SigningCredentials =
				new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature),
		};
		
		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(securityToken);
	}
}