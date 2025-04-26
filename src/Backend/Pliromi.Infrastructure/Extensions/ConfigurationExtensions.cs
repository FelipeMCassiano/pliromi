using Microsoft.Extensions.Configuration;

namespace Pliromi.Infrastructure.Extensions;

public static class ConfigurationExtensions
{
	public static bool IsUnitTestEnvironment(this IConfiguration configuration) => configuration.GetValue<bool>("IsUnitTestEnvironment");

	public static string ConnectionString(this IConfiguration configuration) =>
		configuration.GetConnectionString("Connection")!;

}