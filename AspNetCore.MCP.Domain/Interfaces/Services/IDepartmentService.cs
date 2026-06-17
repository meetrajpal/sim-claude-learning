namespace AspNetCore.MCP.Domain.Interfaces.Services;

public interface IDepartmentService
{
    Task<ApiResponse<List<Department>>> GetAllDepartmentsAsync(Guid? id, bool isActive, int page, int limit);

    Task<ApiResponse<Department>> CreateNewDepartmentRecord(DepartmentCreateRequestDTO department);

    Task<ApiResponse<string>> UpdateDepartmentRecord(Guid id, DepartmentUpdateRequestDTO department);

    Task<ApiResponse<string>> DeleteDepartmentRecord(Guid id);

}
