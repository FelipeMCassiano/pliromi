using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;

public class PliromiClassFixture : IClassFixture<CustomWebApplicationFactory>
{
	private readonly HttpClient _client;

	public PliromiClassFixture(CustomWebApplicationFactory factory)
	{
		_client = factory.CreateClient();
	}
	// Todo: GET, POST
	protected async Task<HttpResponseMessage> DoPost<T>(string path, T content, string token = "")
	{
		AuthorizeRequest(token);
		return await _client.PostAsJsonAsync(path, content);
	}

	protected async Task<HttpResponseMessage> DoGet(string path, string token = "")
	{
		AuthorizeRequest(token);
		return await _client.GetAsync(path);
	}

	
	
	private void AuthorizeRequest(string token)
	{
		if (string.IsNullOrWhiteSpace(token)) return;

		_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}
}
