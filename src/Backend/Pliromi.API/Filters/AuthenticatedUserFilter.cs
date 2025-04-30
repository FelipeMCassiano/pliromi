using Communication.Responses;
using Exceptions;
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
		try
		{
			var token = TokenOnRequest(context);
			var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

			var existUser = await _repository.ExistActiveUserWithIdentifierAsync(userIdentifier);
			if (!existUser)
			{
						context.Result = new UnauthorizedObjectResult(new ResponseError(PliromiAuthMessagesErrors.UserWithoutPermission));
			}
		}
		catch (UnauthorizedException e)
		{
			context.Result = new UnauthorizedObjectResult(new ResponseError(e.GetErrorMessages()));
		}
			
		
	}

	private static string TokenOnRequest(AuthorizationFilterContext context)
	{
		var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
    
    if (string.IsNullOrWhiteSpace(authHeader))
    {
        throw new UnauthorizedException(PliromiAuthMessagesErrors.AuthorizationHeaderMissing);
    }

    const string bearerPrefix = "Bearer ";
    if (!authHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
    {
        throw new UnauthorizedException(PliromiAuthMessagesErrors.AuthorizationHeaderInvalid);
    }

    var token = authHeader[bearerPrefix.Length..].Trim();

    return token;
	}
}