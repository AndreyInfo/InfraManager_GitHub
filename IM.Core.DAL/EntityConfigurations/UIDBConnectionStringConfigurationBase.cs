using InfraManager.DAL.Database.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UIDBConnectionStringConfigurationBase : IEntityTypeConfiguration<UIDBConnectionString>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIDBConnectionString> entity);

        public void Configure(EntityTypeBuilder<UIDBConnectionString> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID);

            entity.Property(x => x.ConnectionString).IsRequired(true).HasMaxLength(1024);

            entity.Property(x => x.ImportSourceType);


            ConfigureDatabase(entity);
        }
    }
}