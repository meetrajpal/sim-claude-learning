namespace AspNetCore.MCP.Domain.DTOs.Employee;

public record EmployeeCreateRequestDTO(string EmployeeName, decimal Salary, string EmailId, DateOnly JoiningDate, string DepartmentId, string Status);
