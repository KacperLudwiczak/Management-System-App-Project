
using System.ComponentModel;
using LeaveManagementSystem.Models.LeaveAllocations;

namespace LeaveManagementSystem.Models.LeaveRequests;

public class ReviewLeaveRequestVM : LeaveRequestReadOnlyVM
{
    public EmployeeListVM Employee { get; set; } = new EmployeeListVM();

    [DisplayName("Additional Information")]
    public string? RequestComments { get; set; }
}