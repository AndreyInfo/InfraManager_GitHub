using InfraManager.DAL.Highlightings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class HighlightingConfigurationBase : IEntityTypeConfiguration<Highlighting>
    {
        protected abstract string KeyName { get; }

        public void Configure(EntityTypeBuilder<Highlighting> builder)
        {
            builder.HasKey(x => x.ID).HasName(KeyName);
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired(true);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Highlighting> builder);
    }
}
