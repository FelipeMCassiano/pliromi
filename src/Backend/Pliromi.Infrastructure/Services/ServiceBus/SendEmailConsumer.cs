using Amazon.SimpleEmail;
using MassTransit;
using Pliromi.Domain.Entities.Events;
using Pliromi.Domain.Services.EmailService;

namespace Pliromi.Infrastructure.Services.ServiceBus;

public class SendEmailConsumer : IConsumer<TransactedEventConsumer>
{
	private readonly IEmailSender _emailSender;

	public SendEmailConsumer(IEmailSender emailSender)
	{
		_emailSender = emailSender;
	}

	public async Task Consume(ConsumeContext<TransactedEventConsumer> context)
	{
		var eventMessage = context.Message;

		try
		{
			await _emailSender.SendEmail(eventMessage);

		}
		catch (AmazonSimpleEmailServiceException ex)
		{
			throw new Exception(ex.Message);
		}
	}
}
