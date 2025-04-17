using System.Transactions;

namespace Exceptions.ExceptionsBase;

public class UnauthorizedTransactionException : PliromiException 
{
	public UnauthorizedTransactionException(string message) : base(message)
	{
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}