namespace SurveyBasket.Api.Contract.Authentication;

public record RequestConfirmEmail
(
    string UserId ,
    string Code
    );
