namespace SurveyBasket.Api.Contract.Users;

public record ResponseUserProfile
(
    string Email ,
    string UserName ,
    string FirstName ,
    string LastName 
    );
