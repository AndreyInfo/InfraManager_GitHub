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
        protected override string PrimaryKeyName => "PK_ProductCatalogImportCSVConfigurationConcordance";

        protected override void ConfigureDatabase(
            EntityTypeBuilder<ProductCatalogImportCSVConfigurationConcordance> builder)
        {
            builder.ToTable("ProductCatalogImportCSVConfigurationConcordance", "dbo");

            builder.Property(x => x.ID).HasColumnName("ProductCatalogImportCSVConfigurationID");

            builder.Property(x => x.Field).HasColumnName("Field");

            builder.Property(x => x.Expression).HasColumnName("Expression");
        }
    }
}