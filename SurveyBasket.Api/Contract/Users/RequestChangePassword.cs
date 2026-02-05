namespace SurveyBasket.Api.Contract.Users;

public record RequestChangePassword
(
    string CurrentPassword,
    string NewPassword);
