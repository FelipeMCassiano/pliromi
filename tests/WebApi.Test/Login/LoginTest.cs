using System.Text.Json;
using CommonTestUtilities.Requests;
using Exceptions;
using Shouldly;

namespace WebApi.Test.Login;

public class LoginTest :  PliromiClassFixture
{
	private const string Path = "/login";
	private readonly string _userEmail;
	private readonly string _password;
	public LoginTest(CustomWebApplicationFactory factory) : base(factory)
	{
		_userEmail = factory.GetUserPersonEmail();
		_password = factory.GetUserPersonPassword();
	}

	[Fact]
	public async Task Success()
	{
		var request = RequestLoginBuilder.Build(_userEmail, _password);
		var response = await DoPost(Path, request);
		response.EnsureSuccessStatusCode();
		
		await using var responseBody = await response.Content.ReadAsStreamAsync();
		var responseData = await JsonDocument.ParseAsync(responseBody);
		
		responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNull();
		responseData.RootElement.GetProperty("tokens").EnumerateArray().ShouldNotBeEmpty();
	}

	[Fact]
	public async Task ErrorInvalidCredentials()
	{
		var request = RequestLoginBuilder.Build(_userEmail, "testPassword");
		var response = await DoPost(Path, request);
		
		await using var responseBody = await response.Content.ReadAsStreamAsync();
		
		var responseData = await JsonDocument.ParseAsync(responseBody);
		var errors =responseData.RootElement.GetProperty("errors").EnumerateArray();
		errors.ShouldContain(e => e.GetString()! == PliromiLoginMessagesErrors.InvalidCredentials);
	}
}