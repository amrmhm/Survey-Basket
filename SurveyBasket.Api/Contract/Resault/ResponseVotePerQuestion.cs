namespace SurveyBasket.Api.Contract.Resault;

public record ResponseVotePerQuestion(
    string Question,
    IEnumerable<ResponseVotePerAnswer> SelectedAnswer
    );
