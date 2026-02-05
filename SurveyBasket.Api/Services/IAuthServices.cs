using OneOf;
using SurveyBasket.Api.Contract.Authentication;

namespace SurveyBasket.Api.Services;

public interface IAuthServices
{
    //public Task<OneOf<ResponseAuth,Error>> GetTokenAsync (string email, string password, CancellationToken cancellationToken = default);



    public Task<Resault<ResponseAuth>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<Resault<ResponseAuth>> GetRefreshTokenAsync (string token, string refreshToken, CancellationToken cancellationToken = default);
    public Task<Resault> RevokeRefreshTokenAsync (string token, string refreshToken, CancellationToken cancellationToken = default);

    public Task<Resault> RegisterAsync(RequestRegister request, CancellationToken cancellationToken = default);

    public Task<Resault> ConfirmEmailAsync(RequestConfirmEmail request);
    public Task<Resault> ResendConfirmEmailAsync(RequestResendConfirmEmail request);

    public Task<Resault> SendResetPasswordAsync(string email);
    public Task<Resault> ResetPasswordAsync(RequestResetPassword request);
}
