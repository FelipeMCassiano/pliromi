using Microsoft.EntityFrameworkCore;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;

namespace Pliromi.Infrastructure.DataAccess.Repositories;

public class TransactionRepository : ITransactionWriteOnlyRepository, ITransactionReadOnlyRepository
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

	public async Task<List<Transaction>> GetLastTransactions(Guid userId)
	{
		return await _dbContext.Transactions.Where(t => t.SenderId == userId || t.ReceiverId == userId).OrderByDescending(t => t.CreatedOn).Include(t => t.Receiver).Include(t=> t.Sender).AsNoTracking()
		                       .ToListAsync();
	}
}