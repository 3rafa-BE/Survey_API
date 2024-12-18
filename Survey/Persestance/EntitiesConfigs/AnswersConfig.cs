using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Survey.Models;

namespace Survey.Persestance.EntitiesConfigs
{
    public class AnswersConfig : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            builder.Property(x => x.Content).HasMaxLength(1000);
            builder.HasIndex(x => new { x.QuestionId, x.Content }).IsUnique();
        }
    }
}
