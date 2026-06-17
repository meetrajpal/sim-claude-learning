namespace AspNetCore.MCP.BAL.Validators.Departments;

public class DepartmentUpdateRequestDTOValidator : AbstractValidator<DepartmentUpdateRequestDTO>
{
    public DepartmentUpdateRequestDTOValidator()
    {
        RuleFor(x => x.DepartmentName).NotNull().NotEmpty().WithMessage("DepartmentName cannot be empty string.");
    }
}
