namespace AspNetCore.MCP.DAL.Repositories;

public class LeaveRequestRepository(ApplicationDbContext dbContext) : BaseRepository<LeaveRequest>(dbContext), ILeaveRequestRepository
{
    #region Methods
    public override async Task<ApiResponse<List<LeaveRequest>>> GetAllAsync(Guid? id, bool isActive = true, int page = 1, int limit = 10)
    {
        IQueryable<LeaveRequest> query = _dbSet;

        query = query.Where(x => x.IsActive == isActive);

        if (id != null)
            query = query.Where(x => x.Id == id);

        query = query.Skip((page - 1) * limit).Take(limit);

        var data = await query.Include("Employee").ToListAsync();

        return ApiResponse<List<LeaveRequest>>.Success(data);
    }

    public override async Task<LeaveRequest?> GetByIdAsync(Guid id)
    {
        return await _dbSet.Include("Employee").FirstOrDefaultAsync(x => x.Id == id);
    }
    #endregion
}
