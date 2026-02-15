using SurveyBasket.Api.Contract.Answer;

namespace SurveyBasket.Api.Contract.Question;

public record ResponseQuestion(
    int Id,
    string Content,
    IEnumerable<ResponseAnswer> Answer
    );
