namespace AspNetCore.MCP.BAL.Validators.Employees;

public class EmployeeUpdateRequestDTOValidator : AbstractValidator<EmployeeUpdateRequestDTO>
{
    public EmployeeUpdateRequestDTOValidator()
    {
        RuleFor(x => x.EmployeeName).NotNull().NotEmpty().WithMessage("EmployeeName cannot be empty string.");

        RuleFor(x => x.Salary).NotNull().GreaterThan(0).WithMessage("Salary must be greater than 0.");

        RuleFor(x => x.EmailId).NotNull().NotEmpty().EmailAddress().WithMessage("Valid EmailId is required.");

        RuleFor(x => x.JoiningDate).NotNull().NotEmpty().WithMessage("JoiningDate cannot be empty.");

        RuleFor(x => x.DepartmentId).NotNull().Must(id => Guid.TryParse(id, out _)).WithMessage("DepartmentId must be a valid GUID.");
    }
}
