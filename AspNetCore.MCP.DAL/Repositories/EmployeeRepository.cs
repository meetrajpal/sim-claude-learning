namespace AspNetCore.MCP.DAL.Repositories;

public class EmployeeRepository(ApplicationDbContext dbContext) : BaseRepository<Employee>(dbContext), IEmployeeRepository
{
    #region Methods
    public override async Task<ApiResponse<List<Employee>>> GetAllAsync(Guid? id, bool isActive = true, int page = 1, int limit = 10)
    {
        IQueryable<Employee> query = _dbSet;

        query = query.Where(x => x.IsActive == isActive);

        if (id != null)
            query = query.Where(x => x.Id == id);

        query = query.Skip((page - 1) * limit).Take(limit);

        var data = await query.Include("Department").ToListAsync();


        return ApiResponse<List<Employee>>.Success(data);
    }

    public override async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include("Department").FirstOrDefaultAsync(x => x.Id == id);
    }
    #endregion
}
