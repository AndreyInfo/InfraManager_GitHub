using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class HighlightingConditionValueConfigurationBase : IEntityTypeConfiguration<HighlightingConditionValue>
    {
        protected abstract string KeyName { get; }
        protected abstract string HighlightingConditionFK { get; }
        protected abstract string PriorityFK { get; }
        protected abstract string UrgencyFK { get; }
        protected abstract string InfluenceFK { get; }
        protected abstract string SlaFK { get; }

        public void Configure(EntityTypeBuilder<HighlightingConditionValue> builder)
        {
            builder.HasKey(x => x.ID).HasName(KeyName);

            builder.Property(x => x.StringValue).HasMaxLength(255).IsRequired(false);

            builder.HasOne(c => c.HighlightingCondition)
            .WithMany(c => c.HighlightingConditionValue)
            .HasForeignKey(c => c.HighlightingConditionID)
            .HasConstraintName(HighlightingConditionFK)
            .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Priority)
            .WithMany()
            .HasForeignKey(c => c.PriorityID)
            .HasConstraintName(PriorityFK);

            builder.HasOne(c => c.Urgency)
            .WithMany()
            .HasForeignKey(c => c.UrgencyID)
            .HasConstraintName(UrgencyFK);

            builder.HasOne(c => c.Influence)
            .WithMany()
            .HasForeignKey(c => c.InfluenceID)
            .HasConstraintName(InfluenceFK);

            builder.HasOne(c => c.Sla)
            .WithMany()
            .HasForeignKey(c => c.SlaID)
            .HasConstraintName(SlaFK);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<HighlightingConditionValue> builder);
    }
}
