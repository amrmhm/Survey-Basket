namespace SurveyBasket.Api.Contract.Authentication;

public class RequestForgetPasswordValidator : AbstractValidator<RequestForgetPassword>
{
    public RequestForgetPasswordValidator()
    {
        RuleFor(c => c.Email)
           .NotEmpty()
           .EmailAddress();
    }
}
