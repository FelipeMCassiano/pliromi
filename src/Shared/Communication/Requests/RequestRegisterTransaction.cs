namespace Communication.Requests;

public class RequestRegisterTransaction
{
	public decimal Value { get; set; }
	public string Sender { get; set; } = string.Empty;
	public string? ReceiverCpf { get; set; } 
	public string? ReceiverCnpj { get; set; } 
	public string? ReceiverEmail { get; set; }
}