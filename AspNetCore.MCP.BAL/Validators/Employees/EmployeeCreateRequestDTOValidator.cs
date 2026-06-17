namespace AspNetCore.MCP.BAL.Validators.Employees;

public class EmployeeCreateRequestDTOValidator : AbstractValidator<EmployeeCreateRequestDTO>
{
    public EmployeeCreateRequestDTOValidator()
    {
        RuleFor(x => x.EmployeeName).NotEmpty().WithMessage("EmployeeName is required.");

        RuleFor(x => x.Salary).NotNull().GreaterThan(0).WithMessage("Salary must be greater than 0.");

        RuleFor(x => x.EmailId).NotEmpty().EmailAddress().WithMessage("Valid EmailId is required.");

        RuleFor(x => x.JoiningDate).NotEmpty().WithMessage("JoiningDate is required.");

        RuleFor(x => x.DepartmentId).Must(id => Guid.TryParse(id, out _)).WithMessage("DepartmentId must be a valid GUID.");
    }
}
