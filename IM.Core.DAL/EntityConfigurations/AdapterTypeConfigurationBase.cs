using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class AdapterTypeConfigurationBase : IEntityTypeConfiguration<AdapterType>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string ProductCatalogTypeForeignKeyName { get; }
        protected abstract string VendorForeignKeyName { get; }
        protected abstract string SlotForeignKeyName { get; }
        protected abstract string ProductCatalogTypeIDIndexName { get; }


        public void Configure(EntityTypeBuilder<AdapterType> entity)
        {
            entity.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);
            
            entity.Property(e => e.IMObjID).ValueGeneratedNever();           
            entity.Property(e => e.Name).IsRequired(true).HasMaxLength(255);
            entity.Property(e => e.Parameters).IsRequired(false).HasMaxLength(255);
            entity.Property(e => e.Note).IsRequired(false).HasMaxLength(255);
            entity.Property(e => e.ProductNumber).IsRequired(false).HasMaxLength(250);
            entity.Property(e => e.ExternalID).IsRequired(true).HasMaxLength(250);
            entity.Property(e => e.Code).IsRequired(false).HasMaxLength(50);

            entity.IsMarkableForDelete();

            entity.HasOne(d => d.ProductCatalogType)
                .WithMany(p => p.AdapterTypes)
                .HasForeignKey(d => d.ProductCatalogTypeID)
                .HasPrincipalKey(x => x.IMObjID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName(ProductCatalogTypeForeignKeyName);

            entity.HasOne(d => d.Vendor)
                .WithMany(p => p.AdapterTypes)
                .HasForeignKey(d => d.ManufacturerID)
                .HasPrincipalKey(x => x.ID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName(VendorForeignKeyName);

            entity.HasOne(x => x.SlotType)
                .WithMany()
                .HasForeignKey(x => x.SlotTypeID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(SlotForeignKeyName);

            entity.HasIndex(x => x.ProductCatalogTypeID).HasDatabaseName(ProductCatalogTypeIDIndexName);

            ConfigureDatabase(entity);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<AdapterType> entity);


    }
}