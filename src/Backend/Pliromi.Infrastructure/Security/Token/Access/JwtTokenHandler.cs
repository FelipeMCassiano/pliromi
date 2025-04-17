using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Pliromi.Infrastructure.Security.Token.Access;

public abstract class JwtTokenHandler
{
	protected static SymmetricSecurityKey SecurityKey(string key)
	{
		var bytes = Encoding.UTF8.GetBytes(key);
		
		return new SymmetricSecurityKey(bytes);
	}
	
}