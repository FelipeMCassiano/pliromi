namespace Exceptions.ExceptionsBase;

public class ErrorOnSendingEmail : PliromiException
{
	public ErrorOnSendingEmail(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}