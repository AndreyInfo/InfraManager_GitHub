using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class HighlightingConditionConfiguration : HighlightingConditionConfigurationBase
    {
        protected override string KeyName => "PK_HighlightingCondition";
        protected override string HighlightingConditionFK => "FK_HighlightingCondition_Highlighting";

        protected override void ConfigureDatabase(EntityTypeBuilder<HighlightingCondition> builder)
        {
            builder.ToTable("HighlightingCondition", Options.Scheme);
            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.HighlightingID).HasColumnName("HighlightingID");
            builder.Property(x => x.DirectoryParameter).HasColumnName("DirectoryParameter");
            builder.Property(x => x.EnumParameter).HasColumnName("EnumParameter");
            builder.Property(x => x.Condition).HasColumnName("Condition");
            builder.Property(x => x.BackgroundColor).HasColumnName("BackgroundColor");
            builder.Property(x => x.FontColor).HasColumnName("FontColor");
        }
    }
}
