

using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class RoleClaimConfigration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
    {



        // Add Default Data To RoleClaim Table

        var permission = Permission.GetAllPermissions();

        var adminCliam = new List<IdentityRoleClaim<string>>();

        for (int i = 0; i < permission.Count; i++)
        {
            adminCliam.Add(new IdentityRoleClaim<string>
            {
                Id = i + 1,
                ClaimType = Permission.Type,
                RoleId = DefaultRole.AdminRoleId,
                ClaimValue = permission[i]
            });
        }

        builder.HasData(adminCliam);

       
    }
}
