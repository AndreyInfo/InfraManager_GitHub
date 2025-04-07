using Inframanager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class ProductCatalogImportSettingTypesConfiguration : ProductCatalogImportSettingTypesConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ProductCatalogImportSettingTypes";

        public override string TableName => "ProductCatalogImportSettingTypes";

        public override string SchemaName => "dbo";

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSettingTypes> builder)
        {
            builder.ToTable(TableName, SchemaName);

            builder.Property(x => x.ProductCatalogImportSettingID).HasColumnName("ProductCatalogImportSettingID");

            builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        }
    }
}