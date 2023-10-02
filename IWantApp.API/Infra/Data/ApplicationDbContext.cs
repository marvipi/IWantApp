using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IWantApp.API.Infra.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Product> Products { get; init; }
    public DbSet<Category> Categories { get; init; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<Notification>();

        modelBuilder.Entity<Product>()
            .Property(product => product.Price)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Product>()
            .Property(product => product.Description)
            .HasMaxLength(250);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .HaveMaxLength(100);
    }
}
