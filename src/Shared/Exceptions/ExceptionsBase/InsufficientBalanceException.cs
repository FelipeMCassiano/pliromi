namespace Exceptions.ExceptionsBase;

public class InsufficientBalanceException : PliromiException 
{
	public InsufficientBalanceException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}