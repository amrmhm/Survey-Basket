namespace SurveyBasket.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredintial =
        new("User.InvalidCredintial", "Invalid Email / Password",StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token" , StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);
}
