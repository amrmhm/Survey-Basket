using SurveyBasket.Api.Abstractions.Const;

namespace SurveyBasket.Api.Contract.Users;

public class RequestChangePasswordValidator : AbstractValidator<RequestChangePassword>
{
    public RequestChangePasswordValidator()
    {


        RuleFor(c => c.CurrentPassword)
            .NotEmpty();

        RuleFor(c => c.NewPassword)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase")
            .NotEqual(c => c.CurrentPassword)
            .WithMessage("New Password Can Not Same Current Password"); 

      

    }
}

