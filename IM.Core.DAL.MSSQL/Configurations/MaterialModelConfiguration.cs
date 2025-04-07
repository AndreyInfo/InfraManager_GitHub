using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class MaterialModelConfiguration : MaterialModelConfigurationBase
{
    protected override string PrimaryKeyName => "PK_MaterialType";

    protected override string CartridgeTypeForeignKey => "FK_MaterialModel_CartridgeType";

    protected override string ManufacturerForeignKey => "FK_MaterialModel_Производители";

    protected override string ProductCatalogTypeForeignKey => "FK_MaterialModel_ProductCatalogType";

    protected override string UnitForeignKey => "FK_MaterialType_Unit";

    protected override string IndexCartridgeTypeID => "IX_MaterialModel_CartridgeTypeID";

    protected override string IndexProductCatalogTypeID => "IX_MaterialModel_ProductCatalogTypeID";

    protected override string IndexUnitID => "IX_MaterialModel_UnitID";

    protected override string IndexVendorID => "IX_MaterialModel_VendorID";

    protected override string IndexRemoved => "IX_MaterialModel_Removed";

    protected override void ConfigureDatabase(EntityTypeBuilder<MaterialModel> builder)
    {
        builder.ToTable("MaterialModel", Options.Scheme);

        builder.Property(x => x.IMObjID).HasColumnName("MaterialModelID");
        builder.Property(x => x.ManufacturerID).HasColumnName("VendorID");
        builder.Property(x => x.UnitID).HasColumnName("UnitID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.Gost).HasColumnName("GOST");
        builder.Property(x => x.CartridgeResource).HasColumnName("CartridgeResourse");
        builder.Property(x => x.CartrigeTypeID).HasColumnName("CartridgeTypeID");
        builder.Property(x => x.Cost).HasColumnName("Cost");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion");
        builder.Property(x => x.Removed).HasColumnName("Removed");
        builder.Property(x => x.ProductNumber).HasColumnName("ProductNumber");
        builder.Property(x => x.CanBuy).HasColumnName("CanBuy");
    }
}