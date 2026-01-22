namespace SurveyBasket.Api.Errors;

public class QuestionErrors
{
    public static readonly Error NotFound = new("Question.NotFound", "No Question Was Found With Given Id", StatusCodes.Status404NotFound);
    public static readonly Error DuplicateQuestion = new("Question.DuplicateQuestion", "Another Question Have Already Exist Content", StatusCodes.Status409Conflict);
}
