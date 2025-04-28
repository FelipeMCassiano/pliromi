namespace Exceptions.ExceptionsBase;

public class ErrorOnValidationException : PliromiException
{
	private readonly List<string> _errors;

	public ErrorOnValidationException(List<string> messages) : base(string.Join(",", messages))
	{
		_errors = messages;
	}
	public override List<string> GetErrorMessages()
	{
		return _errors;
	}
}