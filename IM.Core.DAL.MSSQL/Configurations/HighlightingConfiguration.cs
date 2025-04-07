using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations
{
    internal class HighlightingConfiguration : HighlightingConfigurationBase
    {
        protected override string KeyName => "PK_Highlighting";

        protected override void ConfigureDatabase(EntityTypeBuilder<Highlighting> builder)
        {
            builder.ToTable("Highlighting", Options.Scheme);
            builder.Property(x => x.ID).HasColumnName("Id");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.Sequence).HasColumnName("Sequence");
        }
    }
}
