namespace AspNetCore.MCP.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository EmployeeRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    ILeaveRequestRepository LeaveRequestRepository { get; }
    Task<int> SaveChangesAsync();
}
