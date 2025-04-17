namespace Exceptions.ExceptionsBase;

public class NotFoundUserException : PliromiException
{
	public NotFoundUserException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}