using InfraManager.DAL.Database.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UIDBSettingsConfigurationBase : IEntityTypeConfiguration<UIDBSettings>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIDBSettings> entity);

        public void Configure(EntityTypeBuilder<UIDBSettings> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID);

            entity.Property(x => x.DBConfigurationID).IsRequired(false);

            entity.Property(x => x.DatabaseName).HasMaxLength(50).IsRequired(false);

            entity.Property(x => x.Removed);

            ConfigureDatabase(entity);
        }
    }
}