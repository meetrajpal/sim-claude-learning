namespace AspNetCore.MCP.BAL.Services;

public class DepartmentService(IUnitOfWork unitOfWork, IDepartmentMapper departmentMapper, IFileLogger logger) : IDepartmentService
{
    #region Fields
    private readonly IDepartmentRepository _departmentRepository = unitOfWork.DepartmentRepository;
    #endregion

    #region Methods
    public async Task<ApiResponse<List<Department>>> GetAllDepartmentsAsync(Guid? id = null, bool isActive = true, int page = 1, int limit = 10)
    {
        logger.Log("Fetching department records.");

        var result = await _departmentRepository.GetAllAsync(id, isActive, page, limit);
        logger.Log($"Department fetched successfully.");
        return result;
    }

    public async Task<ApiResponse<Department>> CreateNewDepartmentRecord(DepartmentCreateRequestDTO dto)
    {
        logger.Log("Creating new department.");

        var exists = await _departmentRepository.GetByNameAsync(dto.DepartmentName);
        if (exists)
        {
            logger.LogError($"Department already exists: {dto.DepartmentName}", null);
            return ApiResponse<Department>.Failure("Record with same department name already exists.");
        }

        var department = departmentMapper.DepartmentCreateRequestDTOToDepartment(dto);
        var created = await _departmentRepository.AddAsync(department);

        await unitOfWork.SaveChangesAsync();

        logger.Log($"Department created with id: {created.Id}");
        return ApiResponse<Department>.Success(created, "Department created successfully.");
    }

    public async Task<ApiResponse<string>> UpdateDepartmentRecord(Guid id, DepartmentUpdateRequestDTO dto)
    {
        logger.Log($"Updating department: {id}");

        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null)
            return ApiResponse<string>.Failure("Department not found.", [$"No department found with id: {id}"]);

        departmentMapper.DepartmentUpdateRequestDTOToDepartment(dto, department);

        await _departmentRepository.UpdateAsync(department);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Department updated: {id}");
        return ApiResponse<string>.Success("Department updated successfully.");
    }

    public async Task<ApiResponse<string>> DeleteDepartmentRecord(Guid id)
    {
        logger.Log($"Deleting department: {id}");

        var department = await _departmentRepository.GetByIdAsync(id);
        if (department is null)
            return ApiResponse<string>.Failure("Department not found.", [$"No department found with id: {id}"]);

        await _departmentRepository.DeleteAsync(department);
        await unitOfWork.SaveChangesAsync();

        logger.Log($"Department deleted: {id}");
        return ApiResponse<string>.Success("Department deleted successfully.");
    }
    #endregion
}