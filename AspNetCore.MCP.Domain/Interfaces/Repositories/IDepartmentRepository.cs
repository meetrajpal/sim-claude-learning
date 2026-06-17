namespace AspNetCore.MCP.Domain.Interfaces.Repositories;

public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<bool> GetByNameAsync(string name);
}
