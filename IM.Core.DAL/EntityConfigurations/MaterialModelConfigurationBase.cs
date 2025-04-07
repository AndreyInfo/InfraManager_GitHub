using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class MaterialModelConfigurationBase: IEntityTypeConfiguration<MaterialModel>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string CartridgeTypeForeignKey { get; }
    protected abstract string ManufacturerForeignKey { get; }
    protected abstract string ProductCatalogTypeForeignKey { get; }
    protected abstract string UnitForeignKey { get; }
    protected abstract string IndexCartridgeTypeID { get; }
    protected abstract string IndexProductCatalogTypeID { get; }
    protected abstract string IndexUnitID { get; }
    protected abstract string IndexVendorID { get; }
    protected abstract string IndexRemoved { get; }

    public void Configure(EntityTypeBuilder<MaterialModel> builder)
    {
        builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);


        builder.Property(x => x.Name).HasMaxLength(255).IsRequired(false);
        builder.Property(x => x.Note).HasMaxLength(255).IsRequired(false);
        builder.Property(x => x.ExternalID).HasMaxLength(250).IsRequired(true)
            .HasDefaultValue("Расходный материал");
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired(false);
        builder.Property(x => x.Gost).HasMaxLength(50).HasDefaultValue("");
        builder.Property(x => x.ProductNumber).HasMaxLength(250).IsRequired(false);
        builder.Property(x => x.ProductNumber).HasMaxLength(250).IsRequired(true);



        builder.HasIndex(x => x.ProductCatalogTypeID, IndexProductCatalogTypeID);
        builder.HasIndex(x => x.ManufacturerID, IndexVendorID);
        builder.HasIndex(x => x.UnitID, IndexUnitID);
        builder.HasIndex(x => x.CartrigeTypeID, IndexCartridgeTypeID);
        builder.HasIndex(x => x.Removed, IndexRemoved);


        builder.HasOne(x => x.ProductCatalogType)
            .WithMany()
            .HasForeignKey(x => x.ProductCatalogTypeID)
            .HasConstraintName(ProductCatalogTypeForeignKey);

        builder.HasOne(x => x.Manufacturer)
            .WithMany()
            .HasForeignKey(x => x.ManufacturerID)
            .HasConstraintName(ManufacturerForeignKey);

        builder.HasOne(x => x.CartridgeType)
            .WithMany()
            .HasForeignKey(x => x.CartrigeTypeID)
            .HasConstraintName(CartridgeTypeForeignKey);

        builder.HasOne(x => x.Unit)
            .WithMany()
            .HasForeignKey(x => x.UnitID)
            .HasConstraintName(UnitForeignKey);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<MaterialModel> builder);

}