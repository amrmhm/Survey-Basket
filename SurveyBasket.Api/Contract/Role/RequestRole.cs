namespace SurveyBasket.Api.Contract.Role;

public record RequestRole
(
    string Name,
    IList<string> Permissions

);
