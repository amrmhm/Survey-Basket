namespace SurveyBasket.Api.Contract.Resault;

public record ResponseVotePerDay(
    DateOnly Date ,
    int NumberOfVote
    
    );
