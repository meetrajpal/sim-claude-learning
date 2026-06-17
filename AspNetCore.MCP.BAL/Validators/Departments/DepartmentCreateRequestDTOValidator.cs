namespace AspNetCore.MCP.BAL.Validators.Departments;

public class DepartmentCreateRequestDTOValidator : AbstractValidator<DepartmentCreateRequestDTO>
{
    public DepartmentCreateRequestDTOValidator(IFileLogger logger)
    {
        logger.Log("Validating department creation");
        RuleFor(x => x.DepartmentName).NotNull().NotEmpty().WithMessage("DepartmentName is required.");
    }
}
