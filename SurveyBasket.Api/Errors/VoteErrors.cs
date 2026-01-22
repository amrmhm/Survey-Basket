namespace SurveyBasket.Api.Errors;

public class VoteErrors
{
    //public static readonly Error NotFound = new("Poll.NotFound", "No Poll Was Found With Given Id",StatusCodes.Status404NotFound);
    public static readonly Error DuplicateVote = new("Vole.DuplicatVote", "This User Is Already Vote Before Poll",StatusCodes.Status409Conflict);
}
