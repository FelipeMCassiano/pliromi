using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pliromi.Infrastructure.DataAccess;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("Test")
		       .ConfigureServices(services =>
		       {
			       var descriptor =
				       services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<PliromiDbContext>));

			       if (descriptor is not null) services.Remove(descriptor);
			       
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
	}

	private void StartDatabase(PliromiDbContext dbContext)
	{
		dbContext.SaveChanges();
	}
	
}