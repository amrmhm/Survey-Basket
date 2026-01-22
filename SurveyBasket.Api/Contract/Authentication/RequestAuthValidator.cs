namespace SurveyBasket.Api.Contract.Authentication;

public class RequestAuthValidator : AbstractValidator<LoginRequest>
{
    public RequestAuthValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(c => c.Password)
            .NotEmpty();
            
    }

   
}
