namespace SurveyBasket.Api.Contract.Resault;

public record ResponseVotePerAnswer(
    string Answer,
    int Count
    );
