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
			Console.WriteLine($"user identifier: {userIdentifier}");

			var existUser = await _repository.ExistActiveUserWithIdentifierAsync(userIdentifier);
			if (!existUser)
			{
				// user without permission access resource
						context.Result = new UnauthorizedObjectResult(new ResponseError("user without permission"));
			}
		
	}

	private static string TokenOnRequest(AuthorizationFilterContext context)
	{
		var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
    
    if (string.IsNullOrWhiteSpace(authHeader))
    {
        throw new UnauthorizedException("Authorization header is missing");
    }

    const string bearerPrefix = "Bearer ";
    if (!authHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
    {
        throw new UnauthorizedException("Authorization header must start with 'Bearer '");
    }

    // Extract token after Bearer prefix
    var token = authHeader[bearerPrefix.Length..].Trim();


    return token;
	}
}