using System.Net;
using System.Text.Json;
using Amazon.Runtime.Internal.Util;
using CommonTestUtilities.AccessToken;
using CommonTestUtilities.Requests;
using Shouldly;
using Xunit.Abstractions;

namespace WebApi.Test.Transaction.Register;

public class RegisterTransactionTest : PliromiClassFixture
{
	private const string Path = "/transaction";
	private readonly Guid _userIdentifier;
	private readonly string _receiverPliromiKey;
	public RegisterTransactionTest(CustomWebApplicationFactory factory) : base(factory)
	{
		_userIdentifier = factory.GetUserPersonIdentifier();
		_receiverPliromiKey = factory.GetStorePliromiKey();
	}

	[Fact]
	public async Task Success()
	{
		var request = RequestRegisterTransactionBuilder.Build(_receiverPliromiKey);
		var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
		var response = await DoPost(Path, request, token);
		response.EnsureSuccessStatusCode();
	}

	[Fact]
	public async Task ErrorNegativeValue()
	{
		var request = RequestRegisterTransactionBuilder.Build(_receiverPliromiKey);
		request.Value = -1;
		var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
		var response = await DoPost(Path, request, token);
		response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
	}

	
	[Fact]
	public async Task ErrorWithoutToken()
	{
		var request = RequestRegisterTransactionBuilder.Build(_receiverPliromiKey);
		var response = await DoPost(Path, request, string.Empty);
		response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
	}

	[Fact]
	public async Task ErrorTokenWithUserNotFound()
	{
		var request = RequestRegisterTransactionBuilder.Build(_receiverPliromiKey);
		var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
		var response = await DoPost(Path, request, token);
		response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
	}
}