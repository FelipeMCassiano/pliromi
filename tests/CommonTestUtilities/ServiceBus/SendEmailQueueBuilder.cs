using Moq;
using Pliromi.Domain.Services.ServiceBus;
using Pliromi.Infrastructure.Services.ServiceBus;

namespace CommonTestUtilities.ServiceBus;

public static class SendEmailQueueBuilder
{
	public static ISendEmailQueue Build()
	{
		var mock = new Mock<ISendEmailQueue>();
		return mock.Object;
	}
	
	
}