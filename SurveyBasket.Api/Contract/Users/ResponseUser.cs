namespace SurveyBasket.Api.Contract.Users;

public record ResponseUser(
    string Id ,
    string FirstName ,
    string LastName ,
    string Email ,
    bool   IsDisabled ,
    IEnumerable<string> Roles 
    );
