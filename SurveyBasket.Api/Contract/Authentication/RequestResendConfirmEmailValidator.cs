namespace SurveyBasket.Api.Contract.Authentication;

public class RequestResendConfirmEmailValidator : AbstractValidator<RequestResendConfirmEmail>
{
    public RequestResendConfirmEmailValidator()
    {
        RuleFor(c => c.Email)
            .NotEmpty()
            .EmailAddress();


    }
}
