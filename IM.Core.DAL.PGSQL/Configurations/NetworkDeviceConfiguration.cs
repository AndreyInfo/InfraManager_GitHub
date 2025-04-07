using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class NetworkDeviceConfiguration : NetworkDeviceConfigurationBase
{
    protected override string PrimaryKeyName => "pk_active_equipment";
    protected override string NetworkDeviceModelForeignKeyName => "fk_network_device_network_device_model";
    protected override string RackForeignKeyName => "fk_active_equipment_cabinet";
    protected override string SnmpTokenForeignKeyName => "fk_network_device_snmp_token";
    protected override string InfrastructureSegmentForeignKeyName => "fk_ad_infrastructure_segment";
    protected override string CriticalityForeignKeyName => "fk_ad_criticality";
    protected override string IMObjIDIndexName => "ix_network_device_im_obj_id";
    protected override string TypeIDIndexName => "ix_network_device_model_id";
    protected override string RackIDIndexName => "ix_network_device_rack_id";
    protected override string RoomIDIndexName => "ix_network_device_room_id";
    protected override string RoomForeignKeyName => "fk_active_equipment_room";
    protected override string IDDefaultValue => "nextval('pk_active_equipment_id_seq')";
    protected override string IMObjIDDefaultValue => "gen_random_uuid()";

    protected override void ConfigureDatabase(EntityTypeBuilder<NetworkDevice> builder)
    {
        builder.ToTable("active_equipment", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("identificator");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.NetworkDeviceModelID).HasColumnName("type_id");
        builder.Property(x => x.RackID).HasColumnName("cabinet_id");
        builder.Property(x => x.IpAddress).HasColumnName("ip_address");
        builder.Property(x => x.IpMask).HasColumnName("ip_mask");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.Connected).HasColumnName("connected");
        builder.Property(x => x.VisioID).HasColumnName("visio_id");
        builder.Property(x => x.Removed).HasColumnName("removed");
        builder.Property(x => x.DateRemoved).HasColumnName("date_removed");
        builder.Property(x => x.InvNumber).HasColumnName("inventory_number");
        builder.Property(x => x.Cpu).HasColumnName("processor");
        builder.Property(x => x.ClockFrequency).HasColumnName("frequency");
        builder.Property(x => x.Bios).HasColumnName("bios");
        builder.Property(x => x.Bus).HasColumnName("bus");
        builder.Property(x => x.MemorySize).HasColumnName("cyr_memory");
        builder.Property(x => x.VideoAdapter).HasColumnName("videocard");
        builder.Property(x => x.Keyboard).HasColumnName("keyboard");
        builder.Property(x => x.Mouse).HasColumnName("mouse");
        builder.Property(x => x.SerialPorts).HasColumnName("serial_ports");
        builder.Property(x => x.ParallelPorts).HasColumnName("parallel_ports");
        builder.Property(x => x.OperatingSystem).HasColumnName("operation_system");
        builder.Property(x => x.DefaultPrinter).HasColumnName("default_printer");
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
        builder.Property(x => x.FrameType).HasColumnName("frame_type");
        builder.Property(x => x.AssetTag).HasColumnName("asset_tag");
        builder.Property(x => x.CpuSocket).HasColumnName("cpu_socket");
        builder.Property(x => x.UsbExist).HasColumnName("usb_exist");
        builder.Property(x => x.Monitor).HasColumnName("monitor");
        builder.Property(x => x.MonitorResolution).HasColumnName("monitor_resolution");
        builder.Property(x => x.OSVer).HasColumnName("os_ver");
        builder.Property(x => x.SerialNumber).HasColumnName("serial_number");
        builder.Property(x => x.BiosVer).HasColumnName("bios_ver");
        builder.Property(x => x.VideoMemory).HasColumnName("video_memory");
        builder.Property(x => x.CsVendorID).HasColumnName("cs_vendor_id");
        builder.Property(x => x.CsModel).HasColumnName("cs_model");
        builder.Property(x => x.CsSize).HasColumnName("cs_size");
        builder.Property(x => x.CsFormFactor).HasColumnName("cs_form_factor");
        builder.Property(x => x.MbVendorID).HasColumnName("mb_vendor_id");
        builder.Property(x => x.MbModel).HasColumnName("mb_model");
        builder.Property(x => x.MbChipSet).HasColumnName("mb_chip_set");
        builder.Property(x => x.MbSlots).HasColumnName("mb_slots");
        builder.Property(x => x.RoomID).HasColumnName("room_id");
        builder.Property(x => x.Code).HasColumnName("code");
        builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
        builder.Property(x => x.PowerConsumption).HasColumnName("power_consumption").HasColumnType("numeric(10, 2)");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.ComplementaryGuidID).HasColumnName("complementary_guid_id");
        builder.Property(x => x.LogicalLocation).HasColumnName("logical_location");
        builder.Property(x => x.Description).HasColumnName("description");
        builder.Property(x => x.Identifier).HasColumnName("identifier");
        builder.Property(x => x.SnmpTokenID).HasColumnName("snmp_token_id");
        builder.Property(x => x.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");
        builder.Property(x => x.CriticalityID).HasColumnName("criticality_id");
        builder.Property(x => x.Memory).HasColumnName("memory").HasColumnType("real");
        builder.Property(x => x.DiskSpace).HasColumnName("disk_space").HasColumnType("real");
        builder.Property(x => x.OrganizationItemID).HasColumnName("organization_item_id");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("organization_item_class_id");
        builder.Property(x => x.CpuCoreNumber).HasColumnName("cpu_core_number");
        builder.Property(x => x.CpuNumber).HasColumnName("cpu_number");
        builder.Property(x => x.CpuModelID).HasColumnName("cpu_model");
        builder.Property(x => x.DiskTypeID).HasColumnName("disk_type");
        builder.Property(x => x.CpuAutoInfo).HasColumnName("cpu_auto_info");
        builder.Property(x => x.DiskAutoInfo).HasColumnName("disk_auto_info");
        builder.Property(x => x.RamAutoInfo).HasColumnName("ram_auto_info");
        builder.Property(x => x.DiskSpaceTotal).HasColumnName("disk_space_total").HasColumnType("real");
        builder.Property(x => x.RamSpace).HasColumnName("ram_space").HasColumnType("real");
        builder.Property(x => x.CpuClockFrequency).HasColumnName("cpu_clock_frequency").HasColumnType("real");

        builder.HasXminRowVersion(x => x.RowVersion);
    }
}
