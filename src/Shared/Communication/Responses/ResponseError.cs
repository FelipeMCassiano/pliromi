namespace Communication.Responses;

public class ResponseError
{
	private readonly IList<string> _errors;

	public ResponseError(IList<string> errors)
	{
		_errors = errors;
	}
	
	public ResponseError(string error)
	{
		_errors = [error];
	}
	public IEnumerable<string> Errors => _errors;
}