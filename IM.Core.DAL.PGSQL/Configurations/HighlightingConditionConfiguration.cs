using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class HighlightingConditionConfiguration : HighlightingConditionConfigurationBase
    {
        protected override string KeyName => "pk_highlighting_condition";
        protected override string HighlightingConditionFK => "fk_highlighting_condition_highlighting";

        protected override void ConfigureDatabase(EntityTypeBuilder<HighlightingCondition> builder)
        {
            builder.ToTable("highlighting_condition", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(x => x.HighlightingID).HasColumnName("highlighting_id");
            builder.Property(x => x.DirectoryParameter).HasColumnName("directory_parameter");
            builder.Property(x => x.EnumParameter).HasColumnName("enum_parameter");
            builder.Property(x => x.Condition).HasColumnName("condition");
            builder.Property(x => x.BackgroundColor).HasColumnName("background_color");
            builder.Property(x => x.FontColor).HasColumnName("font_color");
        }
    }
}
