using Microsoft.EntityFrameworkCore;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;

namespace Pliromi.Infrastructure.DataAccess.Repositories;

public class UserRepository : IUserWriteOnlyRepository , IUserReadOnlyRepository, IUserUpdateOnlyRepository
{
	private readonly PliromiDbContext _dbContext;
	

	public UserRepository(PliromiDbContext dbContext)
	{ 
		_dbContext = dbContext;
	}

	public async Task AddAsync(User user)
	{
		await _dbContext.Users.AddAsync(user);
	}


	public Task<User?> GetReceiverByPliromiKey(string pliromiKey)
	{
		return _dbContext.Users.Where(u => u.IsActive && u.PliromiKey.Key == pliromiKey).FirstOrDefaultAsync();
	}

	public async Task<bool> ExistActiveUserWithIdentifierAsync(Guid identifier)
	{
		return await _dbContext.Users.Where(u => u.IsActive && u.UserIdentifier == identifier).AsNoTracking().AnyAsync();
	}

	public async Task<User?> GetUserByEmail(string email)
	{
		return await _dbContext.Users.Where(u => u.Email == email).AsNoTracking().FirstOrDefaultAsync();
	}

	public async Task<User?> GetActiveUserByIdentifiersAsync(User user)
	{
	return await _dbContext.Users
        .AsNoTracking()
        .Where(u => u.IsActive && ( u.Cpf != null && u.Cpf == user.Cpf) || 
                   u.Email == user.Email || 
                   (u.Cnpj != null && u.Cnpj == user.Cnpj))
        .FirstOrDefaultAsync();
	}

	public async Task<string> GetPliromiKeyByUserId(Guid userId)
	{
		return await _dbContext.PliromiKeys.AsNoTracking().Where(p => p.UserId == userId).Select(p => p.Key).FirstAsync();
	}

	public async Task<User> GetUser(User user)
	{
		return await _dbContext.Users.Where(u => u.Id == user.Id && u.IsActive
		       ).FirstAsync();
	}
}