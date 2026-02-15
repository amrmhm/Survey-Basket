namespace SurveyBasket.Api.Authentication;

public interface IJwtProvider
{
    (string token, int expireIn) GenrateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
