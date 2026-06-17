using AspNetCore.MCP.Domain.DTOs.LeaveRequest;

namespace AspNetCore.MCP.Domain.Interfaces.Services;

public interface ILeaveRequestService
{
    Task<ApiResponse<List<LeaveRequest>>> GetAllLeaveRequestsAsync(Guid? id, bool isActive, int page, int limit);

    Task<ApiResponse<LeaveRequest>> CreateNewLeaveRequestRecord(LeaveRequestCreateRequestDTO leaveRequest);

    Task<ApiResponse<string>> UpdateLeaveRequestRecord(Guid id, LeaveRequestUpdateRequestDTO leaveRequest);

    Task<ApiResponse<string>> DeleteLeaveRequestRecord(Guid id);
}