using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Repositories;

public interface IUserReadOnlyRepository
{
	Task<bool> ExistActiveUserWithIdentifierAsync(Guid identifier);
	Task<User?> GetUserByEmail(string email);
	Task<User?> GetActiveUserByIdentifiersAsync(User user);
	Task<string> GetPliromiKeyByUserId(Guid userId);
}