using IM.Core.DAL.Postgres;
using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class SoftwareLicenceConfiguration : SoftwareLicenceConfigurationBase
{
    protected override string PrimaryKeyName => "pk_software_licence";

    protected override string ParentForeignKey => "fk_software_licence_software_licence";
    protected override string ProductCatalogTypeForeignKey => "fk_software_licence_product_catalo_type";
    protected override string SoftwareModelForeignKey => "fk_software_licence_software_licence_model";
    
    protected override string IndexByRoomIntID => "ix_software_licence_room_int_id";
    protected override string IndexBySoftwareModelID => "ix_software_licence_software_model_id";
    protected override string IndexByProductCatalogTypeID => "ix_software_licence_product_catalog_type_id";
    protected override string IndexBySoftwareLicenceModelID => "ix_software_licence_software_licence_model_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareLicence> builder)
    {
        builder.ToTable("software_licence", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.IsFull).HasColumnName("is_full");
        builder.Property(e => e.ParentID).HasColumnName("parent_id");
        builder.Property(e => e.RoomIntID).HasColumnName("room_int_id");
        builder.Property(e => e.OemdeviceID).HasColumnName("oem_device_id");
        builder.Property(e => e.LimitInHours).HasColumnName("limit_in_hours");
        builder.Property(e => e.HaspadapterID).HasColumnName("hasp_adapter_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.InventoryNumber).HasColumnName("inventory_number");
        builder.Property(e => e.SoftwareModelID).HasColumnName("software_model_id");
        builder.Property(e => e.OemdeviceClassID).HasColumnName("oem_device_class_id");
        builder.Property(e => e.DowngradeAvailable).HasColumnName("downgrade_available");
        builder.Property(e => e.RestrictionsHzFrom).HasColumnName("restrictions_hz_from");
        builder.Property(e => e.RestrictionsHzTill).HasColumnName("restrictions_hz_till");
        builder.Property(e => e.SoftwareLicenceType).HasColumnName("software_licence_type");
        builder.Property(e => e.RestrictionsCpuFrom).HasColumnName("restrictions_cpu_from");
        builder.Property(e => e.RestrictionsCpuTill).HasColumnName("restrictions_cpu_till");
        builder.Property(e => e.RestrictionsCoreFrom).HasColumnName("restrictions_core_from");
        builder.Property(e => e.RestrictionsCoreTill).HasColumnName("restrictions_core_till");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(e => e.SoftwareLicenceScheme).HasColumnName("software_licence_scheme");
        builder.Property(e => e.SoftwareExecutionCount).HasColumnName("software_execution_count");
        builder.Property(e => e.SoftwareLicenceModelID).HasColumnName("software_licence_model_id");
        builder.Property(e => e.SoftwareLicenceSchemeEnum).HasColumnName("software_licence_scheme_enum");

        builder.Property(e => e.BeginDate).HasColumnType("timestamp without time zone").HasColumnName("begin_date");
        builder.Property(e => e.EndDate).HasColumnType("timestamp without time zone").HasColumnName("end_date");

        builder.HasXminRowVersion(e => e.RowVersion);
    }
}