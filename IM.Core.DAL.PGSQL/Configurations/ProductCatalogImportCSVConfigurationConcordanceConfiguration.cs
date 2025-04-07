using IM.Core.DAL.Postgres;
using Inframanager.DAL.EntityConfigurations;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class
        ProductCatalogImportCSVConfigurationConcordanceConfiguration :
            ProductCatalogImportCSVConfigurationConcordanceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_product_catalog_import_csv_configuration_concordance";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<ProductCatalogImportCSVConfigurationConcordance> builder)
        {
            builder.ToTable("product_catalog_import_csv_configuration_concordance", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("product_catalog_import_csv_configuration_id");

            builder.Property(x => x.Field).HasColumnName("field");

            builder.Property(x => x.Expression).HasColumnName("expression");

        }
    }
}