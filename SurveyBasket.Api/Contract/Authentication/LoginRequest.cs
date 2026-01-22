namespace SurveyBasket.Api.Contract.Authentication;

public record LoginRequest(
    string Email ,
    string Password
    );
