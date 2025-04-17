using Pliromi.Domain.Repositories;

namespace Pliromi.Infrastructure.DataAccess;

public class UnitOfWork : IUnitOfWork
{
	private readonly PliromiDbContext _dbContext;

	public UnitOfWork(PliromiDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Commit()
	{
		await _dbContext.SaveChangesAsync();
	}
}