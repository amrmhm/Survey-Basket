namespace SurveyBasket.Api.Contract.Authentication;

public class RequestResetPasswordValidator : AbstractValidator<RequestResetPassword>
{
    public RequestResetPasswordValidator()
    {
        RuleFor(c => c.Email)
           .NotEmpty()
           .EmailAddress();

        RuleFor(c => c.Code)
           .NotEmpty();

        RuleFor(c => c.NewPassword)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");
    }
}
