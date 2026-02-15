namespace SurveyBasket.Api.Contract.Users;

public record RequestUpdateUser
(
    string FirstName,
    string LastName,
    string Email,
    IList<string> Roles
    );
