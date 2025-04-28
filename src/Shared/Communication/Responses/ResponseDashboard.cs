namespace Communication.Responses;

public class ResponseDashboard
{
	public string PliromiKey { get; set; } = string.Empty;
	public int Income { get; set; }
	public int Outcome { get; set; }
	public int Balance { get; set; }
	public List<ResponseDashboardTransaction> LastTransactions { get; set; } = [];
}