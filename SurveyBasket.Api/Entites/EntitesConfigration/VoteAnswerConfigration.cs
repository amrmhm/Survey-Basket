namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class VoteAnswerConfigration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(c => new { c.VoteId, c.QuestionId }).IsUnique();


    }
}
