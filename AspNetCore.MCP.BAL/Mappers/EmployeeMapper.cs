namespace AspNetCore.MCP.BAL.Mappers;

[Mapper]
public partial class EmployeeMapper : IEmployeeMapper
{
    [MapperIgnoreTarget(nameof(Employee.Id))]
    [MapperIgnoreTarget(nameof(Employee.CreatedAt))]
    [MapperIgnoreTarget(nameof(Employee.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Employee.IsActive))]
    [MapperIgnoreTarget(nameof(Employee.Department))]
    public partial Employee EmployeeCreateRequestDTOToEmployee(EmployeeCreateRequestDTO dto);

    [MapperIgnoreTarget(nameof(Employee.Id))]
    [MapperIgnoreTarget(nameof(Employee.CreatedAt))]
    [MapperIgnoreTarget(nameof(Employee.UpdatedAt))]
    [MapperIgnoreTarget(nameof(Employee.Department))]
    public partial void EmployeeUpdateRequestDTOToEmployee(EmployeeUpdateRequestDTO dto, Employee employee);
}
