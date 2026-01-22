namespace SurveyBasket.Api.Contract.Question;

public class RequestQuestionValidtor : AbstractValidator<RequestQuestion>
{
    public RequestQuestionValidtor()
    {
        RuleFor(c => c.Content)
            .NotEmpty()
            .Length(3, 1000)
            .WithMessage("{PropertyName} Should Be least {MinLength} and Maximum {MaxLength} ,You Entery {TotalLength}");

        RuleFor(c => c.Answer)
            .NotNull();

        RuleFor(c => c.Answer)
            .Must(c => c.Count > 1)
            .WithMessage("Questions Should Be have 2 Answers")
            .When(c => c.Answer != null);

        RuleFor(c => c.Answer)
            .NotEmpty()
            .Must(c => c.Distinct().Count() == c.Count)
            .WithMessage("You Can Not Duplicated Answers for The Same Question")
            .When(c => c.Answer != null);

    }
}
