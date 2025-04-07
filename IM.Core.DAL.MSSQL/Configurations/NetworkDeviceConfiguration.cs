using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class NetworkDeviceConfiguration : NetworkDeviceConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Активное устройство";
    protected override string NetworkDeviceModelForeignKeyName => "FK_NetworkDevice_NetworkDeviceModel";
    protected override string RackForeignKeyName => "FK_Активное устройство_Шкаф";
    protected override string SnmpTokenForeignKeyName => "FK_NetworkDevice_SnmpToken";
    protected override string InfrastructureSegmentForeignKeyName => "fk_AD_InfrastructureSegment";
    protected override string CriticalityForeignKeyName => "fk_AD_Criticality";
    protected override string RoomForeignKeyName => "FK_Активное устройство_Комната";
    protected override string IMObjIDIndexName => "IX_NetworkDevice_IMObjID";
    protected override string TypeIDIndexName => "IX_NetworkDevice_ModelID";
    protected override string RoomIDIndexName => "IX_NetworkDevice_RoomID";
    protected override string RackIDIndexName => "IX_NetworkDevice_RackID";
    protected override string IDDefaultValue => "NEXT VALUE FOR [pk_active_equipment_types_seq]";
    protected override string IMObjIDDefaultValue => "newid()";

    protected override void ConfigureDatabase(EntityTypeBuilder<NetworkDevice> builder)
    {
        builder.ToTable("Активное устройство", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("Идентификатор");
        
        builder.Property(x => x.Name).HasColumnName("Название");
        builder.Property(x => x.NetworkDeviceModelID).HasColumnName("ИД типа");
        builder.Property(x => x.RackID).HasColumnName("ИД шкафа");
        builder.Property(x => x.IpAddress).HasColumnName("IP-address");
        builder.Property(x => x.IpMask).HasColumnName("IP-mask");
        builder.Property(x => x.Note).HasColumnName("Примечание");
        builder.Property(x => x.Connected).HasColumnName("Connected");
        builder.Property(x => x.VisioID).HasColumnName("Visio_ID");
        builder.Property(x => x.Removed).HasColumnName("Removed");
        builder.Property(x => x.DateRemoved).HasColumnName("DateRemoved").HasColumnType("smalldatetime");
        builder.Property(x => x.InvNumber).HasColumnName("Инвентарный номер");
        builder.Property(x => x.Cpu).HasColumnName("Процессор");
        builder.Property(x => x.ClockFrequency).HasColumnName("Такт частота");
        builder.Property(x => x.Bios).HasColumnName("BIOS");
        builder.Property(x => x.Bus).HasColumnName("Шина");
        builder.Property(x => x.MemorySize).HasColumnName("Память");
        builder.Property(x => x.VideoAdapter).HasColumnName("Видеоадаптер");
        builder.Property(x => x.Keyboard).HasColumnName("Клавиатура");
        builder.Property(x => x.Mouse).HasColumnName("Мышь");
        builder.Property(x => x.SerialPorts).HasColumnName("Последовательных портов");
        builder.Property(x => x.ParallelPorts).HasColumnName("Параллельных портов");
        builder.Property(x => x.OperatingSystem).HasColumnName("Операционная система");
        builder.Property(x => x.DefaultPrinter).HasColumnName("Принтер по умолчанию");
        builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.FrameType).HasColumnName("FrameType");
        builder.Property(x => x.AssetTag).HasColumnName("AssetTag");
        builder.Property(x => x.CpuSocket).HasColumnName("CPUSocket");
        builder.Property(x => x.UsbExist).HasColumnName("USBExist");
        builder.Property(x => x.Monitor).HasColumnName("Monitor");
        builder.Property(x => x.MonitorResolution).HasColumnName("MonitorResolution");
        builder.Property(x => x.OSVer).HasColumnName("OSVer");
        builder.Property(x => x.SerialNumber).HasColumnName("SerialNumber");
        builder.Property(x => x.BiosVer).HasColumnName("BIOSVer");
        builder.Property(x => x.VideoMemory).HasColumnName("VideoMemory");
        builder.Property(x => x.CsVendorID).HasColumnName("CsVendorID");
        builder.Property(x => x.CsModel).HasColumnName("CsModel");
        builder.Property(x => x.CsSize).HasColumnName("CsSize");
        builder.Property(x => x.CsFormFactor).HasColumnName("CsFormFactor");
        builder.Property(x => x.MbVendorID).HasColumnName("MbVendorID");
        builder.Property(x => x.MbModel).HasColumnName("MbModel");
        builder.Property(x => x.MbChipSet).HasColumnName("MbChipSet");
        builder.Property(x => x.MbSlots).HasColumnName("MbSlots");
        builder.Property(x => x.RoomID).HasColumnName("RoomID");
        builder.Property(x => x.Code).HasColumnName("Code");
        builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
        builder.Property(x => x.PowerConsumption).HasColumnName("PowerConsumption").HasColumnType("decimal(10, 2)");
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.LogicalLocation).HasColumnName("LogicalLocation");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.Identifier).HasColumnName("Identifier");
        builder.Property(x => x.SnmpTokenID).HasColumnName("SnmpTokenID");
        builder.Property(x => x.InfrastructureSegmentID).HasColumnName("InfrastructureSegmentID");
        builder.Property(x => x.CriticalityID).HasColumnName("CriticalityID");
        builder.Property(x => x.Memory).HasColumnName("Memory").HasColumnType("real");
        builder.Property(x => x.DiskSpace).HasColumnName("DiskSpace").HasColumnType("real");
        builder.Property(x => x.OrganizationItemID).HasColumnName("OrganizationItemID");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
        builder.Property(x => x.CpuCoreNumber).HasColumnName("CPUCoreNumber");
        builder.Property(x => x.CpuNumber).HasColumnName("CPUNumber");
        builder.Property(x => x.CpuModelID).HasColumnName("CPUModel");
        builder.Property(x => x.DiskTypeID).HasColumnName("DiskType");
        builder.Property(x => x.CpuAutoInfo).HasColumnName("CPUAutoInfo");
        builder.Property(x => x.DiskAutoInfo).HasColumnName("DiskAutoInfo");
        builder.Property(x => x.RamAutoInfo).HasColumnName("RAMAutoInfo");
        builder.Property(x => x.DiskSpaceTotal).HasColumnName("DiskSpaceTotal").HasColumnType("real");
        builder.Property(x => x.RamSpace).HasColumnName("RAMSpace").HasColumnType("real");
        builder.Property(x => x.CpuClockFrequency).HasColumnName("CPUClockFrequency").HasColumnType("real");

        builder.Property(x => x.RowVersion)
            .IsRequired()
            .HasColumnName("tstamp")
            .HasColumnType("timestamp")
            .IsRowVersion();
    }
}