using Moq;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public  class UserReadOnlyRepositoryBuilder
{
	private readonly Mock<IUserReadOnlyRepository> _mock;

	public UserReadOnlyRepositoryBuilder()
	{
		_mock = new Mock<IUserReadOnlyRepository>();
	}
	public IUserReadOnlyRepository Build()
	{
		return _mock.Object;
	}

	public void ExistActiveUserWithIdentifierAsync(Guid identifier)
	{
		_mock.Setup(x => x.ExistActiveUserWithIdentifierAsync(identifier)).ReturnsAsync(true);
	}

	public void GetUserByEmail(User user)
	{
		_mock.Setup(x => x.GetUserByEmail(user.Email)).ReturnsAsync(user);
	}

	public void ActiveUserWithCpfOrEmailOrCpnj(User user)
	{
		_mock.Setup(x => x.GetActiveUserByIdentifiersAsync(user)).ReturnsAsync(user);
	}
	
}