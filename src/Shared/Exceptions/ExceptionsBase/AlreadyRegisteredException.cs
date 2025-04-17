namespace Exceptions.ExceptionsBase;

public class AlreadyRegisteredException : PliromiException
{
	private readonly List<string> _errors;
	public AlreadyRegisteredException(List<string> errors) : base(string.Empty)
	{
		_errors = errors;
	}

	public override List<string> GetErrorMessages()
	{
		return _errors;
	}
}