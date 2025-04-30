using Bogus;
using Pliromi.Domain.Entities;
using Pliromi.Domain.Enums;

namespace CommonTestUtilities.Entities;

public static class PliromiKeyBuilder
{
	public static PliromiKey Build(string userKey, PliromiKeyType pliromiKeyType)
	{
		return new PliromiKey
		{
			Key = userKey,
			Type = pliromiKeyType
		};
	}
	
}