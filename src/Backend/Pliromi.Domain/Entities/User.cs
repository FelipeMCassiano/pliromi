namespace Pliromi.Domain.Entities;

public class User  
{
	public Guid Id { get;  set; }
	public bool IsActive { get;  set; } = true;
	public string Fullname { get;  set; } = string.Empty;
	public decimal Balance { get;  set; }
	public string Email { get;  set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string? Cpf { get;  set; }
	public string? Cnpj { get;  set; }
	public bool IsStore { get;  set; }
	public DateTime CreatedOn { get;  set; } = DateTime.UtcNow;
public Guid UserIdentifier { get;  set; } 
	public List<Transaction> SentTransactions { get; set; } = [];
	public List<Transaction> ReceivedTransactions { get; set; } = [];
}