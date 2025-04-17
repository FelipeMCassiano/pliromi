using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Repositories;

public interface IUserUpdateOnlyRepository
{
	Task<User> GetUser(User user);
	Task<User?> GetReceiver(ReceiverDataForTransaction receiver);
}