using IM.Core.DAL.Postgres;
using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProductCatalogImportSettingConfiguration : ProductCatalogImportSettingConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_product_catalog_import_setting";

        protected override ProductCatalogImportSettingTypesConfigurationBase ManyToManyTableConfig => new ProductCatalogImportSettingTypesConfiguration();

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSetting> builder)
        {
            builder.ToTable("product_catalog_import_setting", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.Name).HasColumnName("name");

            builder.Property(x => x.Note).HasColumnName("note");

            builder.Property(x => x.RestoreRemovedModels).HasColumnName("restore_removed_models");

            builder.Property(x => x.TechnologyTypeID).HasColumnName("technology_type_id");

            builder.Property(x => x.JackTypeID).HasColumnName("jack_type_id");

            builder.Property(x => x.ProductCatalogImportCSVConfigurationID)
                .HasColumnName("product_catalog_import_csv_configuration_id");

            builder.Property(x => x.Path).HasColumnName("path");

            builder.HasXminRowVersion(x=>x.RowVersion);
        }
    }
}