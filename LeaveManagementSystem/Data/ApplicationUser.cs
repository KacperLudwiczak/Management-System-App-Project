using Microsoft.AspNetCore.Identity;

namespace LeaveManagementSystem.Data;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
