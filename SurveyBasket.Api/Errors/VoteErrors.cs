namespace SurveyBasket.Api.Errors;

public class VoteErrors
{
    public static readonly Error InvalidQuestion = new("Poll.InvalidQuestion", "InvalidQuestion", StatusCodes.Status400BadRequest);
    public static readonly Error DuplicateVote = new("Vole.DuplicatVote", "This User Is Already Vote Before Poll",StatusCodes.Status409Conflict);
}
