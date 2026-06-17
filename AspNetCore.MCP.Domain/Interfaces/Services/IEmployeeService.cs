namespace AspNetCore.MCP.Domain.Interfaces.Services;

public interface IEmployeeService
{
    Task<ApiResponse<List<Employee>>> GetAllEmployeesAsync(Guid? id, bool isActive, int page, int limit);

    Task<ApiResponse<Employee>> CreateNewEmployeeRecord(EmployeeCreateRequestDTO employee);

    Task<ApiResponse<string>> UpdateEmployeeRecord(Guid id, EmployeeUpdateRequestDTO employee);

    Task<ApiResponse<string>> DeleteEmployeeRecord(Guid id);
}
