namespace AspNetCore.MCP.BAL.Services;

public class EmployeeService(IUnitOfWork unitOfWork, IEmployeeMapper employeeMapper, IFileLogger logger) : IEmployeeService
{
    #region Fields
    private readonly IEmployeeRepository _employeeRepository = unitOfWork.EmployeeRepository;
    private readonly IDepartmentRepository _departmentRepository = unitOfWork.DepartmentRepository;
    #endregion

    #region Methods
    public async Task<ApiResponse<List<Employee>>> GetAllEmployeesAsync(Guid? id = null, bool isActive = true, int page = 1, int limit = 10)
    {
        logger.Log("Fetching employee records.");

        var result = await _employeeRepository.GetAllAsync(id, isActive, page, limit);
        logger.Log($"Employee fetched successfully with id: {id}");
        return result;
    }

    public async Task<ApiResponse<Employee>> CreateNewEmployeeRecord(EmployeeCreateRequestDTO dto)
    {
        logger.Log("Creating new employee record.");

        var departmentId = Guid.Parse(dto.DepartmentId);

        var departmentExists = await _departmentRepository.ExistsAsync(departmentId);
        if (!departmentExists)
            return ApiResponse<Employee>.Failure("Department not found.");

        var employee = employeeMapper.EmployeeCreateRequestDTOToEmployee(dto);
        employee.DepartmentId = departmentId;

        var created = await _employeeRepository.AddAsync(employee);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Employee created successfully with id: {created.Id}");
        return ApiResponse<Employee>.Success(created, "Employee created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateEmployeeRecord(Guid id, EmployeeUpdateRequestDTO dto)
    {
        logger.Log($"Updating employee: {id}");

        var departmentId = Guid.Parse(dto.DepartmentId);

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
            return ApiResponse<string>.Failure("Employee not found.", [$"No employee found with id: {id}"]);


        var departmentExists = await _departmentRepository.ExistsAsync(departmentId);
        if (!departmentExists)
            return ApiResponse<string>.Failure("Department not found.");

        employeeMapper.EmployeeUpdateRequestDTOToEmployee(dto, employee);
        employee.DepartmentId = departmentId;

        await _employeeRepository.UpdateAsync(employee);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Employee updated successfully with id: {id}");
        return ApiResponse<string>.Success("Employee updated successfully.");
    }

    public async Task<ApiResponse<string>> DeleteEmployeeRecord(Guid id)
    {
        logger.Log($"Deleting employee: {id}");

        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee is null)
            return ApiResponse<string>.Failure("Employee not found.", [$"No employee found with id: {id}"]);

        await _employeeRepository.DeleteAsync(employee);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Employee deleted successfully with id: {id}");
        return ApiResponse<string>.Success("Employee deleted successfully.");
    }

    #endregion
}