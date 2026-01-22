namespace SurveyBasket.Api.Contract.Question;

public record RequestQuestion(
    string Content , 
    List<string> Answer
    );
