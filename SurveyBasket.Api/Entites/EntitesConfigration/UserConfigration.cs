using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    }
}
