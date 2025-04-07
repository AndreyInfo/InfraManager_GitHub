using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProductCatalogImportSettingConfiguration : ProductCatalogImportSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ProductCatalogImportSetting";

        protected override ProductCatalogImportSettingTypesConfigurationBase ManyToManyTableConfig => new ProductCatalogImportSettingTypesConfiguration();

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSetting> builder)
        {
            builder.ToTable("ProductCatalogImportSetting", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name");

            builder.Property(x => x.Note).HasColumnName("Note");

            builder.Property(x => x.RestoreRemovedModels).HasColumnName("RestoreRemovedModels");

            builder.Property(x => x.TechnologyTypeID).HasColumnName("TechnologyTypeID");

            builder.Property(x => x.JackTypeID).HasColumnName("JackTypeID");

            builder.Property(x => x.ProductCatalogImportCSVConfigurationID)
                .HasColumnName("ProductCatalogImportCSVConfigurationID");

            builder.Property(x => x.Path).HasColumnName("Path");

            builder.Property(x => x.RowVersion).HasColumnName("RowVersion");
        }
    }
}