namespace SurveyBasket.Api.Contract.Authentication;

public record ResponseAuth(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpireIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
    );
