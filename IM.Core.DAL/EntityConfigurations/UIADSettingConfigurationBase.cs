using Inframanager.DAL.ActiveDirectory.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class UIADSettingConfigurationBase : IEntityTypeConfiguration<UIADSetting>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIADSetting> entity);

        public void Configure(EntityTypeBuilder<UIADSetting> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            ConfigureDatabase(entity);
        }
    }
}