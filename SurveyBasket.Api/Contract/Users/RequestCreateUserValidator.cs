namespace SurveyBasket.Api.Contract.Users;

public class RequestCreateUserValidator : AbstractValidator<RequestCreateUser>
{
    public RequestCreateUserValidator()
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


        RuleFor(c => c.Password)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(c => c.Roles)
           .NotEmpty()
           .NotNull();

        RuleFor(c => c.Roles)
           .Must(c => c.Distinct().Count() == c.Count)
           .WithMessage(" Can Not Duplicate Role .")
           .When(c => c.Roles != null);
    }




}


