using System.Net;
using System.Text.Json;
using CommonTestUtilities.Requests;
using Exceptions;
using MassTransit.Mediator;
using Shouldly;
using Xunit.Abstractions;

namespace WebApi.Test.User.Register;

public class RegisterUserTest : PliromiClassFixture
{
	private readonly ITestOutputHelper _testOutputHelper;
	private const string Path = "/user";
	public RegisterUserTest(CustomWebApplicationFactory factory, ITestOutputHelper testOutputHelper) : base(factory)
	{
		_testOutputHelper = testOutputHelper;
	}

	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		var response = await DoPost(Path, request);

		response.EnsureSuccessStatusCode();
		
		await using var responseBody = await response.Content.ReadAsStreamAsync();
		var responseData = await JsonDocument.ParseAsync(responseBody);
		
		responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.FullName);
		responseData.RootElement.GetProperty("tokens").EnumerateArray().ShouldNotBeEmpty();
	}

	[Fact]
	public async Task ErrorFullNameEmpty()
	{
		var request = RequestRegisterUserPersonBuilder.Build();
		request.FullName = string.Empty;
		
		var response = await DoPost(Path, request);
		response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
		
		await using var responseBody = await response.Content.ReadAsStreamAsync();
		var responseData = await JsonDocument.ParseAsync(responseBody);
		var errors =responseData.RootElement.GetProperty("errors").EnumerateArray();
		
		errors.ShouldContain(e => e.GetString()!.Equals(PliromiUserMessagesErrors.FullNameLengthInvalid));
	}
}