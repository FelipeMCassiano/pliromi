
namespace Communication.Requests;

public class RequestRegisterTransaction
{
	public int Value { get; set; }
	public string PliromiKey { get; set; } = string.Empty;
}