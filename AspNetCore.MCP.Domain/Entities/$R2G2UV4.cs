namespace AspNetCore.MCP.Domain.Entities;

public class LeaveRequest : BaseEntity, IAuditEnitity
{
    public Guid EmployeeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string LeaveType { get; set; } = string.Empty!;

    public string Status { get; set; } = string.Empty!;

    public Employee Employee { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}