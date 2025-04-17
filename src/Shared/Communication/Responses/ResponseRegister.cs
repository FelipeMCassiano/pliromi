namespace Communication.Responses;

public class ResponseRegister
{
	public string Name { get; set; } = string.Empty;
	public List<string> Tokens { get; set; } = [];
}