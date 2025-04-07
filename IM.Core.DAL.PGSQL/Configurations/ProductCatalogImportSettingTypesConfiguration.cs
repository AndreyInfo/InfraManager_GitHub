using IM.Core.DAL.Postgres;
using Inframanager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProductCatalogImportSettingTypesConfiguration : ProductCatalogImportSettingTypesConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_product_catalog_import_setting_types";

        public override string TableName => "product_catalog_import_setting_types";

        public override string SchemaName => Options.Scheme;

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSettingTypes> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.Property(x => x.ProductCatalogImportSettingID).HasColumnName("product_catalog_import_setting_id");

            builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        }
    }
}