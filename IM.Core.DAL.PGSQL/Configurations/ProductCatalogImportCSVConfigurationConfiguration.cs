using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal class
        ProductCatalogImportCSVConfigurationConfiguration : ProductCatalogImportCSVConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_product_catalog_import_csv_configuration";

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportCSVConfiguration> builder)
        {
            builder.ToTable("product_catalog_import_csv_configuration", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");

            builder.Property(x => x.Name).HasColumnName("name");

            builder.Property(x => x.Note).HasColumnName("note");

            builder.Property(x => x.Delimeter).HasColumnName("delimiter");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}