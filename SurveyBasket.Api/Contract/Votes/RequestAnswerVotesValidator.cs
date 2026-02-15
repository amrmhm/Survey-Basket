namespace SurveyBasket.Api.Contract.Votes;

public class RequestAnswerVotesValidator : AbstractValidator<RequestAnswerVotes>
{

    public RequestAnswerVotesValidator()
    {
        RuleFor(c => c.QuestionId)
             .GreaterThan(0);
        RuleFor(c => c.AnswerId)
             .GreaterThan(0);
    }
}
