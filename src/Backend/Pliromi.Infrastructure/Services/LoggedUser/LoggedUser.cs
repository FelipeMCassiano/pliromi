using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Security.Tokens;
using Pliromi.Domain.Services.LoggedUser;
using Pliromi.Infrastructure.DataAccess;

namespace Pliromi.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
	private readonly PliromiDbContext _dbContext;
	private readonly ITokenProvider _tokenProvider;

	public LoggedUser(PliromiDbContext dbContext, ITokenProvider tokenProvider)
	{
		_dbContext = dbContext;
		_tokenProvider = tokenProvider;
	}

	public async Task<User> User()
	{
		var token = _tokenProvider.Value();

		var tokenHandler = new JwtSecurityTokenHandler();
		var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
		var identifier = Guid.Parse(jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value);
		
		return await  _dbContext.Users.AsNoTracking().FirstAsync(u => u.IsActive && u.UserIdentifier == identifier);
	}


}