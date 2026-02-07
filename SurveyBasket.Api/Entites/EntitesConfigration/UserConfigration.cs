

using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class UserConfigration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        
        builder.Property(c => c.FirstName).HasMaxLength(100);
        builder.Property(c => c.LastName).HasMaxLength(100);
        builder.OwnsMany(c => c.RefreshToken)
            .ToTable("RefreshTokens")
            .WithOwner()
            .HasForeignKey("UserId");

        // Add Default Data To User Table

        //Hash the password before seeding the data
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        builder.HasData(
            new ApplicationUser
            {
                Id = DefaultUser.AdminId,
                Email = DefaultUser.AdminEmail,
                NormalizedEmail = DefaultUser.AdminEmail.ToUpper(),
                UserName = DefaultUser.AdminEmail,
                NormalizedUserName = DefaultUser.AdminEmail.ToUpper(),
                FirstName = DefaultUser.AdminFirstName,
                LastName = DefaultUser.AdminLastName,
                SecurityStamp = DefaultUser.AdminSecurityStamp,
                ConcurrencyStamp = DefaultUser.AdminConcurrencyStamp,
                EmailConfirmed = true,
                PasswordHash = passwordHasher.HashPassword(null!, DefaultUser.AdminPasswoard)
                

            });
        
    }
}
