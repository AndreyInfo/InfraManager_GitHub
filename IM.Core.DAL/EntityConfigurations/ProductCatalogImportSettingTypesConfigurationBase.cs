using InfraManager.DAL.Asset;
using Inframanager.DAL.ProductCatalogue.Import;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inframanager.DAL.EntityConfigurations
{
    public abstract class
        ProductCatalogImportSettingTypesConfigurationBase :IEntityTypeConfiguration<ProductCatalogImportSettingTypes>
    {
        protected abstract string PrimaryKeyName { get; }

        public abstract string TableName { get; }
        
        public abstract string SchemaName { get; }
        
        protected abstract void ConfigureDatabase(EntityTypeBuilder<ProductCatalogImportSettingTypes> entity);

        public void Configure(EntityTypeBuilder<ProductCatalogImportSettingTypes> entity)
        {
            entity.HasKey(x => new {x.ProductCatalogImportSettingID, x.ProductCatalogTypeID}).HasName(PrimaryKeyName);

            entity.Property(x => x.ProductCatalogImportSettingID).IsRequired(true);

            entity.Property(x => x.ProductCatalogTypeID).IsRequired(true);


            entity.HasIndex(x => new {x.ProductCatalogImportSettingID, x.ProductCatalogTypeID});


            entity.HasOne(x => x.ProductCatalogImportSetting)
                .WithMany()
                .HasForeignKey(x => x.ProductCatalogImportSettingID);

            entity.HasOne(x => x.ProductCatalogType)
                .WithMany()
                .HasForeignKey(x => x.ProductCatalogTypeID);


            ConfigureDatabase(entity);

        }
    }
}