namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class QuestionConfigration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasIndex(c => new { c.Content, c.PollId }).IsUnique();
        builder.Property(c => c.Content).HasMaxLength(1000);

    }
}
