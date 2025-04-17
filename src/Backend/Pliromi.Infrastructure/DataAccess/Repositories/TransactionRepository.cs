using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;

namespace Pliromi.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionWriteOnlyRepository
{
	private readonly PliromiDbContext _dbContext;

	public TransactionRepository(PliromiDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task AddAsync(Transaction transaction)
	{
	await	_dbContext.Transactions.AddAsync(transaction);
		
	}
}