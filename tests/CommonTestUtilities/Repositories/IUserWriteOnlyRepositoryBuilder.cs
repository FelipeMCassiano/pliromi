using Moq;
using Pliromi.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public static class IUserWriteOnlyRepositoryBuilder
{
	public static IUserWriteOnlyRepository Build()
	{
		var mock = new Mock<IUserWriteOnlyRepository>();
		return mock.Object;
	}
	
}