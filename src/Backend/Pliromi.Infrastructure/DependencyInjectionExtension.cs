using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Security.Cryptography;
using Pliromi.Domain.Security.Tokens;
using Pliromi.Domain.Services.LoggedUser;
using Pliromi.Infrastructure.DataAccess;
using Pliromi.Infrastructure.DataAccess.Repositories;
using Pliromi.Infrastructure.Security.Cryptography;
using Pliromi.Infrastructure.Security.Token.Access;
using Pliromi.Infrastructure.Security.Token.Access.Generator;
using Pliromi.Infrastructure.Security.Token.Access.Validator;
using Pliromi.Infrastructure.Services.LoggedUser;

namespace Pliromi.Infrastructure;

public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddDbContext(services, configuration);
		AddRepositories(services);	
		AddSecurity(services, configuration);
		AddLoggedUser(services);
	}

	private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Connection");
		services.AddDbContext<PliromiDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		});
	}

	private static void AddRepositories(IServiceCollection services )
	{
		services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserReadOnlyRepository, UserRepository>();
		services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
		services.AddScoped<ITransactionWriteOnlyRepository, TransactionRepository>();
	}

	private static void AddSecurity(IServiceCollection services, IConfiguration configuration)
	{
		var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
		
		services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey!));
		services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(signingKey!));

		services.AddScoped<IPasswordEncrypter, BcryptNet>();
	}

	private  static void AddLoggedUser(IServiceCollection services)
	{
		services.AddScoped<ILoggedUser, LoggedUser>();

	}
	
}