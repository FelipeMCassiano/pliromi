using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Repositories;

public interface ITransactionWriteOnlyRepository
{
	Task AddAsync(Transaction transaction );
}