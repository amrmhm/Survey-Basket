namespace SurveyBasket.Api.Errors;

public class PollErrors
{
    public static readonly Error NotFound = new("Poll.NotFound", "No Poll Was Found With Given Id",StatusCodes.Status404NotFound);
    public static readonly Error DuplicatePoll = new("Poll.DuplicatePoll", "Another Poll Have Already Exist Title",StatusCodes.Status409Conflict);
}
