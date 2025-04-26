using Pliromi.Domain.Entities;
using Pliromi.Domain.Entities.Events;

namespace Pliromi.Domain.Services.ServiceBus;

public interface ISendEmailQueue
{
	Task SendMessage(TransactedEventConsumer transaction);

}