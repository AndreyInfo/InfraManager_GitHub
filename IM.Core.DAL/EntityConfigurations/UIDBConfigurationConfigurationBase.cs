using InfraManager.DAL.Database.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UIDBConfigurationConfigurationBase : IEntityTypeConfiguration<UIDBConfiguration>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIDBConfiguration> entity);

        public void Configure(EntityTypeBuilder<UIDBConfiguration> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID);

            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(255);

            entity.Property(x => x.Note).IsRequired(true).HasMaxLength(255);

            entity.Property(x => x.OrganizationTableName).IsRequired(false).HasMaxLength(50);

            entity.Property(x => x.SubdivisionTableName).IsRequired(false).HasMaxLength(50);

            entity.Property(x => x.UserTableName).IsRequired(false).HasMaxLength(50);


            ConfigureDatabase(entity);
        }
    }
}