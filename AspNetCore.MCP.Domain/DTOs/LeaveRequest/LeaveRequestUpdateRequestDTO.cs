namespace AspNetCore.MCP.Domain.DTOs.LeaveRequest;

public record LeaveRequestUpdateRequestDTO(
    Guid EmployeeId,
    DateOnly StartDate,
    DateOnly EndDate,
    string LeaveType,
    string Status,
    bool IsActive);
