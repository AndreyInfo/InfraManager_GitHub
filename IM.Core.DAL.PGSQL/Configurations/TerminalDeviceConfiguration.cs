using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    public class TerminalDeviceConfiguration : TerminalDeviceConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_terminal_equipment";
        protected override string TerminalDeviceModelForeignKeyName => "fk_terminal_device_terminal_device_model";
        protected override string WorkplaceIDColumnName => "workplace_id";
        protected override string WorkplaceForeignKeyName => "fk_terminal_equipment_workplace";
        protected override string SnmpTokenForeignKeyName => "fk_terminal_device_snmp_token";
        protected override string RoomForeignKeyName => "fk_terminal_equipment_room";
        protected override string IMObjIDIndexName => "ix_terminal_device_im_obj_id";
        protected override string TerminalDeviceModelIDIndexName => "ix_terminal_device_model_id";
        protected override string WorkplaceIDIndexName => "ix_terminal_device_workplace_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<TerminalDevice> builder)
        {
            builder.ToTable("terminal_equipment", Options.Scheme);
            
            builder.Property(x => x.ID).HasColumnName("identificator");
            builder.Property(x => x.TypeID).HasColumnName("terminal_equipment_type_id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.UserID).HasColumnName("user_id");
            builder.Property(x => x.IpAddress).HasColumnName("ip_address");
            builder.Property(x => x.IpMask).HasColumnName("ip_mask");
            builder.Property(x => x.RoomID).HasColumnName("room_id");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.MacAddress).HasColumnName("mac_address");
            builder.Property(x => x.Connected).HasColumnName("connected").HasDefaultValueSql("0");
            builder.Property(x => x.Connection1).HasColumnName("connection1").HasDefaultValueSql("''");
            builder.Property(x => x.VisioID).HasColumnName("visio_id");
            builder.Property(x => x.Removed).HasColumnName("removed").HasDefaultValueSql("false");
            builder.Property(x => x.DateRemoved).HasColumnName("date_removed").HasColumnType("timestamp(0)");
            builder.Property(x => x.InvNumber).HasColumnName("inventory_number");
            builder.Property(x => x.Cpu).HasColumnName("processor");
            builder.Property(x => x.ClockFrequency).HasColumnName("frequency");
            builder.Property(x => x.Bios).HasColumnName("bios");
            builder.Property(x => x.Bus).HasColumnName("bus");
            builder.Property(x => x.Memory).HasColumnName("memory");
            builder.Property(x => x.VideoAdapter).HasColumnName("videocard");
            builder.Property(x => x.Keyboard).HasColumnName("keyboard");
            builder.Property(x => x.Mouse).HasColumnName("mouse");
            builder.Property(x => x.SerialPorts).HasColumnName("serial_ports");
            builder.Property(x => x.ParallelPorts).HasColumnName("parallel_ports");
            builder.Property(x => x.OperatingSystem).HasColumnName("operation_system");
            builder.Property(x => x.Osver).HasColumnName("os_ver");
            builder.Property(x => x.DefaultPrinter).HasColumnName("default_printer");
            builder.Property(x => x.ExternalID).HasColumnName("external_id").HasDefaultValueSql("''");
            builder.Property(x => x.FrameType).HasColumnName("frame_type");
            builder.Property(x => x.AssetTag).HasColumnName("asset_tag");
            builder.Property(x => x.Cpusocket).HasColumnName("cpu_socket");
            builder.Property(x => x.Usbexist).HasColumnName("usb_exist");
            builder.Property(x => x.Monitor).HasColumnName("monitor");
            builder.Property(x => x.MonitorResolution).HasColumnName("monitor_resolution");
            builder.Property(x => x.Biosver).HasColumnName("bios_ver");
            builder.Property(x => x.SerialNumber).HasColumnName("serial_number");
            builder.Property(x => x.Biosver).HasColumnName("bios_ver");
            builder.Property(x => x.VideoMemory).HasColumnName("video_memory");
            builder.Property(x => x.Connection).HasColumnName("connection").HasDefaultValueSql("''");
            builder.Property(x => x.CsVendorID).HasColumnName("cs_vendor_id").HasDefaultValueSql("0");
            builder.Property(x => x.CsModel).HasColumnName("cs_model");
            builder.Property(x => x.CsSize).HasColumnName("cs_size");
            builder.Property(x => x.CsFormFactor).HasColumnName("cs_form_factor");
            builder.Property(x => x.MbVendorID).HasColumnName("mb_vendor_id").HasDefaultValueSql("0");
            builder.Property(x => x.MbModel).HasColumnName("mb_model");
            builder.Property(x => x.MbChipSet).HasColumnName("mb_chip_set");
            builder.Property(x => x.MbSlots).HasColumnName("mb_slots");
            builder.Property(x => x.ConnectorID).HasColumnName("connector_id");
            builder.Property(x => x.TechnologyID).HasColumnName("technology_id");
            builder.Property(x => x.Code).HasColumnName("code");
            builder.Property(x => x.ConnectedPortID).HasColumnName("connected_port_id");
            builder.Property(x => x.IMObjID).HasColumnName("im_obj_id").HasDefaultValueSql("(gen_random_uuid())");
            builder.Property(x => x.PowerConsumption).HasColumnName("power_consumption").HasColumnType("numeric(10, 2)");
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(x => x.ComplementaryGuidID).HasColumnName("complementary_guid_id");
            builder.Property(x => x.LogicalLocation).HasColumnName("logical_location").IsUnicode(false);
            builder.Property(x => x.Description).HasColumnName("description").IsUnicode(false);
            builder.Property(x => x.Identifier).HasColumnName("identifier").IsUnicode(false);
            builder.Property(x => x.SnmpTokenID).HasColumnName("snmp_token_id");
            builder.Property(x => x.CpucoreNumber).HasColumnName("cpu_core_number");
            builder.Property(x => x.Cpunumber).HasColumnName("cpu_number");
            builder.Property(x => x.Cpumodel).HasColumnName("cpu_model");
            builder.Property(x => x.DiskType).HasColumnName("disk_type");
            builder.Property(x => x.CpuautoInfo).HasColumnName("cpu_auto_info");
            builder.Property(x => x.DiskAutoInfo).HasColumnName("disk_auto_info");
            builder.Property(x => x.RamautoInfo).HasColumnName("ram_auto_info");
            builder.Property(x => x.DiskSpaceTotal).HasColumnName("disk_space_total").HasColumnType("real");
            builder.Property(x => x.Ramspace).HasColumnName("ram_space").HasColumnType("real");
            builder.Property(x => x.CpuclockFrequency).HasColumnName("cpu_clock_frequency").HasColumnType("real");
            builder.Property(x => x.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}
