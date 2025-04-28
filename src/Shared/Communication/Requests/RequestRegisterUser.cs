namespace Communication.Requests;

public class RequestRegisterUser
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public int Balance { get; set; } 
	public string FullName { get; set; } = string.Empty;
	public string? Cpf { get; set; }
	public string? Cnpj { get; set; }
	public bool IsStore { get; set; }
}