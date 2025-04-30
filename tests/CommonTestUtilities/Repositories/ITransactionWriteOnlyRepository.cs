using Moq;
using Pliromi.Domain.Repositories;

namespace CommonTestUtilities.Repositories;

public static class TransactionWriteOnlyRepositoryBuilder
{
	public static ITransactionWriteOnlyRepository Build()
	{
		var mock = new Mock<ITransactionWriteOnlyRepository>();
		return mock.Object;
	}
	
	
}