

using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class UserRoleConfigration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
       
        // Add Default Data To User Role Table


        builder.HasData(
           new IdentityUserRole<string>
           {
                RoleId = DefaultRole.AdminRoleId,
                UserId = DefaultUser.AdminId
           });
    }
}
