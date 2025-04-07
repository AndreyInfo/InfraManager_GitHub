using Inframanager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class
        ProductCatalogImportCSVConfigurationConfiguration : ProductCatalogImportCSVConfigurationConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_ProductCatalogImportCSVConfiguration";

        protected override void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportCSVConfiguration> builder)
        {
            builder.ToTable("ProductCatalogImportCSVConfiguration", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");

            builder.Property(x => x.Name).HasColumnName("Name");

            builder.Property(x => x.Note).HasColumnName("Note");

            builder.Property(x => x.Delimeter).HasColumnName("Delimeter");

            builder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken().HasColumnName("RowVersion");
        }
    }
}