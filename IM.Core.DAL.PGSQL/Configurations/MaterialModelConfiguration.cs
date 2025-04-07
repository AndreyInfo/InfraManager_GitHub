using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaterialModelConfiguration : MaterialModelConfigurationBase
{
    protected override string PrimaryKeyName => "pk_material_type";

    protected override string CartridgeTypeForeignKey => "pk_material_model_cartridge_type";

    protected override string ManufacturerForeignKey => "pk_material_model_manufacturers";

    protected override string ProductCatalogTypeForeignKey => "pk_material_model_product_catalog_type";

    protected override string UnitForeignKey => "pk_material_model_unit";

    protected override string IndexCartridgeTypeID => "ix_material_model_cartridge_type_id";

    protected override string IndexProductCatalogTypeID => "ix_material_model_product_catalog_type_id";

    protected override string IndexUnitID => "ix_material_model_unit_id";

    protected override string IndexVendorID => "ix_material_model_vendor_id";

    protected override string IndexRemoved => "ix_material_model_removed";

    protected override void ConfigureDatabase(EntityTypeBuilder<MaterialModel> builder)
    {
        builder.ToTable("material_model", Options.Scheme);

        builder.Property(x => x.IMObjID).HasColumnName("material_model_id");
        builder.Property(x => x.ManufacturerID).HasColumnName("vendor_id");
        builder.Property(x => x.UnitID).HasColumnName("unit_id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.Gost).HasColumnName("gost");
        builder.Property(x => x.CartrigeTypeID).HasColumnName("cartridge_type_id");
        builder.Property(x => x.CartridgeResource).HasColumnName("cartridge_resourse");
        builder.Property(x => x.Cost).HasColumnName("cost");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.HasXminRowVersion(x => x.RowVersion);
        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.Property(x => x.ProductNumber).HasColumnName("product_number");
        builder.Property(x => x.CanBuy).HasColumnName("can_buy");
    }
}