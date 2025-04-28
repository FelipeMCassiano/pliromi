namespace Exceptions.ExceptionsBase;

public class ErrorOnRegisteringPliromiKey : PliromiException
{
	public ErrorOnRegisteringPliromiKey(string message) : base(message)
	{
		
	}

	public override List<string> GetErrorMessages()
	{
		return [Message];
	}
}