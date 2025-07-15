namespace LeaveManagementSystem.Data;

public class LeaveType : BaseEntity
{
    [MaxLength(150)]
    public required string Name { get; set; }
    public int NumberOfDays { get; set; }
    public List<LeaveAllocation>? LeaveAllocations { get; set; }
}
