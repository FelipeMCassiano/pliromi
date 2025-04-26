using Pliromi.Domain.Entities.Events;

namespace Pliromi.Domain.Services.EmailService;

public interface IEmailSender
{
	Task SendEmail(TransactedEventConsumer eventConsumed);

}