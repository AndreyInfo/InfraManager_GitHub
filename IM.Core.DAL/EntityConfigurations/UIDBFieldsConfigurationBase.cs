using InfraManager.DAL.Database.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class UIDBFieldsConfigurationBase : IEntityTypeConfiguration<UIDBFields>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<UIDBFields> entity);

        public void Configure(EntityTypeBuilder<UIDBFields> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.ID);

            entity.Property(x => x.ConfigurationID);

            entity.Property(x => x.FieldID);

            entity.Property(x => x.Value).IsRequired(false).HasMaxLength(1024);

            
            entity.HasOne(x => x.Configuration)
                .WithMany()
                .HasForeignKey(x => x.ConfigurationID);

            ConfigureDatabase(entity);
        }
    }
}