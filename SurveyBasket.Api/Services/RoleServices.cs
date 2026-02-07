
using SurveyBasket.Api.Abstractions.Const;
using SurveyBasket.Api.Contract.Role;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class RoleServices(RoleManager<ApplicationRole> roleManager , ApplicationDbContext context) : IRoleServices
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<ResponseRole>> GetAllAsync (bool? includeDelete = false, CancellationToken cancellationToken = default)
    {
        //Get all roles that are not default roles, optionally including deleted roles

        var role = await _roleManager.Roles
            .Where(c => !c.IsDefault && (!c.IsDelete || (includeDelete.HasValue && includeDelete.Value)))
            .ProjectToType<ResponseRole>()
            .ToListAsync(cancellationToken);
        return role;
    }
    public async Task<Resault<ResponseRoleDatails>> GetAsync (string id )
    {
        
        if(await _roleManager.FindByIdAsync(id) is not { } role)
            return  Resault.Faliure<ResponseRoleDatails>(RoleErrors.NotFound);

        var permission = await _roleManager.GetClaimsAsync(role);

        var response = new ResponseRoleDatails
        (
            role.Id,
            role.Name!,
            role.IsDelete,
            permission.Select(c => c.Value)
        );

        return Resault.Success(response);

    }

    public async Task<Resault<ResponseRoleDatails>> CreateAsync (RequestRole request)
    {
        var roleIsExist = await _roleManager.FindByNameAsync(request.Name);
        if (roleIsExist is not null)
            return Resault.Faliure<ResponseRoleDatails>(RoleErrors.DuplicateRole);

        var allowPermission = Permission.GetAllPermissions();
        
        if(request.Permissions.Except(allowPermission).Any())
            return Resault.Faliure<ResponseRoleDatails>(RoleErrors.InvalidPermission);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            
        };
        var resualts = await _roleManager.CreateAsync(role);

        if(resualts.Succeeded)
        {
            var permission = request.Permissions.Select(c => new IdentityRoleClaim<string>
            {
                RoleId = role.Id,
                ClaimType = Permission.Type,
                ClaimValue = c
            });

            await _context.RoleClaims.AddRangeAsync(permission);
            await _context.SaveChangesAsync();

            var response = new ResponseRoleDatails
            (
                role.Id,
                role.Name!,
                role.IsDelete,
                request.Permissions);

            return Resault.Success(response);

        }
        var error = resualts.Errors.First();
        return Resault.Faliure<ResponseRoleDatails>(new Error (error.Code , error.Description , StatusCodes.Status400BadRequest));

    }
    public async Task<Resault> UpdateAsync (string id , RequestRole request)
    {
        if(await _roleManager.FindByNameAsync(request.Name) is not { } role)
            return Resault.Faliure(RoleErrors.NotFound);

        var roleIsExist = await _roleManager.Roles.AnyAsync(c => c.Name == request.Name && c.Id != id);
        if(roleIsExist)
            return Resault.Faliure(RoleErrors.DuplicateRole);

        var allowPermission = Permission.GetAllPermissions();
           if( request.Permissions.Except(allowPermission).Any() )
            return Resault.Faliure(RoleErrors.InvalidPermission);
           
        role.Name = request.Name;
        var resualts = await _roleManager.UpdateAsync(role);
        if(resualts.Succeeded)
        {
            var currentPermission = await _context.RoleClaims.Where(
                c => c.RoleId == id && c.ClaimType == Permission.Type)
                .Select(c => c.ClaimValue)
                .ToListAsync();
            var newPermission = request.Permissions.Except(currentPermission)
               .Select(c => new IdentityRoleClaim<string>
               {
                   RoleId = role.Id,
                   ClaimType = Permission.Type,
                   ClaimValue = c
               });
            var removedPermission = currentPermission.Except(request.Permissions);
                 await _context.RoleClaims
                .Where(c => c.RoleId == id && removedPermission.Contains(c.ClaimValue))
                .ExecuteDeleteAsync();

            await _context.RoleClaims.AddRangeAsync(newPermission);
            await _context.SaveChangesAsync();


            return Resault.Success();



        }
        var error = resualts.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));



    }

    public async Task<Resault> ToggleStatusAsync(string id)
    {
        if(await _roleManager.FindByIdAsync(id) is not { } role)
            return Resault.Faliure(RoleErrors.NotFound);
        role.IsDelete = !role.IsDelete;
        var resualts = await _roleManager.UpdateAsync(role);
        if (resualts.Succeeded)
        {
            return Resault.Success();
        }
        var error = resualts.Errors.First();
        return Resault.Faliure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
}
