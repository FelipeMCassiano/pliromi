namespace Exceptions.ExceptionsBase;

public abstract class PliromiException : SystemException
{
	protected  PliromiException(string message) : base(message)
	{
		
	}
	public abstract List<string> GetErrorMessages();
}