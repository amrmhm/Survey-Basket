namespace SurveyBasket.Api.Contract.Authentication;

public record RequestResetPassword
(
    string Email,
    string Code,
    string NewPassword
    );
