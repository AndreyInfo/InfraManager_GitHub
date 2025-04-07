using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations
{
    internal sealed class TerminalDeviceModelConfiguration : TerminalDeviceModelConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_terminal_equipment_types";
        protected override string VendorForeignKeyName => "fk_terminal_equipment_type_manufacturers";
        protected override string ProductCatalogTypeForeignKeyName => "fk_terminal_device_model_product_catalog_type";
        protected override string HypervisorModelForeignKeyName => "fk_terminal_device_type_hypervisor_model";
        protected override string IMObjIDIndexName => "ix_terminal_device_model_im_obj_id";
        protected override string ProductCatalogTypeIDIndexName => "ix_terminal_device_model_product_catalog_type_id";
        protected override string DefaultValueIMObjID => "(gen_random_uuid())";
        protected override string DefaultValueID => "nextval('actives_request_specification_sequence')";
        protected override string ConnectorTypeForeignKeyName => "fk_terminal_equipment_types_connector_kinds";
        protected override string TechnologyTypeForeignKeyName => "fk_terminal_equipment_types_technology_kinds";

        protected override void ConfigureDatabase(EntityTypeBuilder<TerminalDeviceModel> builder)
        {
            builder.ToTable("terminal_equipment_types", Options.Scheme);

            builder.Property(e => e.ID).HasColumnName("identificator");
            builder.Property(e => e.Name).HasColumnName("name");
            builder.Property(e => e.ManufacturerID).HasColumnName("manufacturer_id");
            builder.Property(e => e.ConnectorTypeID).HasColumnName("connector_id");
            builder.Property(e => e.TechnologyTypeID).HasColumnName("tchnology_id");
            builder.Property(e => e.ImageCyrillic).HasColumnName("image");
            builder.Property(e => e.ProductNumberCyrillic).HasColumnName("cyr_product_number");
            builder.Property(e => e.ProductNumber).HasColumnName("product_number");
            builder.Property(e => e.ExternalID).HasColumnName("external_id").HasDefaultValueSql("'Оконечное оборудование'");
            builder.Property(e => e.Code).HasColumnName("code");
            builder.Property(e => e.Note).HasColumnName("note");
            builder.Property(e => e.Removed).HasColumnName("removed").HasDefaultValueSql("false");
            builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
            builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(e => e.HypervisorModelID).HasColumnName("hypervisor_model_id");
            builder.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
            builder.Property(e => e.CanBuy).HasColumnName("can_buy");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}