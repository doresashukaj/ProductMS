using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductMS.Entities;
using ProductMS.SeedConfiguration;


namespace ProductMS;

public class DatabaseContext : IdentityDbContext<User, Role, string>

{
    public DatabaseContext(DbContextOptions options)
        : base(options)
    { 

    }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Property(u => u.Gender)
            .HasConversion<string>();

        builder.Entity<Product>()
        .Property(p => p.Price)
        .HasColumnType("decimal(18,2)");

        builder.ApplyConfiguration(new RoleConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());

    }

}
