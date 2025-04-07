using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class NetworkDeviceConfigurationBase : IEntityTypeConfiguration<NetworkDevice>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string NetworkDeviceModelForeignKeyName { get; }
    protected abstract string RackForeignKeyName { get; }
    protected abstract string SnmpTokenForeignKeyName { get; }
    protected abstract string CriticalityForeignKeyName { get; }
    protected abstract string InfrastructureSegmentForeignKeyName { get; }
    protected abstract string IMObjIDIndexName { get; }
    protected abstract string TypeIDIndexName { get; }
    protected abstract string RoomForeignKeyName { get; }
    protected abstract string RackIDIndexName { get; }
    protected abstract string RoomIDIndexName { get; }

    protected abstract string IDDefaultValue { get; }
    protected abstract string IMObjIDDefaultValue { get; }


    public void Configure(EntityTypeBuilder<NetworkDevice> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ID).HasDefaultValueSql(IDDefaultValue);
        builder.Property(x => x.Name).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.IpAddress).IsRequired(false).HasMaxLength(15);
        builder.Property(x => x.IpMask).IsRequired(false).HasMaxLength(15);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(1000);
        builder.Property(x => x.InvNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Cpu).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.ClockFrequency).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Bios).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Bus).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.VideoAdapter).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Keyboard).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Mouse).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.SerialPorts).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.ParallelPorts).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.OperatingSystem).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.DefaultPrinter).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.ExternalID).IsRequired(true).HasMaxLength(250).HasDefaultValueSql("''");
        builder.Property(x => x.FrameType).IsRequired(false).HasMaxLength(20);
        builder.Property(x => x.AssetTag).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.CpuSocket).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Monitor).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.MonitorResolution).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.OSVer).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.SerialNumber).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.BiosVer).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.CsModel).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.CsSize).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.CsFormFactor).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.MbModel).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.MbChipSet).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.MbSlots).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Code).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Connected).IsRequired(true).HasDefaultValueSql("0");
        builder.Property(x => x.CsVendorID).IsRequired(false).HasDefaultValueSql("0");
        builder.Property(x => x.MbVendorID).IsRequired(false).HasDefaultValueSql("0");
        builder.Property(x => x.RoomID).IsRequired(true).HasDefaultValueSql("0");
        builder.Property(x => x.IMObjID).IsRequired(true).HasDefaultValueSql(IMObjIDDefaultValue);
        builder.Property(x => x.LogicalLocation).IsRequired(true).HasMaxLength(250).HasDefaultValueSql("''");
        builder.Property(x => x.Description).IsRequired(true).HasMaxLength(250).HasDefaultValueSql("''");
        builder.Property(x => x.Identifier).IsRequired(true).HasMaxLength(50).HasDefaultValueSql("''");

        builder.IsMarkableForDelete();

        builder.HasOne(x => x.Model)
            .WithMany(x => x.NetworkDevice)
            .HasForeignKey(x => x.NetworkDeviceModelID)
            .HasConstraintName(NetworkDeviceModelForeignKeyName);

        builder.HasOne(x => x.Rack)
            .WithMany(x => x.NetworkDevice)
            .HasForeignKey(x => x.RackID)
            .HasConstraintName(RackForeignKeyName);

        builder.HasOne(x => x.InfrastructureSegment)
            .WithMany()
            .HasForeignKey(x => x.InfrastructureSegmentID)
            .HasConstraintName(InfrastructureSegmentForeignKeyName);

        builder.HasOne(x => x.Criticality)
            .WithMany()
            .HasForeignKey(x => x.CriticalityID)
            .HasConstraintName(CriticalityForeignKeyName);

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomID)
            .HasConstraintName(RoomForeignKeyName);

        // todo: FK есть в БД. Настроить навигационное свойство когда мигрирует сущность SnmpToken.
        // builder.HasOne<SnmpToken>(x => x.SnmpToken)
        //     .WithMany()
        //     .HasForeignKey(x => x.SnmpTokenId)
        //     .HasPrincipalKey(x => x.ID)
        //     .HasConstraintName(SnmpTokenForeignKeyName);

        builder.HasIndex(x => x.IMObjID).HasDatabaseName(IMObjIDIndexName);
        builder.HasIndex(x => x.NetworkDeviceModelID).HasDatabaseName(TypeIDIndexName);
        builder.HasIndex(x => x.RackID).HasDatabaseName(RackIDIndexName);
        builder.HasIndex(x => x.RoomID).HasDatabaseName(RoomIDIndexName);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<NetworkDevice> builder);
}