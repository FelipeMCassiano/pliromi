using Pliromi.Domain.Security.Tokens;

namespace Pliromi.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string Value()
	{
		var auth = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
		
		return auth["Bearer ".Length..].Trim();
	}
}