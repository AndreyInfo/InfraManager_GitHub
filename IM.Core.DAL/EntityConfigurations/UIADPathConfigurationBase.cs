using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADPathConfigurationBase : IEntityTypeConfiguration<UIADPath>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADPath> entity);

        public void Configure(EntityTypeBuilder<UIADPath> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID).IsRequired(true);

            entity.Property(x => x.ADSettingID).IsRequired(true);

            entity.Property(x => x.Path).IsRequired(false).HasMaxLength(4000);

            ConfigureDatabase(entity);
        }
    }
}