namespace WebApi.Test;

public class PliromiClassFixture : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public PliromiClassFixture(CustomWebApplicationFactory factory)
	{
		_client = factory.CreateClient();
	}
	// Todo: GET, POST
}
