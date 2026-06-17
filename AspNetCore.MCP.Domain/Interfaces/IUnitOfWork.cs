namespace AspNetCore.MCP.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository EmployeeRepository { get; }
    IDepartmentRepository DepartmentRepository { get; }
    Task<int> SaveChangesAsync();
}
