using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Pliromi.Domain.Entities.Events;
using Pliromi.Domain.Services.EmailService;

namespace Pliromi.Infrastructure.Services.EmailService;

public class AmazonSimpleEmailSender: IEmailSender
{
	private readonly IAmazonSimpleEmailService _simpleEmailService;
	private readonly string _source;

	public AmazonSimpleEmailSender(IAmazonSimpleEmailService simpleEmailService, string source)
	{
		_simpleEmailService = simpleEmailService;
		_source = source;
	}

	public async Task SendEmail(TransactedEventConsumer eventConsumed)
	{
		Console.WriteLine(eventConsumed.SenderEmail);
		var emailRequest = new SendEmailRequest
		{
			Source = _source,
			Destination = new Destination
			{
				ToAddresses = [eventConsumed.SenderEmail]
			},
			Message = new Message
			{
				Body = new Body
				{
					Text = new Content()
					{
						Charset = "UTF-8",
						Data = ApplyTemplate(eventConsumed)
					}
				},
				Subject = new Content()
				{
					Data = "Transaction Successful"
				}
			},
			
		};
			var response = await _simpleEmailService.SendEmailAsync(emailRequest);
			Console.WriteLine($"response : {response.HttpStatusCode}");
	}

	private static string ApplyTemplate(TransactedEventConsumer eventConsumed)
	{
		var paymentMessage = $@"
Amount: R${eventConsumed.Value}

To: {eventConsumed.ReceiverName}

Key: {eventConsumed.Key}

Date: {eventConsumed.Date}

Transaction Id: {eventConsumed.TransactionId}";
		return paymentMessage;

	}
}
