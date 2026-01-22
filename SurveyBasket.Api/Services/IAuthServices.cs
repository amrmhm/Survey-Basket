using OneOf;
using SurveyBasket.Api.Contract.Authentication;

namespace SurveyBasket.Api.Services;

public interface IAuthServices
{
    //public Task<OneOf<ResponseAuth,Error>> GetTokenAsync (string email, string password, CancellationToken cancellationToken = default);



    public Task<Resault<ResponseAuth>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<Resault<ResponseAuth>> GetRefreshTokenAsync (string token, string refreshToken, CancellationToken cancellationToken = default);
    public Task<Resault> RevokeRefreshTokenAsync (string token, string refreshToken, CancellationToken cancellationToken = default);
}
