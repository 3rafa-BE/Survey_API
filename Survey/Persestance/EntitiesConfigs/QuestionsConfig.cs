using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Survey.Models;

namespace Survey.Persestance.EntitiesConfigs
{
    public class QuestionsConfig : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(x => new { x.Content, x.Pollid }).IsUnique();
            builder.Property(x => x.Content).HasMaxLength(1000);
        }
    }
}
