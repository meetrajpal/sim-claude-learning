namespace AspNetCore.MCP.Domain.Entities;

public class Department : BaseEntity, IAuditEnitity
{
    public string DepartmentName { get; set; } = string.Empty!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    [JsonIgnore]
    public ICollection<Employee> Employees { get; set; } = [];
}
