namespace AspNetCore.MCP.DAL.UnitOfWork;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    #region Fields
    private readonly ApplicationDbContext _context = context;

    private readonly Lazy<IEmployeeRepository> _employeeRepository = new(() => new EmployeeRepository(context));

    private readonly Lazy<IDepartmentRepository> _departmentRepository = new(() => new DepartmentRepository(context));
    #endregion

    #region Properties
    public IEmployeeRepository EmployeeRepository => _employeeRepository.Value;
    public IDepartmentRepository DepartmentRepository => _departmentRepository.Value;
    #endregion

    #region Methods
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
    #endregion
}
