using System.Net;
using CommonTestUtilities.AccessToken;
using CommonTestUtilities.Requests;
using Shouldly;

namespace WebApi.Test.Dashboard;

public class DashboardTest : PliromiClassFixture
{
	private const string Path = "/dashboard";
	private Guid _userIdentifier;
	public DashboardTest(CustomWebApplicationFactory factory) : base(factory)
	{
		_userIdentifier = factory.GetUserPersonIdentifier();
	}

	[Fact]
	public async Task Success()
	{
		var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
		var response = await DoGet(Path, token);
		response.EnsureSuccessStatusCode();
	}
	[Fact]
	public async Task ErrorWithoutToken()
	{
		var response = await DoGet(Path, string.Empty);
		response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task ErrorTokenWithUserNotFound()
	{
		var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
		var response = await DoGet(Path, token);
		response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
	}
}