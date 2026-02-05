using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Services;

public interface IUserServices
{
  public Task<Resault<ResponseUserProfile>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);

  public Task<Resault> UpdateProfileAsync(string userId, RequestUpdateProfile request, CancellationToken cancellationToken = default);
    public Task<Resault> ChangePasswordAsync(string userId, RequestChangePassword request, CancellationToken cancellationToken = default);
}
