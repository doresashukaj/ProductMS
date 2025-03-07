using ProductMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductMS.SeedConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(

            new Role
            {
                Id = "7afdab2c-004e-49cd-8fc3-b8f323dfcb9d",
                Name = "User",
                NormalizedName = "USER",
                Description = "The visitor role for the user"
            },
            new Role
            {
                Id= "050c25ee-e873-468e-8983-63eaa8325de4",
                Name = "Admin",
                NormalizedName = "ADMIN",
                Description = "The admin role for the user"
            }
            );
    }
}
