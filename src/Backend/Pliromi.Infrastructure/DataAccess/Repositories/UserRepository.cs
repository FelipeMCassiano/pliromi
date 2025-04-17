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


	public Task<User?> GetReceiver(ReceiverDataForTransaction receiver)
	{
		var query = _dbContext.Users.Where(u => u.IsActive);
		if (!string.IsNullOrEmpty(receiver.ReceiverCpf))
		{
			query = query.Where(u => u.Cpf == receiver.ReceiverCpf);
		}

		if (!string.IsNullOrEmpty(receiver.ReceiverEmail))
		{
			query = query.Where(u => u.Email == receiver.ReceiverEmail);
		}

		if (!string.IsNullOrEmpty(receiver.ReceiverCnpj))
		{
			query = query.Where(u => u.Cnpj == receiver.ReceiverCnpj && u.IsStore);
		}
		
		return query.FirstOrDefaultAsync();
	}

	public Task<bool> ExistActiveUserWithIdentifierAsync(Guid identifier)
	{
		return _dbContext.Users.Where(u => u.IsActive && u.UserIdentifier == identifier).AsNoTracking().AnyAsync();
	}

	public Task<User?> GetUserByEmail(string email)
	{
		return _dbContext.Users.Where(u => u.Email == email).AsNoTracking().FirstOrDefaultAsync();
	}

	public Task<User?> ActiveUserWithCpfOrEmailOrCnpj(User user)
	{
		return _dbContext.Users
		                 .Where(u => u.IsActive && (u.Cpf == user.Cpf || u.Email == user.Email || u.Cnpj == user.Cnpj)).AsNoTracking()
		                 .FirstOrDefaultAsync();
	}

	public async Task<User> GetUser(User user)
	{
		return await _dbContext.Users.Where(u => u.Id == user.Id && u.IsActive
		       ).FirstAsync();
	}
}