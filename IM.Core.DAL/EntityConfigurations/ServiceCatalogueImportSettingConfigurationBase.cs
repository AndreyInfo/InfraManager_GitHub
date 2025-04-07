using InfraManager.DAL.Import.ServiceCatalogue;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ServiceCatalogueImportSettingConfigurationBase : IEntityTypeConfiguration<ServiceCatalogueImportSetting>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string ForeignKeyName { get; }
        protected abstract string UI_Name { get; }


        public void Configure(EntityTypeBuilder<ServiceCatalogueImportSetting> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID);

            builder.Property(e => e.Name)
                .HasMaxLength(250)
                .IsRequired(true);

            builder.Property(e => e.Note)
                .IsRequired(true)
                .HasMaxLength(500);

            builder.Property(x => x.ServiceCatalogueImportCSVConfigurationID);

            builder.Property(e => e.Path)
                .IsRequired(true)
                .HasMaxLength(500);
            builder.HasIndex(x => x.Name).HasDatabaseName(UI_Name).IsUnique();

            builder.HasOne(x => x.ServiceCatalogueImportCSVConfiguration)
               .WithOne()
               .HasForeignKey<ServiceCatalogueImportSetting>(e => e.ServiceCatalogueImportCSVConfigurationID)
               .HasConstraintName(ForeignKeyName); 

            OnConfigurePartial(builder);
        }

        protected abstract void OnConfigurePartial(EntityTypeBuilder<ServiceCatalogueImportSetting> builder);
    }
}


