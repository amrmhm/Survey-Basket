namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class PollConfigration : IEntityTypeConfiguration<Poll>
{
    public void Configure(EntityTypeBuilder<Poll> builder)
    {
        builder.HasIndex(c => c.Title).IsUnique();
        builder.Property(c => c.Title).HasMaxLength(100);
        builder.Property(c => c.Summary).HasMaxLength(1500);
    }
}
