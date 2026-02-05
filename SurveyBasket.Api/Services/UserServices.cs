using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Services;

public class UserServices(UserManager<ApplicationUser> userManager) : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task<Resault<ResponseUserProfile>> GetProfileAsync(string userId , CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users
            .Where(c => c.Id == userId)
            .ProjectToType<ResponseUserProfile>()
            .SingleAsync(cancellationToken);

        return Resault.Success(user.Adapt<ResponseUserProfile>());
    }

    public async Task<Resault> UpdateProfileAsync(string userId, RequestUpdateProfile request, CancellationToken cancellationToken = default)
    {
        // TO check Methods FindByIdAsync if Cancellation is found Or Not
        //cancellationToken.ThrowIfCancellationRequested();

        //var user = await _userManager.FindByIdAsync(userId);

        //cancellationToken.ThrowIfCancellationRequested();

        //request.Adapt(user);

        //TO Performace ====================================

        var user = await _userManager.Users
            .Where(c => c.Id == userId)
            .ExecuteUpdateAsync(setter => setter
            .SetProperty(c => c.FirstName, request.FirstName)
            .SetProperty(c => c.LastName, request.LastName),cancellationToken);
        return Resault.Success();
    }

    public async Task<Resault> ChangePasswordAsync (string userId, RequestChangePassword request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.FindByIdAsync(userId);

        cancellationToken.ThrowIfCancellationRequested();
        var resault = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if(resault.Succeeded)
            return Resault.Success();

        var error = resault.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description,StatusCodes.Status400BadRequest));


    }
}
