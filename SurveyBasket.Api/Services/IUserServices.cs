using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Services;

public interface IUserServices
{
    public Task<IEnumerable<ResponseUser>> GetAllAsync(CancellationToken cancellationToken = default);
    public Task<Resault<ResponseUser>> GetAsync(string id);
    public Task<Resault<ResponseUser>> CreateAsync(RequestCreateUser request, CancellationToken cancellationToken = default);
    public Task<Resault> UpdateAsync(string id, RequestUpdateUser request, CancellationToken cancellationToken = default);
    public Task<Resault> ToggleStatusAsync(string id);
    public Task<Resault> UnLockUserAsync(string id);
    public Task<Resault<ResponseUserProfile>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);

    public Task<Resault> UpdateProfileAsync(string userId, RequestUpdateProfile request, CancellationToken cancellationToken = default);
    public Task<Resault> ChangePasswordAsync(string userId, RequestChangePassword request, CancellationToken cancellationToken = default);
}
