namespace AspNetCore.MCP.Domain.DTOs.LeaveRequest;

public record LeaveRequestCreateRequestDTO(
    Guid EmployeeId,
    DateOnly StartDate,
    DateOnly EndDate,
    string LeaveType,
    string Status);
