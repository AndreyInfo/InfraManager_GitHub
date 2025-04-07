using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADIMFieldConcordanceConfigurationBase : IEntityTypeConfiguration<UIADIMFieldConcordance>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADIMFieldConcordance> entity);

        public void Configure(EntityTypeBuilder<UIADIMFieldConcordance> entity)
        {
            entity.HasKey(x => new {ID = x.ConfigurationID, x.ClassID, x.IMFieldID}).HasName(PrimaryKeyName);

            entity.Property(x => x.ConfigurationID).IsRequired(true);

            entity.Property(x => x.ClassID).IsRequired(true);

            entity.Property(x => x.Expression).IsRequired(false).HasMaxLength(4096);


            ConfigureDatabase(entity);
        }
    }
}