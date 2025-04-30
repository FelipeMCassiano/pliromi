using Amazon.SimpleEmail.Model.Internal.MarshallTransformations;
using Moq;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
	private readonly Mock<IUserUpdateOnlyRepository> _mock;

	public UserUpdateOnlyRepositoryBuilder()
	{
		_mock = new Mock<IUserUpdateOnlyRepository>();
	}

	public IUserUpdateOnlyRepository Build(User user, User? receiver = null)
	{
		
		_mock.Setup(x => x.GetUser(user)).ReturnsAsync(user);
		GetReceiverByPliromiKey(receiver);
		return _mock.Object;
	}

	private void GetReceiverByPliromiKey(User? receiver = null)
	{
		if (receiver == null)
		{
			_mock.Setup(x => x.GetReceiverByPliromiKey("falseKey")).ReturnsAsync((User?)null);
			return;
		}
		_mock.Setup(x => x.GetReceiverByPliromiKey(receiver.PliromiKey.Key)).ReturnsAsync(receiver);
	}

}