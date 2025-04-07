using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADConfigurationConfigurationBase : IEntityTypeConfiguration<UIADConfiguration>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADConfiguration> entity);

        public void Configure(EntityTypeBuilder<UIADConfiguration> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID).IsRequired(true);

            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(250);

            entity.Property(x => x.Note).IsRequired(false).HasMaxLength(500);


            ConfigureDatabase(entity);
        }
    }
}