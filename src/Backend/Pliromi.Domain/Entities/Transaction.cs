namespace Pliromi.Domain.Entities;

public class Transaction
{
	public Guid Id { get;  set; }
	public decimal Value { get; set; }
	
	public Guid SenderId { get;  set; }
	public User Sender { get; set; } = null!;
	public Guid ReceiverId { get;  set; }
	public User Receiver { get; set; } = null!;
}