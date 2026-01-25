namespace SurveyBasket.Api.Contract.Resault;

public record ResponsePollVotes(
    string Title ,
    IEnumerable <ResponseVote> Votes
    );
