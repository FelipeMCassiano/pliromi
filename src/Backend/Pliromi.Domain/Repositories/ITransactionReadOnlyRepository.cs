using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Repositories;

public interface ITransactionReadOnlyRepository
{
	Task<List<Transaction>> GetLastTransactions(Guid id);
}