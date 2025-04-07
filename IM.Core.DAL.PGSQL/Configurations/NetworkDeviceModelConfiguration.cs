using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class NetworkDeviceModelConfiguration : NetworkDeviceModelConfigurationBase
{
    protected override string PrimaryKeyName => "pk_active_equipment_types";
    protected override string ProductCatalogTypeForeignKey => "fk_network_device_model_product_catalog_type";
    protected override string ManufacturerForeignKey => "fk_active_equipment_type_manufacturers";
    protected override string IMObjIDIndex => "ix_network_device_model_im_obj_id";
    protected override string ProductCatalogTypeIDIndex => "ix_network_device_model_product_catalog_type_id";
    protected override string DefaultValueIMObjID => "gen_random_uuid()";

    protected override void ConfigureDatabase(EntityTypeBuilder<NetworkDeviceModel> entity)
    {
        entity.ToTable("active_equipment_types", Options.Scheme);

        entity.Property(e => e.ID).HasColumnName("identificator")
            .HasDefaultValueSql("nextval('actives_request_specification_sequence')");

        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.ManufacturerID).HasColumnName("manufacturer_id");
        entity.Property(e => e.PortCount).HasColumnName("port_count");
        entity.Property(e => e.ImageCyrillic).HasColumnName("image");
        entity.Property(e => e.Width).HasColumnName("width").HasColumnType("numeric(9, 2)");
        entity.Property(e => e.Height).HasColumnName("height").HasColumnType("numeric(9, 2)");
        entity.Property(e => e.HeightInUnits).HasColumnType("numeric(9, 2)").HasColumnName("height_size");
        entity.Property(e => e.ProductNumberCyrillic).HasColumnName("cyr_product_number");
        entity.Property(e => e.Oid).HasColumnName("o_id");
        entity.Property(e => e.SlotCount).HasColumnName("slot_count");
        entity.Property(e => e.ProductNumber).HasColumnName("product_number");
        entity.Property(e => e.ExternalID).HasColumnName("external_id");
        entity.Property(e => e.Code).HasColumnName("code");
        entity.Property(e => e.Note).HasColumnName("note");
        entity.Property(e => e.Removed).HasColumnName("removed");
        entity.Property(e => e.IMObjID).HasColumnName("im_obj_id").HasDefaultValueSql("(gen_random_uuid())");
        entity.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        entity.Property(e => e.Depth).HasColumnName("depth").HasColumnType("numeric(9, 2)");
        entity.Property(e => e.IsRackmount).HasColumnName("is_rackmount").IsRequired();
        entity.Property(e => e.HypervisorModelID).HasColumnName("hypervisor_model_id");
        entity.Property(e => e.MaxLoad).HasColumnName("max_load");
        entity.Property(e => e.NominalLoad).HasColumnName("nominal_load");
        entity.Property(e => e.ColorPrint).HasColumnName("color_print");
        entity.Property(e => e.PhotoPrint).HasColumnName("photo_print");
        entity.Property(e => e.DuplexMode).HasColumnName("duplex_mode");
        entity.Property(e => e.PrintTechnology).HasColumnName("print_technology");
        entity.Property(e => e.MaxPrintFormat).HasColumnName("max_print_format");
        entity.Property(e => e.PrintSpeedUnit).HasColumnName("print_speed_unit");
        entity.Property(e => e.RollNumber).HasColumnName("roll_number");
        entity.Property(e => e.Speed).HasColumnName("speed").HasColumnType("numeric(10, 2)");
        entity.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        entity.Property(e => e.CanBuy).HasColumnName("can_buy");

        entity.HasXminRowVersion(x => x.RowVersion);
    }
}