namespace Pliromi.Domain.Entities.Events;

public class TransactedEventConsumer
{
	public Guid TransactionId { get; init; }
	public int Value { get; init; }
	public string ReceiverName { get; init; } = string.Empty;
	public string SenderEmail { get; init; } = string.Empty;
	public string Key { get; set; } = string.Empty;
	public DateTime Date { get; init; }
	
};


	
	
