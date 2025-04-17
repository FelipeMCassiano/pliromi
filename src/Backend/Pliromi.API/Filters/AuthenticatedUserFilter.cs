using Communication.Responses;
using Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Security.Tokens;

namespace Pliromi.API.Filters;

public class AuthenticatedUserFilter: IAsyncAuthorizationFilter
{
	private readonly IUserReadOnlyRepository _repository;
	private readonly IAccessTokenValidator _accessTokenValidator;

	public AuthenticatedUserFilter(IUserReadOnlyRepository repository, IAccessTokenValidator accessTokenValidator)
	{
		_repository = repository;
		_accessTokenValidator = accessTokenValidator;
	}

	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
			var token = TokenOnRequest(context);
			var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

			var existUser = await _repository.ExistActiveUserWithIdentifierAsync(userIdentifier);
			if (!existUser)
			{
				// user without permission access resource
						context.Result = new UnauthorizedObjectResult(new ResponseError(string.Empty));
			}
		
	}

	private static string TokenOnRequest(AuthorizationFilterContext context)
	{
		var auth = context.HttpContext.Request.Headers.Authorization.ToString();
		if (string.IsNullOrWhiteSpace(auth))
		{
			// no token
			throw new UnauthorizedException(string.Empty);
		}
		return auth["Bearer".Length..].Trim();
	}
}