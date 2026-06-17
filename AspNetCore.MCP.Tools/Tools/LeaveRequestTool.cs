namespace AspNetCore.MCP.Tools.Tools;

[McpServerToolType]
public class LeaveRequestTool(
    ILeaveRequestService _leaveRequestService,
    IValidator<LeaveRequestCreateRequestDTO> _createValidator,
    IValidator<LeaveRequestUpdateRequestDTO> _updateValidator)
{
    [McpServerTool, Description("Get all leave requests. Optionally filter by leave request ID, active status, with pagination support.")]
    public async Task<string> GetAllLeaveRequestsAsync(
        [Description("Optional leave request GUID to filter by specific leave request")] string? id = null,
        [Description("Filter by active status. Default is true")] bool isActive = true,
        [Description("Page number for pagination. Default is 1")] int page = 1,
        [Description("Number of records per page. Default is 10")] int limit = 10)
    {
        Guid? leaveRequestId = null;

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var parsedId))
                throw new ArgumentException($"Invalid Guid format for given id: {id}");

            leaveRequestId = parsedId;
        }

        var result = await _leaveRequestService.GetAllLeaveRequestsAsync(leaveRequestId, isActive, page, limit);
        return System.Text.Json.JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Create a new leave request record.")]
    public async Task<string> CreateLeaveRequest(
        [Description("Employee GUID the leave request belongs to")] string employeeId,
        [Description("Start date of the leave in yyyy-MM-dd format")] string startDate,
        [Description("End date of the leave in yyyy-MM-dd format")] string endDate,
        [Description("Type of leave e.g. Vacation, Sick, etc.")] string leaveType,
        [Description("Status of the leave request e.g. Pending, Approved, Rejected")] string status)
    {
        if (!Guid.TryParse(employeeId, out var parsedEmployeeId))
            throw new ArgumentException($"Invalid Guid format for given employeeId: {employeeId}");

        var dto = new LeaveRequestCreateRequestDTO(
            EmployeeId: parsedEmployeeId,
            StartDate: DateOnly.Parse(startDate),
            EndDate: DateOnly.Parse(endDate),
            LeaveType: leaveType,
            Status: status
        );

        var validationRes = await _createValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new FluentValidation.ValidationException(validationRes.Errors);

        var result = await _leaveRequestService.CreateNewLeaveRequestRecord(dto);
        return System.Text.Json.JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Update an existing leave request record by ID.")]
    public async Task<string> UpdateLeaveRequest(
        [Description("Leave request GUID to update")] string id,
        [Description("Employee GUID the leave request belongs to")] string employeeId,
        [Description("Updated start date of the leave in yyyy-MM-dd format")] string startDate,
        [Description("Updated end date of the leave in yyyy-MM-dd format")] string endDate,
        [Description("Updated type of leave e.g. Vacation, Sick, etc.")] string leaveType,
        [Description("Updated status of the leave request e.g. Pending, Approved, Rejected")] string status,
        [Description("Updated active status")] bool isActive)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        if (!Guid.TryParse(employeeId, out var parsedEmployeeId))
            throw new ArgumentException($"Invalid Guid format for given employeeId: {employeeId}");

        var dto = new LeaveRequestUpdateRequestDTO(
            EmployeeId: parsedEmployeeId,
            StartDate: DateOnly.Parse(startDate),
            EndDate: DateOnly.Parse(endDate),
            LeaveType: leaveType,
            Status: status,
            IsActive: isActive
        );

        var validationRes = await _updateValidator.ValidateAsync(dto);
        if (!validationRes.IsValid)
            throw new FluentValidation.ValidationException(validationRes.Errors);

        var result = await _leaveRequestService.UpdateLeaveRequestRecord(parsedId, dto);
        return System.Text.Json.JsonSerializer.Serialize(result);
    }

    [McpServerTool, Description("Delete a leave request record by ID.")]
    public async Task<string> DeleteLeaveRequest(
        [Description("Leave request GUID to delete")] string id)
    {
        if (!Guid.TryParse(id, out var parsedId))
            throw new ArgumentException($"Invalid Guid format for given id: {id}");

        var result = await _leaveRequestService.DeleteLeaveRequestRecord(parsedId);
        return System.Text.Json.JsonSerializer.Serialize(result);
    }
}
