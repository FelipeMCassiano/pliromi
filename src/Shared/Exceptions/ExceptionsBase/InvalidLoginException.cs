namespace Exceptions.ExceptionsBase;

public class InvalidLoginException: PliromiException
{
	public InvalidLoginException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		// invalid email or password
		return [Message];
	}
}