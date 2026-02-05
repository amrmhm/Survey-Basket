

using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class RoleConfigration : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        // Add Default Data To Role Table
        builder.HasData(
         [
            new ApplicationRole
            {
                Id = DefaultRole.AdminRoleId,
                Name = DefaultRole.Admin,
                NormalizedName = DefaultRole.Admin.ToUpper(),
                ConcurrencyStamp = DefaultRole.AdminConcurrencyStamp
            },
            new ApplicationRole
            {
                Id = DefaultRole.MemberRoleId,
                Name = DefaultRole.Member,
                NormalizedName = DefaultRole.Member.ToUpper(),
                ConcurrencyStamp = DefaultRole.MemberConcurrencyStamp,
                IsDefault = true
            }
            ]);
    }
}
