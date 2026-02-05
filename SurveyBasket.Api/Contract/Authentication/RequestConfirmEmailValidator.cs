namespace SurveyBasket.Api.Contract.Authentication;

public class RequestConfirmEmailValidator : AbstractValidator<RequestConfirmEmail>
{
    public RequestConfirmEmailValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty();

        RuleFor(c => c.Code)
            .NotEmpty();

    }
}
