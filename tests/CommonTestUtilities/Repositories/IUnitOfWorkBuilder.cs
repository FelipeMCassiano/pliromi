using Moq;
using Pliromi.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public static class IUnitOfWorkBuilder
{
	public static IUnitOfWork Build()
	{
		var mock = new Mock<IUnitOfWork>();
		return mock.Object;
	}
	
}