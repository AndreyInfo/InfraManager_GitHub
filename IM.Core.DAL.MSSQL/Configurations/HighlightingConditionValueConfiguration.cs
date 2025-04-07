using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class HighlightingConditionValueConfiguration : HighlightingConditionValueConfigurationBase
    {
        protected override string KeyName => "PK_HighlightingConditionValue";
        protected override string HighlightingConditionFK => "FK_HighlightingConditionValue_HighlightingCondition";
        protected override string PriorityFK => "FK_HighlightingConditionValue_Priority";
        protected override string UrgencyFK => "FK_HighlightingConditionValue_Urgency";
        protected override string InfluenceFK => "FK_HighlightingConditionValue_Influence";
        protected override string SlaFK => "FK_HighlightingConditionValue_Sla";

        protected override void ConfigureDatabase(EntityTypeBuilder<HighlightingConditionValue> builder)
        {
            builder.ToTable("HighlightingConditionValue", Options.Scheme);
            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.HighlightingConditionID).HasColumnName("HighlightingConditionID");
            builder.Property(x => x.PriorityID).HasColumnName("PriorityID");
            builder.Property(x => x.UrgencyID).HasColumnName("UrgencyID");
            builder.Property(x => x.InfluenceID).HasColumnName("InfluenceID");
            builder.Property(x => x.SlaID).HasColumnName("SlaID");
            builder.Property(x => x.IntValue1).HasColumnName("IntValue1");
            builder.Property(x => x.IntValue2).HasColumnName("IntValue2");
            builder.Property(x => x.StringValue).HasColumnName("StringValue");
        }
    }
}
