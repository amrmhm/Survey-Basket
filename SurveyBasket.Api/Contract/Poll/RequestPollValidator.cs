namespace SurveyBasket.Api.Contract.Poll;

public class RequestAuthValidator : AbstractValidator<RequestPoll>
{
    public RequestAuthValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty()
            .Length(3, 100)
            .WithMessage("{PropertyName} Should Be least {MinLength} and Maximum {MaxLength} ,You Entery {TotalLength}");
        RuleFor(c => c.Summary)
            .NotEmpty()
            .Length(3, 1500)
            .WithMessage("{PropertyName} Should Be least {MinLength} and Maximum {MaxLength} ,You Entery {TotalLength}");
        RuleFor(c => c.StartsAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));
        RuleFor(c => c)
            .Must(ValidDate)
            .WithName(nameof(RequestPoll.EndsAt))
            .WithMessage("{PropertyName} Must Be Gratter Than Start Date");
    }

    public bool ValidDate(RequestPoll requestPoll)
    {
        return requestPoll.EndsAt > requestPoll.StartsAt;
    }
}
