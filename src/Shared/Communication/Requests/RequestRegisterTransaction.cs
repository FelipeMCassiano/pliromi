namespace Communication.Requests;

public class RequestRegisterTransaction
{
	public int Value { get; set; }
	public string? ReceiverCpf { get; set; } 
	public string? ReceiverCnpj { get; set; } 
	public string? ReceiverEmail { get; set; }
}