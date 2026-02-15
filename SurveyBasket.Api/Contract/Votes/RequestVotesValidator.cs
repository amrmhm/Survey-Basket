namespace SurveyBasket.Api.Contract.Votes;

public class RequestVotesValidator : AbstractValidator<RequestVotes>
{
    public RequestVotesValidator()
    {
        RuleFor(c => c.AnswerVotes)
            .NotNull();
        RuleForEach(c => c.AnswerVotes).SetInheritanceValidator(c => c.Add(new RequestAnswerVotesValidator()));
    }
}
