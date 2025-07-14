using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Roles
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "role-emp", Name = "Employee", NormalizedName = "EMPLOYEE" },
            new IdentityRole { Id = "role-sup", Name = "Supervisor", NormalizedName = "SUPERVISOR" },
            new IdentityRole { Id = "role-admin", Name = "Administrator", NormalizedName = "ADMINISTRATOR" }
        );

        // Seed Admin User
        var hasher = new PasswordHasher<ApplicationUser>();
        var adminUser = new ApplicationUser
        {
            Id = "admin-user-id",
            UserName = "admin@localhost.com",
            NormalizedUserName = "ADMIN@LOCALHOST.COM",
            Email = "admin@localhost.com",
            NormalizedEmail = "ADMIN@LOCALHOST.COM",
            EmailConfirmed = true,
            FirstName = "Default",
            LastName = "Admin",
            DateOfBirth = new DateOnly(1950, 12, 1),
            PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
        };

        builder.Entity<ApplicationUser>().HasData(adminUser);

        // Assign Role to Admin
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = "role-admin"
            });
    }

    public DbSet<LeaveType> LeaveTypes { get; set; }
}