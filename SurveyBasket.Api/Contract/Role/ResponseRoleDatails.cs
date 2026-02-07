namespace SurveyBasket.Api.Contract.Role;

public record ResponseRoleDatails
(
    string Id ,
    string Name,
    bool IsDelete ,
    IEnumerable<string> Permissions

    );
