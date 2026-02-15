namespace SurveyBasket.Api.Contract.Role;

public class RequestFilterValidator : AbstractValidator<RequestRole>
{
    public RequestFilterValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("Role name is required.")
            .Length(3, 256);
        RuleFor(c => c.Permissions)
            .NotNull()
            .NotEmpty();
        RuleFor(c => c.Permissions)
            .Must(c => c.Distinct().Count() == c.Count)
            .WithMessage(" Can Not Duplicate Permissions .");
    }
}
