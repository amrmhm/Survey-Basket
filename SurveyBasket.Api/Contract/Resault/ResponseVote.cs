namespace SurveyBasket.Api.Contract.Resault;

public record ResponseVote (
    string VoterName ,
    DateTime VoteDate ,
    IEnumerable<ResponseQuestionAnswer> SelectedAnswer
    
    );
