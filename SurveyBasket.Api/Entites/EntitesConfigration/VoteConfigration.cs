namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class VoteConfigration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(c => new { c.PollId, c.UserId }).IsUnique();


    }
}
