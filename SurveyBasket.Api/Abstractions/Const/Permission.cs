namespace SurveyBasket.Api.Abstractions.Const;

public static class Permission
{
    public static string Type { get; } = "permissions";

    public const string GetPolls = "polls:read";
    public const string AddPolls = "polls:add";
    public const string UpdatePolls = "polls:update";
    public const string DeletePolls = "polls:delete";


    public const string GetQuestions = "questions:read";
    public const string AddQuestions = "questions:add";
    public const string UpdatQuestions = "questions:update";


    public const string GetUsers = "users:read";
    public const string AddUsers = "users:add";
    public const string UpdateUsers = "users:update";


    public const string GetRoles = "roles:read";
    public const string AddPRoles = "roles:add";
    public const string UpdateRoles = "roles:update";

    public const string GetResaults = "resaults:read";


    public static IList<string?> GetAllPermissions() =>
        typeof(Permission).GetFields().Select(c => c.GetValue(c) as string).ToList();


}
