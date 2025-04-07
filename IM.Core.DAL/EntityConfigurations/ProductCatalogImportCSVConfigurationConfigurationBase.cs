using InfraManager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class
        ProductCatalogImportCSVConfigurationConfigurationBase : IEntityTypeConfiguration<
            ProductCatalogImportCSVConfiguration>
    {
        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportCSVConfiguration> entity);

        public void Configure(EntityTypeBuilder<ProductCatalogImportCSVConfiguration> entity)
        {
            entity.HasKey(x => x.ID).HasName(PrimaryKeyName);

            entity.Property(x => x.Name).IsRequired(true).HasMaxLength(250);

            entity.Property(x => x.Note).IsRequired(true);

            entity.Property(x => x.Delimeter).IsRequired(true);

            entity.HasOne(x => x.ProductCatalogImportSetting)
                .WithMany()
                .HasForeignKey(x => x.ID)
                .HasPrincipalKey(x => x.ProductCatalogImportCSVConfigurationID);
            
            ConfigureDatabase(entity);
        }
    }
}