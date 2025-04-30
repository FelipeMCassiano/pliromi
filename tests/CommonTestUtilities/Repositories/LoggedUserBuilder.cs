using Moq;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Repositories;

public static class LoggedUserBuilder
{
	public static ILoggedUser Build(User user)
	{
		var mock = new Mock<ILoggedUser>();
		mock.Setup(x => x.User()).ReturnsAsync(user);
		return mock.Object;
	}
	
}