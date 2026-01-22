using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SurveyBasket.Api.Entites.EntitesConfigration;

public class AnswerConfigration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasIndex(c => new {c.Content , c.QuestionId }).IsUnique();
        builder.Property(c => c.Content).HasMaxLength(1000);
       
    }
}
