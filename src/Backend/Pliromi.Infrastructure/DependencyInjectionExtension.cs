using Amazon;
using Amazon.SimpleEmail;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pliromi.Domain.Repositories;
using Pliromi.Domain.Security.Cryptography;
using Pliromi.Domain.Security.Tokens;
using Pliromi.Domain.Services.EmailService;
using Pliromi.Domain.Services.LoggedUser;
using Pliromi.Domain.Services.ServiceBus;
using Pliromi.Infrastructure.DataAccess;
using Pliromi.Infrastructure.DataAccess.Repositories;
using Pliromi.Infrastructure.Extensions;
using Pliromi.Infrastructure.Security.Cryptography;
using Pliromi.Infrastructure.Security.Token.Access;
using Pliromi.Infrastructure.Security.Token.Access.Generator;
using Pliromi.Infrastructure.Security.Token.Access.Validator;
using Pliromi.Infrastructure.Services.EmailService;
using Pliromi.Infrastructure.Services.LoggedUser;
using Pliromi.Infrastructure.Services.ServiceBus;

namespace Pliromi.Infrastructure;

public static class DependencyInjectionExtension
{
	public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		AddRepositories(services);	
		AddSecurity(services, configuration);
		AddLoggedUser(services);
		AddQueueService(services);

		if (configuration.IsUnitTestEnvironment())
		{
			return;
		}
		AddDbContext(services, configuration);
		AddEmailSenderService(services, configuration);
	}

	private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString("Connection");
		services.AddDbContext<PliromiDbContext>(options =>
		{
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("Pliromi.API"));
		});
	}

	private static void AddRepositories(IServiceCollection services )
	{
		services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddScoped<IUserReadOnlyRepository, UserRepository>();
		services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
		services.AddScoped<ITransactionWriteOnlyRepository, TransactionRepository>();
		services.AddScoped<ITransactionReadOnlyRepository, TransactionRepository>();
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

	private static void AddQueueService(IServiceCollection services)
	{
		services.AddMassTransit(x =>
		{
			x.AddConsumer<SendEmailConsumer>();
			x.UsingRabbitMq((context, cfg) =>
			{
				cfg.Host("localhost", "/", h =>
				{
					h.Username("guest");
					h.Password("guest");
				});
				cfg.ConfigureEndpoints(context);
			});
		});
		services.AddScoped<ISendEmailQueue, SendEmailQueue>();
	}
	private static void AddEmailSenderService(this IServiceCollection services, IConfiguration configuration)
	{
		var accessKey = configuration.GetValue<string>("Settings:SES:AccessKey");
		var secretKey = configuration.GetValue<string>("Settings:SES:SecretKey");
		var emailSource = configuration.GetValue<string>("Settings:SES:EmailSource");
		
		var regionEndpoint = RegionEndpoint.USEast1;
		
		var client = new AmazonSimpleEmailServiceClient(accessKey, secretKey, region: regionEndpoint);
		
		services.AddScoped<IEmailSender >(_ => new AmazonSimpleEmailSender(client,emailSource! ));
	}
	
}