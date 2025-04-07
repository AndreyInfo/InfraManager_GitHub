using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class PeripheralTypeConfigurationBase: IEntityTypeConfiguration<PeripheralType>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string VendorForeignKeyName { get; }
        protected abstract string ProductCatalogTypeForeignKeyName { get; }
        protected abstract string ProductCatalogTypeIDIndexName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<PeripheralType> entity);

        public void Configure(EntityTypeBuilder<PeripheralType> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name).HasMaxLength(255).IsRequired(true);
            builder.Property(x => x.Parameters).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.Note).HasMaxLength(255).IsRequired(false);
            builder.Property(x => x.ProductNumber).HasMaxLength(250).IsRequired(false);
            builder.Property(x => x.Code).HasMaxLength(50).IsRequired(false);
            builder.Property(x => x.ExternalID).HasMaxLength(250)
                .HasDefaultValue("'Периферийное устройство'")
                .IsRequired(true);

            builder.IsMarkableForDelete();

            builder.HasOne(x => x.Vendor)
                .WithMany(x => x.PeripheralType)
                .HasForeignKey(x => x.ManufacturerID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(VendorForeignKeyName);

            builder.HasOne(x => x.ProductCatalogType)
                .WithMany(x => x.PeripheralType)
                .HasForeignKey(x => x.ProductCatalogTypeID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(ProductCatalogTypeForeignKeyName);

            builder.HasIndex(x => x.ProductCatalogTypeID).HasDatabaseName(ProductCatalogTypeIDIndexName);

            ConfigureDatabase(builder);
        }
    }
}