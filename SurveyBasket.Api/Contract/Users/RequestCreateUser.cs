namespace SurveyBasket.Api.Contract.Users;

public record RequestCreateUser
(
    string FirstName ,
    string LastName ,
    string Email ,
    string Password ,
    IList<string> Roles
    );
