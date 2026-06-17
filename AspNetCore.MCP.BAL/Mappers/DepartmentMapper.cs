namespace AspNetCore.MCP.BAL.Mappers;

[Mapper]
public partial class DepartmentMapper : IDepartmentMapper
{
    [MapperIgnoreTarget(nameof(Department.Id))]
    [MapperIgnoreTarget(nameof(Department.CreatedAt))]
    [MapperIgnoreTarget(nameof(Department.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Department.IsActive))]
    [MapperIgnoreTarget(nameof(Department.Employees))]
    public partial Department DepartmentCreateRequestDTOToDepartment(DepartmentCreateRequestDTO dto);

    [MapperIgnoreTarget(nameof(Department.Id))]
    [MapperIgnoreTarget(nameof(Department.CreatedAt))]
    [MapperIgnoreTarget(nameof(Department.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Department.Employees))]
    public partial void DepartmentUpdateRequestDTOToDepartment(DepartmentUpdateRequestDTO dto, Department department);
}
