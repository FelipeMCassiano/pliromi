using Pliromi.Domain.Entities;

namespace Pliromi.Domain.Services.LoggedUser;

public interface ILoggedUser
{
	Task<User> User();
}