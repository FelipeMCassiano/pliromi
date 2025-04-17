namespace Exceptions.ExceptionsBase;

public class ErrorOnValidationException : PliromiException 
{
	public ErrorOnValidationException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}