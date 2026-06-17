using AspNetCore.MCP.Domain.DTOs.LeaveRequest;

namespace AspNetCore.MCP.BAL.Mappers;

[Mapper]
public partial class LeaveRequestMapper : ILeaveRequestMapper
{
    [MapperIgnoreTarget(nameof(LeaveRequest.Id))]
    [MapperIgnoreTarget(nameof(LeaveRequest.CreatedAt))]
    [MapperIgnoreTarget(nameof(LeaveRequest.UpdatedAt))]
    public partial LeaveRequest LeaveRequestCreateRequestDTOToLeaveRequest(LeaveRequestCreateRequestDTO dto);

    [MapperIgnoreTarget(nameof(LeaveRequest.Id))]
    [MapperIgnoreTarget(nameof(LeaveRequest.CreatedAt))]
    [MapperIgnoreTarget(nameof(LeaveRequest.UpdatedAt))]
    public partial void LeaveRequestUpdateRequestDTOToLeaveRequest(LeaveRequestUpdateRequestDTO dto, LeaveRequest leaveRequest);
}
