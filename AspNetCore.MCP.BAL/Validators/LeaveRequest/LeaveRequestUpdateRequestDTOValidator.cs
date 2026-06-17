namespace AspNetCore.MCP.BAL.Validators.LeaveRequest;

public class LeaveRequestUpdateRequestDTOValidator : AbstractValidator<LeaveRequestUpdateRequestDTO>
{
    public LeaveRequestUpdateRequestDTOValidator()
    {
        RuleFor(x => x.LeaveType).NotEmpty().WithMessage("LeaveType is required.");

        RuleFor(x => x.Status).NotEmpty().WithMessage("Status is required.");

        RuleFor(x => x.StartDate).NotEmpty().WithMessage("StartDate is required.");

        RuleFor(x => x.EndDate).NotEmpty().WithMessage("EndDate is required.")
                                 .GreaterThan(x => x.StartDate)
                                 .WithMessage("EndDate must be greater than StartDate.");

        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("EmployeeId is required.");
    }
}
