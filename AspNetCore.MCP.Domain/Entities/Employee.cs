namespace AspNetCore.MCP.Domain.Entities;

public class Employee : BaseEntity, IAuditEnitity
{
    public string EmployeeName { get; set; } = string.Empty!;
    public decimal Salary { get; set; }
    public string EmailId { get; set; } = string.Empty!;
    public DateOnly JoiningDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;

    public string Status { get; set; } = string.Empty!;
}
