namespace AspNetCore.MCP.BAL.Mappers.Interfaces;

public interface IEmployeeMapper
{
    Employee EmployeeCreateRequestDTOToEmployee(EmployeeCreateRequestDTO dto);
    void EmployeeUpdateRequestDTOToEmployee(EmployeeUpdateRequestDTO dto, Employee employee);
}
