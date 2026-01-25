namespace SurveyBasket.Api.Contract.Votes;

public record RequestVotes(
    IEnumerable<RequestAnswerVotes> AnswerVotes
    );
