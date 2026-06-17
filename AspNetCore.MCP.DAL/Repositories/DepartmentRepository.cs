namespace AspNetCore.MCP.DAL.Repositories;

public class DepartmentRepository(ApplicationDbContext dbContext) : BaseRepository<Department>(dbContext), IDepartmentRepository
{
    #region Methods
    public async Task<bool> GetByNameAsync(string name)
    {
        return await _dbSet.AnyAsync(x => x.DepartmentName == name);
    }
    #endregion
}
