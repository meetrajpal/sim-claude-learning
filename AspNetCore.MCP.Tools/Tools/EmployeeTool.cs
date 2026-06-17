namespace AspNetCore.MCP.API.Tools;

[McpServerToolType]
public class EmployeeTool(
    IEmployeeService _employeeService,
    IValidator<EmployeeCreateRequestDTO> _createValidator,
    IValidator<EmployeeUpdateRequestDTO> _updateValidator)
{
    [McpServerTool, Description("Get all employees. Optionally filter by employee ID, active status, with pagination support.")]
    public async Task<string> GetAllEmployeesAsync(
        [Description("Optional employee GUID to filter by specific employee")] string? id = null,
        [Description("Filter by active status. Default is true")] bool isActive = true,
        [Description("Page number for pagination. Default is 1")] int page = 1,
        [Description("Number of records per page. Default is 10")] int limit = 10)
    {
        Guid? employeeId = null;

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var parsedId))
                throw new ArgumentException($"Invalid Guid format for given id: {id}");

            employeeId = parsedId;
        }

        var result = await _employeeService.GetAllEmployeesAsync(employeeId, isActive, page, limit);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Create a new employee record.")]
    public async Task<string> CreateEmployee(
        [Description("Full name of the employee")] string employeeName,
        [Description("Salary of the employee")] decimal salary,
        [Description("Email address of the employee")] string emailId,
        [Description("Joining date of the employee in yyyy-MM-dd format")] string joiningDate,
        [Description("Department GUID the employee belongs to")] string departmentId,
        [Description("Status of the employee e.g. Registered, Onboarded, etc.")] string status)
    {
        var dto = new EmployeeCreateRequestDTO(
            EmployeeName: employeeName,
            Salary: salary,
            EmailId: emailId,
            JoiningDate: DateOnly.Parse(joiningDate),
            DepartmentId: departmentId,
            Status: status
        );

        var validationRes = await _createValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _employeeService.CreateNewEmployeeRecord(dto);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Update an existing employee record by ID.")]
    public async Task<string> UpdateEmployee(
        [Description("Employee GUID to update")] string id,
        [Description("Updated full name of the employee")] string employeeName,
        [Description("Updated salary of the employee")] decimal salary,
        [Description("Updated email address of the employee")] string emailId,
        [Description("Updated joining date of the employee in yyyy-MM-dd format")] string joiningDate,
        [Description("Updated department GUID the employee belongs to")] string departmentId,
        [Description("Status of the employee e.g. Registered, Onboarded, etc.")] string status)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        var dto = new EmployeeUpdateRequestDTO(
            EmployeeName: employeeName,
            Salary: salary,
            EmailId: emailId,
            JoiningDate: DateOnly.Parse(joiningDate),
            DepartmentId: departmentId,
            Status: status,
            IsActive: true
        );

        var validationRes = await _updateValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new ValidationException(validationRes.Errors);

        var result = await _employeeService.UpdateEmployeeRecord(parsedId, dto);
        return JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Delete an employee record by ID.")]
    public async Task<string> DeleteEmployee(
        [Description("Employee GUID to delete")] string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        var result = await _employeeService.DeleteEmployeeRecord(parsedId);
        return JsonSerializer.Serialize(result);
    }
}