namespace Exceptions.ExceptionsBase;

public class UnauthorizedException : PliromiException
{
	public UnauthorizedException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}