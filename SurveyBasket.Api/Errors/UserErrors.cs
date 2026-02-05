namespace SurveyBasket.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredintial =
        new("User.InvalidCredintial", "Invalid Email / Password",StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token" , StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

    public static readonly Error DuplicateEmail =
        new("User.DuplicateEmail", "Another User With Same Email Has Already Exist", StatusCodes.Status409Conflict);
    public static readonly Error EmailNotConfirmed =
    new("User.EmailNotConfirmed", "Email Is Not Confirmed", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode =
    new("User.InvalidCode", "Invalid Code", StatusCodes.Status401Unauthorized);
    public static readonly Error DuplicatedConfirmation =
    new("User.DuplicatedConfirmation", "Email Already Confirmed", StatusCodes.Status401Unauthorized);
}
