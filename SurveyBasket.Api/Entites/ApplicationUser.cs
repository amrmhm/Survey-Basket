
namespace SurveyBasket.Api.Entites;

public class ApplicationUser :IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public List<RefreshToken> RefreshToken { get; set; } = [];
}
