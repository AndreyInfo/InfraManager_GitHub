using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class HighlightingConfiguration : HighlightingConfigurationBase
    {
        protected override string KeyName => "pk_highlighting";

        protected override void ConfigureDatabase(EntityTypeBuilder<Highlighting> builder)
        {
            builder.ToTable("highlighting", Options.Scheme);
            builder.Property(e => e.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(e => e.Sequence).HasColumnName("sequence");
        }
    }
}
