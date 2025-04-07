using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class HighlightingConditionConfigurationBase : IEntityTypeConfiguration<HighlightingCondition>
    {
        protected abstract string KeyName { get; }
        protected abstract string HighlightingConditionFK { get; }

        public void Configure(EntityTypeBuilder<HighlightingCondition> builder)
        {
            builder.HasKey(x => x.ID).HasName(KeyName);

            builder.Property(x => x.BackgroundColor).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.FontColor).HasMaxLength(50).IsRequired(true);

            builder.HasOne(c => c.Highlighting)
            .WithMany(c => c.HighlightingCondition)
            .HasForeignKey(c => c.HighlightingID)
            .HasConstraintName(HighlightingConditionFK)
            .OnDelete(DeleteBehavior.Cascade);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<HighlightingCondition> builder);
    }
}
