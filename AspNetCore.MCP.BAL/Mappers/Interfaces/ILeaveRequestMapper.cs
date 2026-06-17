namespace AspNetCore.MCP.BAL.Mappers.Interfaces;

public interface ILeaveRequestMapper
{
    LeaveRequest LeaveRequestCreateRequestDTOToLeaveRequest(LeaveRequestCreateRequestDTO dto);
    void LeaveRequestUpdateRequestDTOToLeaveRequest(LeaveRequestUpdateRequestDTO dto, LeaveRequest leaveRequest);
}
