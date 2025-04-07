using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class NetworkDeviceModelConfigurationBase : IEntityTypeConfiguration<NetworkDeviceModel>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string ProductCatalogTypeForeignKey { get; }
    protected abstract string ManufacturerForeignKey { get; }
    protected abstract string IMObjIDIndex { get; }
    protected abstract string ProductCatalogTypeIDIndex { get; }
    protected abstract string DefaultValueIMObjID { get; }

    public void Configure(EntityTypeBuilder<NetworkDeviceModel> entity)
    {
        entity.HasKey(e => e.ID).HasName(PrimaryKeyName);

        entity.HasIndex(e => e.IMObjID, IMObjIDIndex);
        entity.HasIndex(e => e.ProductCatalogTypeID, ProductCatalogTypeIDIndex);
        
        entity.Property(e => e.IMObjID).HasDefaultValueSql(DefaultValueIMObjID);

        entity.Property(e => e.Oid).HasMaxLength(510).IsRequired(false);
        entity.Property(e => e.Name).HasMaxLength(255).IsRequired(false);
        entity.Property(e => e.Note).HasMaxLength(510).IsRequired(false);
        entity.Property(e => e.Code).HasMaxLength(100).IsRequired(false);
        entity.Property(e => e.ProductNumber).HasMaxLength(500).IsRequired(false);
        entity.Property(e => e.ImageCyrillic).HasMaxLength(1000).IsRequired(false);
        entity.Property(e => e.ProductNumberCyrillic).HasMaxLength(100).IsRequired(false);
        entity.Property(e => e.ExternalID).HasMaxLength(500)
            .IsRequired(true)
            .HasDefaultValueSql("(N'Сетевое оборудование')");

        entity.HasOne(d => d.ProductCatalogType)
            .WithMany(p => p.NetworkDeviceModel)
            .HasForeignKey(d => d.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeForeignKey)
            .OnDelete(DeleteBehavior.ClientSetNull);

        entity.HasOne(d => d.Manufacturer)
            .WithMany(p => p.NetworkDeviceModel)
            .HasForeignKey(d => d.ManufacturerID)
            .HasConstraintName(ManufacturerForeignKey);

        ConfigureDatabase(entity);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<NetworkDeviceModel> entity);

}