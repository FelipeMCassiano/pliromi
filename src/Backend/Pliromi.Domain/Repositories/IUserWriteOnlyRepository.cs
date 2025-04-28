using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Repositories;

public interface IUserWriteOnlyRepository
{
	Task AddAsync(User user );
} 