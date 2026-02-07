using SurveyBasket.Api.Abstractions.Const;

namespace SurveyBasket.Api.Contract.Users;

public class RequestUpdateUserValidator : AbstractValidator<RequestUpdateUser>
{
    public RequestUpdateUserValidator()
    {


        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .Length(3, 100);


        RuleFor(c => c.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(c => c.Roles)
           .NotEmpty()
           .NotNull();

        RuleFor(c => c.Roles)
           .Must(c => c.Distinct().Count() == c.Count)
           .WithMessage(" Can Not Duplicate Role .")
           .When ( c => c.Roles != null);
    }




}


