namespace AspNetCore.MCP.DAL.Repositories;

public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : BaseEntity
{
    #region Properties
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();
    #endregion

    #region Methods
    public virtual async Task<ApiResponse<List<T>>> GetAllAsync(Guid? id, bool isActive = true, int page = 1, int limit = 10)
    {
        IQueryable<T> query = _dbSet;

        query = query.Where(x => x.IsActive == isActive);

        if (id != null)
            query = query.Where(x => x.Id == id);

        query = query.Skip((page - 1) * limit).Take(limit);

        var data = await query.AsNoTracking().ToListAsync();


        return ApiResponse<List<T>>.Success(data);
    }
    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public virtual Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        entity.IsActive = false;
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        return await _dbSet.AsNoTracking().AnyAsync(e => e.Id == id);
    }
    #endregion
}