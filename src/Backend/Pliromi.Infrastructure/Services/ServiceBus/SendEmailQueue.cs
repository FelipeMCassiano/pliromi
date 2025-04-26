using MassTransit;
using Pliromi.Domain.Entities.Events;
using Pliromi.Domain.Services.ServiceBus;

namespace Pliromi.Infrastructure.Services.ServiceBus;

public class SendEmailQueue : ISendEmailQueue
{
	private readonly IBus _bus;

	public SendEmailQueue(IBus bus)
	{
		_bus = bus;
	}

	public async Task SendMessage(TransactedEventConsumer transaction)
	{
		await _bus.Publish(transaction);
	}
}