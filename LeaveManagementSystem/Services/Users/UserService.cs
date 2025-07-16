namespace LeaveManagementSystem.Services.Users;

public class UserService(UserManager<ApplicationUser> _userManager, IHttpContextAccessor _httpContextAccessor) : IUserService
{
    public async Task<ApplicationUser> GetLoggedInUser()
    {
        var http = _httpContextAccessor.HttpContext?.User;
        if (http == null)
            throw new InvalidOperationException("No user context available.");

        var user = await _userManager.GetUserAsync(http);
        if (user == null)
            throw new InvalidOperationException("Authenticated user could not be resolved.");
        return user;
    }

    public async Task<ApplicationUser> GetUserById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException("Authenticated user could not be resolved.");
        return user;
    }

    public async Task<List<ApplicationUser>> GetEmployees()
    {
        var employees = await _userManager.GetUsersInRoleAsync(Roles.Employee);
        return employees.ToList();
    }
}