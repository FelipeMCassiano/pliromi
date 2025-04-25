namespace Communication.Responses;

public class ResponseDashboardTransaction
{
	public int Value { get; set; }
	public ResponseDashboardUser Receiver { get; set; } = null!;
	public ResponseDashboardUser Sender { get; set; } = null!;
	public DateTime Date { get; set; }
}