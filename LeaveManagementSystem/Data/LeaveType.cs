using System.ComponentModel.DataAnnotations;

namespace LeaveManagementSystem.Data;

public class LeaveType
{
    public int Id { get; set; }
    [MaxLength(150)]
    public required string Name { get; set; }
    public int NumberOfDays { get; set; }
}
