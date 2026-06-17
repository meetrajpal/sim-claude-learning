namespace AspNetCore.MCP.Domain.DTOs.Employee;

public record EmployeeUpdateRequestDTO(string EmployeeName, decimal Salary, string EmailId, DateOnly JoiningDate, string DepartmentId, string Status, bool IsActive);