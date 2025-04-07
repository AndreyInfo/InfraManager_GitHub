using InfraManager.DAL.Asset;
using Inframanager.DAL.ProductCatalogue.Import;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class
        ProductCatalogImportSettingConfigurationBase : IEntityTypeConfiguration<ProductCatalogImportSetting>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract ProductCatalogImportSettingTypesConfigurationBase ManyToManyTableConfig { get; }
        protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSetting> entity);

        public void Configure(EntityTypeBuilder<ProductCatalogImportSetting> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(250);

            entity.Property(x => x.Note).IsRequired(true);

            entity.Property(x => x.RestoreRemovedModels).IsRequired(true);

            entity.Property(x => x.Path).IsRequired(true);


            entity.HasOne(x => x.TechnologyType)
                .WithMany()
                .HasForeignKey(x => x.TechnologyTypeID);

            entity.HasOne(x => x.ConnectorType)
                .WithMany()
                .HasForeignKey(x => x.JackTypeID);

            entity.HasOne(x => x.ProductCatalogImportCSVConfiguration)
                .WithMany()
                .HasForeignKey(x => x.ProductCatalogImportCSVConfigurationID);

            ConfigureDatabase(entity);
        }
    }
}