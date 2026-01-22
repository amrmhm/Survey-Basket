namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token, int expireIn) GenrateToken(ApplicationUser user);
    string? ValidateToken (string  token);
}
