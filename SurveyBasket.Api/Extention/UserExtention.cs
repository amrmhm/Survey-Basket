using System.Security.Claims;

namespace SurveyBasket.Api.Extention;

public static class UserExtention
{
    public static string? GetUserId (this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
