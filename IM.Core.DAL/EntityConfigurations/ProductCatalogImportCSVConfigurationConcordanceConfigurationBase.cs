using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class
        ProductCatalogImportCSVConfigurationConcordanceConfigurationBase : IEntityTypeConfiguration<
            ProductCatalogImportCSVConfigurationConcordance>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(
            EntityTypeBuilder<ProductCatalogImportCSVConfigurationConcordance> entity);

        public void Configure(EntityTypeBuilder<ProductCatalogImportCSVConfigurationConcordance> entity)
        {
            entity.HasKey(x => new {x.ID, x.Field}).HasName(PrimaryKeyName);

            entity.Property(x => x.ID).IsRequired(true);

            entity.Property(x => x.Field).IsRequired(true).HasMaxLength(50);

            entity.Property(x => x.Expression).IsRequired(true);

            entity.HasOne(x => x.ProductCatalogImportCSVConfiguration)
                .WithMany()
                .HasForeignKey(x => x.ID);


            ConfigureDatabase(entity);
        }
    }
}