namespace SurveyBasket.Api.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredintial =
        new("User.InvalidCredintial", "Invalid Email / Password",StatusCodes.Status404NotFound);
    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid Jwt token" , StatusCodes.Status404NotFound);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status404NotFound);
}
