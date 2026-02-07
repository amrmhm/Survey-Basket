namespace SurveyBasket.Api.Errors;

public static class RoleErrors
{
    public static readonly Error NotFound =
        new("Role.NotFound", "Role Is Not Found",StatusCodes.Status404NotFound);
    public static readonly Error InvalidPermission =
        new("Role.InvalidPermission", "Invalid Permission", StatusCodes.Status400BadRequest);

    //public static readonly Error InvalidRefreshToken =
    //    new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicateRole =
        new("Role.DuplicateRole", "Another Role With Same Name Has Already Exist", StatusCodes.Status409Conflict);
    //public static readonly Error EmailNotConfirmed =
    //new("User.EmailNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status401Unauthorized);
    //public static readonly Error InvalidCode =
    //new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    //public static readonly Error DuplicatedConfirmation =
    //new("User.DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status401Unauthorized);
}
