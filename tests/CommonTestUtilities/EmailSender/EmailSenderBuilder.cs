using Moq;
using Pliromi.Domain.Services.EmailService;

namespace CommonTestUtilities.EmailSender;

public static class EmailSenderBuilder
{
	public static IEmailSender Build()
	{
		return new Mock<IEmailSender>().Object;

	}
	
}