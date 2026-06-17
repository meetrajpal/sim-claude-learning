namespace AspNetCore.MCP.BAL.Mappers.Interfaces;

public interface IDepartmentMapper
{
    Department DepartmentCreateRequestDTOToDepartment(DepartmentCreateRequestDTO dto);

    void DepartmentUpdateRequestDTOToDepartment(DepartmentUpdateRequestDTO dto, Department department);
}
