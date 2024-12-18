using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Survey.Models;

namespace Survey.Persestance.EntitiesConfigs
{
    public class VoteAnswersConfig : IEntityTypeConfiguration<VoteAnswers>
    {
        public void Configure(EntityTypeBuilder<VoteAnswers> builder)
        {
            builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique();
        }
    }
}
