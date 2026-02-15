namespace SurveyBasket.Api.Contract.Votes;

public record RequestAnswerVotes(
    int QuestionId,
    int AnswerId
    );
