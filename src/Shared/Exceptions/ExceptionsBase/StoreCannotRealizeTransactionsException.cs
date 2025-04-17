namespace Exceptions.ExceptionsBase;

public class StoreCannotRealizeTransactionsException : PliromiException
{
	public StoreCannotRealizeTransactionsException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}