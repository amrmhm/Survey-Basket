namespace SurveyBasket.Api.Contract.Authentication;

public class RequestRegisterValidation : AbstractValidator<RequestRegister>
{
    public RequestRegisterValidation()
    {
        RuleFor(c => c.Email)
           .NotEmpty()
           .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty()
            .Matches(RegexPattern.Password)
            .WithMessage("Password should be at least 8 digits and should contains Lowercase, NonAlphanumeric and Uppercase");

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .Length(3, 100);


        RuleFor(c => c.LastName)
            .NotEmpty()
            .Length(3, 100);

    }
}
