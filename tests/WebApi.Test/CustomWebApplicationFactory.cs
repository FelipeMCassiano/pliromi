using CommonTestUtilities.EmailSender;
using CommonTestUtilities.Entities;
using CommonTestUtilities.ServiceBus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pliromi.Domain.Services.EmailService;
using Pliromi.Domain.Services.ServiceBus;
using Pliromi.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	private Pliromi.Domain.Entities.User _userPerson = null!;
	private string _passwordPerson = string.Empty;
	private string _passwordStore = string.Empty;
	public Pliromi.Domain.Entities.User _userStore = null!;
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Test")
		       .ConfigureServices(services =>
		       {
			       var descriptor =
				       services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PliromiDbContext>));

			       if (descriptor is not null) services.Remove(descriptor);

			       var queue = SendEmailQueueBuilder.Build();
			       services.AddScoped(_ => queue);
			       services.AddScoped(_ => EmailSenderBuilder.Build());
			       var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
			       services.AddDbContext<PliromiDbContext>(opt =>
			       {
				       opt.UseInMemoryDatabase("InMemoryDbForTesting");
				       opt.UseInternalServiceProvider(provider);
			       });
			       using var scope = services.BuildServiceProvider().CreateScope();
			       var dbContext = scope.ServiceProvider.GetRequiredService<PliromiDbContext>();
			       StartDatabase(dbContext);
		       });
		builder.ConfigureLogging(l =>
		{
			l.ClearProviders();
			l.AddConsole();
		});
	}
	public Guid GetUserPersonIdentifier() => _userPerson.UserIdentifier;
	public string GetUserPersonPassword() => _passwordPerson;
	public string GetUserPersonEmail() => _userPerson.Email;
	public string GetStorePliromiKey () => _userStore.PliromiKey.Key;
	
	public Pliromi.Domain.Entities.User GetUserStore() => _userStore;

	private void StartDatabase(PliromiDbContext dbContext)
	{
		 (_userPerson, _passwordPerson) = UserBuilder.BuildPerson();
		 (_userStore, _passwordStore) = UserBuilder.BuildStore();
		 
		dbContext.Users.Add(_userPerson);
		dbContext.Add(_userStore);
		dbContext.SaveChanges();
	}
	
}