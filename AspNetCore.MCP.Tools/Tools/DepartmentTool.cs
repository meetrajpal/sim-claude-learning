namespace AspNetCore.MCP.API.Tools;

[McpServerToolType]
public class DepartmentTool(
    IDepartmentService _departmentService,
    IValidator<DepartmentCreateRequestDTO> _createValidator,
    IValidator<DepartmentUpdateRequestDTO> _updateValidator)
{
    [McpServerTool, Description("Get all departments. Optionally filter by department ID, active status, with pagination support.")]
    public async Task<string> GetAllDepartmentsAsync(
        [Description("Optional department GUID to filter by specific department")] string? id = null,
        [Description("Filter by active status. Default is true")] bool isActive = true,
        [Description("Page number for pagination. Default is 1")] int page = 1,
        [Description("Number of records per page. Default is 10")] int limit = 10)
    {
        Guid? departmentId = null;

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var parsedId))
                throw new ArgumentException($"Invalid Guid format for given id: {id}");

            departmentId = parsedId;
        }

        var result = await _departmentService.GetAllDepartmentsAsync(departmentId, isActive, page, limit);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Create a new department record.")]
    public async Task<string> CreateDepartment(
        [Description("Department name")] string name)
    {
        var dto = new DepartmentCreateRequestDTO(name);

        var validationRes = await _createValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _departmentService.CreateNewDepartmentRecord(dto);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Update an existing department record by ID.")]
    public async Task<string> UpdateDepartment(
        [Description("Department GUID to update")] string id,
        [Description("Updated department name")] string name)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        var dto = new DepartmentUpdateRequestDTO(name, true);

        var validationRes = await _updateValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _departmentService.UpdateDepartmentRecord(parsedId, dto);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Delete a department record by ID.")]
    public async Task<string> DeleteDepartment(
        [Description("Department GUID to delete")] string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        var result = await _departmentService.DeleteDepartmentRecord(parsedId);
        return JsonSerializer.Serialize(result);
    }
}