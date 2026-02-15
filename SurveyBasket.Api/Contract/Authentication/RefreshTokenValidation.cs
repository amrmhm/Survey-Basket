namespace SurveyBasket.Api.Contract.Authentication;

public class RefreshTokenValidation : AbstractValidator<RequestRefreshToken>
{
    public RefreshTokenValidation()
    {
        RuleFor(c => c.token).NotEmpty();
        RuleFor(c => c.refreshToken).NotEmpty();

    }
}
