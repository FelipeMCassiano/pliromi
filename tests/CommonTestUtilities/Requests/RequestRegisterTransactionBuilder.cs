using Bogus;
using Communication.Requests;
using Pliromi.Domain.Entities;

namespace CommonTestUtilities.Requests;

public static class RequestRegisterTransactionBuilder
{
	public static RequestRegisterTransaction Build(string pliromiKey)
	{
		return new Faker<RequestRegisterTransaction>()
			.RuleFor(r => r.Value, f => f.Random.Int(1, 10))
			.RuleFor(r => r.PliromiKey, pliromiKey)
			;
	}
	
}