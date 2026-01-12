

namespace SurveyBasket.Api.Contract.Validation;

public class RequestPollValidator : AbstractValidator<RequestPoll>
{
    public RequestPollValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .Length(3,100)
            .WithMessage("{PropertyName} Should Be least {MinLength} and Maximum {MaxLength} ,You Entery {TotalLength}");
    }
}
