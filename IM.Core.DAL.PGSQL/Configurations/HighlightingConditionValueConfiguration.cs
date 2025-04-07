using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class HighlightingConditionValueConfiguration : HighlightingConditionValueConfigurationBase
    {
        protected override string KeyName => "pk_highlighting_condition_value";
        protected override string HighlightingConditionFK => "fk_highlighting_condition_value_highlighting_condition";
        protected override string PriorityFK => "fk_highlighting_condition_value_priority";
        protected override string UrgencyFK => "fk_highlighting_condition_value_urgency";
        protected override string InfluenceFK => "fk_highlighting_condition_value_influence";
        protected override string SlaFK => "fk_highlighting_condition_value_sla";

        protected override void ConfigureDatabase(EntityTypeBuilder<HighlightingConditionValue> builder)
        {
            builder.ToTable("highlighting_condition_value", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(x => x.HighlightingConditionID).HasColumnName("highlighting_condition_id");
            builder.Property(x => x.PriorityID).HasColumnName("priority_id");
            builder.Property(x => x.UrgencyID).HasColumnName("urgency_id");
            builder.Property(x => x.InfluenceID).HasColumnName("influence_id");
            builder.Property(x => x.SlaID).HasColumnName("sla_id");
            builder.Property(x => x.IntValue1).HasColumnName("int_value1");
            builder.Property(x => x.IntValue2).HasColumnName("int_value2");
            builder.Property(x => x.StringValue).HasColumnName("string_value");
        }
    }
}
