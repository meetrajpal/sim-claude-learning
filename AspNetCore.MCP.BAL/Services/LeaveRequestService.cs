using AspNetCore.MCP.Domain.DTOs.LeaveRequest;

namespace AspNetCore.MCP.BAL.Services;

public class LeaveRequestService(IUnitOfWork unitOfWork, ILeaveRequestMapper leaveRequestMapper, IFileLogger logger) : ILeaveRequestService
{
    #region Fields
    private readonly ILeaveRequestRepository _leaveRequestRepository = unitOfWork.LeaveRequestRepository;
    private readonly IEmployeeRepository _employeeRepository = unitOfWork.EmployeeRepository;
    #endregion

    #region Methods
    public async Task<ApiResponse<List<LeaveRequest>>> GetAllLeaveRequestsAsync(Guid? id = null, bool isActive = true, int page = 1, int limit = 10)
    {
        logger.Log("Fetching leave request records.");

        var result = await _leaveRequestRepository.GetAllAsync(id, isActive, page, limit);
        logger.Log($"Leave request fetched successfully with id: {id}");
        return result;
    }

    public async Task<ApiResponse<LeaveRequest>> CreateNewLeaveRequestRecord(LeaveRequestCreateRequestDTO dto)
    {
        logger.Log("Creating new leave request record.");

        var employeeId = dto.EmployeeId;

        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
            return ApiResponse<LeaveRequest>.Failure("Employee not found.");

        var leaveRequest = leaveRequestMapper.LeaveRequestCreateRequestDTOToLeaveRequest(dto);

        var created = await _leaveRequestRepository.AddAsync(leaveRequest);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Leave request created successfully with id: {created.Id}");
        return ApiResponse<LeaveRequest>.Success(created, "Leave request created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateLeaveRequestRecord(Guid id, LeaveRequestUpdateRequestDTO dto)
    {
        logger.Log($"Updating leave request: {id}");

        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
        if (leaveRequest is null)
            return ApiResponse<string>.Failure("Leave request not found.", [$"No leave request found with id: {id}"]);

        var employeeId = dto.EmployeeId;
        var employeeExists = await _employeeRepository.ExistsAsync(employeeId);
        if (!employeeExists)
            return ApiResponse<string>.Failure("Employee not found.");

        leaveRequestMapper.LeaveRequestUpdateRequestDTOToLeaveRequest(dto, leaveRequest);

        await _leaveRequestRepository.UpdateAsync(leaveRequest);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Leave request updated successfully with id: {id}");
        return ApiResponse<string>.Success("Leave request updated successfully.");
    }

    public async Task<ApiResponse<string>> DeleteLeaveRequestRecord(Guid id)
    {
        logger.Log($"Deleting leave request: {id}");

        var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
        if (leaveRequest is null)
            return ApiResponse<string>.Failure("Leave request not found.", [$"No leave request found with id: {id}"]);

        await _leaveRequestRepository.DeleteAsync(leaveRequest);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Leave request deleted successfully with id: {id}");
        return ApiResponse<string>.Success("Leave request deleted successfully.");
    }

    #endregion
}
