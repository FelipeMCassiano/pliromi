using Microsoft.EntityFrameworkCore;
using Pliromi.Domain.Entities;

namespace Pliromi.Infrastructure.DataAccess;

public class PliromiDbContext : DbContext
{
	public PliromiDbContext( DbContextOptions<PliromiDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id);
			entity.HasIndex(e => e.Email).IsUnique();
			entity.HasIndex(e => e.Cpf).IsUnique();
			entity.HasIndex(e => e.Cnpj).IsUnique();
			entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
			entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
			entity.Property(u => u.Cpf)
			.HasMaxLength(11);
		entity.Property(u => u.Cnpj)
			.HasMaxLength(14);
		entity.Property(u => u.IsStore).HasDefaultValue(false);
		entity.Property(u => u.Balance).IsRequired();

		});
		
		modelBuilder.Entity<Transaction>()
		            .HasOne(tr => tr.Sender)
		            .WithMany(u => u.SentTransactions)
		            .HasForeignKey(tr => tr.SenderId)
		            .OnDelete(DeleteBehavior.Restrict);
	}
	public DbSet<User> Users { get; set; }
	public DbSet<Transaction> Transactions { get; set; }
	public DbSet<PliromiKey> PliromiKeys { get; set; }
}