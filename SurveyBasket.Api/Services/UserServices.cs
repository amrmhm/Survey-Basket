using SurveyBasket.Api.Contract.Users;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class UserServices(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRoleServices roleServices) : IUserServices
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly IRoleServices _roleServices = roleServices;

    public async Task<IEnumerable<ResponseUser>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await (
            from u in _context.Users
            join ur in _context.UserRoles
            on u.Id equals ur.UserId
            join r in _context.Roles
            on ur.RoleId equals r.Id into roles
            where !roles.Any(c => c.Name == DefaultRole.Member)
            select new
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                IsDisabled = u.IsDisabled,
                Roles = roles.Select(c => c.Name!).ToList()
            }).GroupBy(c => new
            {
                c.Id,
                c.FirstName,
                c.LastName,
                c.Email,
                c.IsDisabled
            }).Select(c => new ResponseUser
            (
                c.Key.Id,
                c.Key.FirstName,
                c.Key.LastName,
                c.Key.Email,
                c.Key.IsDisabled,
                c.SelectMany(r => r.Roles)

            )).ToListAsync();
    }

    public async Task<Resault<ResponseUser>> GetAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Resault.Faliure<ResponseUser>(UserErrors.NotFound);
        //Get Roles
        var roles = await _userManager.GetRolesAsync(user);

        // Map User And Roles To ResponseUser
        var response = (user, roles).Adapt<ResponseUser>();
        return Resault.Success(response);
        //Or use 
        //var response = new ResponseUser
        //(
        //    user.Id,
        //    user.FirstName,
        //    user.LastName,
        //    user.Email,
        //    user.IsDisabled,
        //    roles
        //);
        // return Resault.Success(response);

    }

    public async Task<Resault<ResponseUser>> CreateAsync(RequestCreateUser request, CancellationToken cancellationToken = default)
    {
        var emailIsExist = await _userManager.Users.AnyAsync(c => c.Email == request.Email, cancellationToken);
        if (emailIsExist)
            return Resault.Faliure<ResponseUser>(UserErrors.DuplicateEmail);

        var allowedRole = await _roleServices.GetAllAsync(cancellationToken: cancellationToken);


        if (request.Roles.Except(allowedRole.Select(c => c.Name)).Any())
            return Resault.Faliure<ResponseUser>(UserErrors.InvalidRole);

        var user = request.Adapt<ApplicationUser>();

        var resault = await _userManager.CreateAsync(user, request.Password);

        if (resault.Succeeded)
        {
            await _userManager.AddToRolesAsync(user, request.Roles);

            var response = (user, request.Roles).Adapt<ResponseUser>();
            return Resault.Success(response);

        }

        var error = resault.Errors.First();
        return Resault.Faliure<ResponseUser>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Resault> UpdateAsync(string id, RequestUpdateUser request, CancellationToken cancellationToken = default)
    {
        var emailIsExist = await _userManager.Users.AnyAsync(c => c.Id != id && c.Email == request.Email, cancellationToken);
        if (emailIsExist)
            return Resault.Faliure(UserErrors.DuplicateEmail);

        var allowedRole = await _roleServices.GetAllAsync(cancellationToken: cancellationToken);


        if (request.Roles.Except(allowedRole.Select(c => c.Name)).Any())
            return Resault.Faliure(UserErrors.InvalidRole);

        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Resault.Faliure(UserErrors.NotFound);

        user = request.Adapt(user);

        var resault = await _userManager.UpdateAsync(user);

        if (resault.Succeeded)
        {
            await _context.UserRoles
                .Where(c => c.UserId == id)
                .ExecuteDeleteAsync(cancellationToken);

            await _userManager.AddToRolesAsync(user, request.Roles);
            return Resault.Success();

        }

        var error = resault.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Resault> ToggleStatusAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Resault.Faliure(UserErrors.NotFound);
        user.IsDisabled = !user.IsDisabled;

        var resualts = await _userManager.UpdateAsync(user);
        if (resualts.Succeeded)
        {
            return Resault.Success();
        }
        var error = resualts.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


    }
    public async Task<Resault> UnLockUserAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Resault.Faliure(UserErrors.NotFound);


        var resualts = await _userManager.SetLockoutEndDateAsync(user, null);
        if (resualts.Succeeded)
        {
            return Resault.Success();
        }
        var error = resualts.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


    }

    public async Task<Resault<ResponseUserProfile>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
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
            .SetProperty(c => c.LastName, request.LastName), cancellationToken);
        return Resault.Success();
    }

    public async Task<Resault> ChangePasswordAsync(string userId, RequestChangePassword request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var user = await _userManager.FindByIdAsync(userId);

        cancellationToken.ThrowIfCancellationRequested();
        var resault = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        if (resault.Succeeded)
            return Resault.Success();

        var error = resault.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


    }
}
