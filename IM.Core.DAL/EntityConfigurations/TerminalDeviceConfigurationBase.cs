using InfraManager.DAL.Asset;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class TerminalDeviceConfigurationBase : IEntityTypeConfiguration<TerminalDevice>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string TerminalDeviceModelForeignKeyName { get; }
    protected abstract string WorkplaceForeignKeyName { get; }
    protected abstract string SnmpTokenForeignKeyName { get; }
    protected abstract string RoomForeignKeyName { get; }
    protected abstract string IMObjIDIndexName { get; }
    protected abstract string TerminalDeviceModelIDIndexName { get; }
    protected abstract string WorkplaceIDIndexName { get; }
    protected abstract string WorkplaceIDColumnName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<TerminalDevice> builder);

    public void Configure(EntityTypeBuilder<TerminalDevice> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.ID).ValueGeneratedNever();
        builder.Property(x => x.Name).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.IpAddress).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.IpMask).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(1000);
        builder.Property(x => x.MacAddress).IsRequired(false).HasMaxLength(23);
        builder.Property(x => x.Connection1).IsRequired(false).HasMaxLength(400);
        builder.Property(x => x.InvNumber).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Cpu).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ClockFrequency).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.Bios).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Bus).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.VideoAdapter).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Keyboard).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Mouse).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.SerialPorts).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ParallelPorts).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.OperatingSystem).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.DefaultPrinter).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.ExternalID).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.FrameType).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.AssetTag).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Cpusocket).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Monitor).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MonitorResolution).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Osver).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.SerialNumber).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Biosver).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Connection).IsRequired(false).HasMaxLength(400);
        builder.Property(x => x.CsModel).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.CsSize).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.CsFormFactor).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MbModel).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MbChipSet).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.MbSlots).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Code).IsRequired(false).HasMaxLength(50);
        builder.Property(x => x.LogicalLocation).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.Description).IsRequired(false).HasMaxLength(250);
        builder.Property(x => x.Identifier).IsRequired(false).HasMaxLength(50);

        builder.IsMarkableForDelete();

        builder.HasOne(x => x.Model)
            .WithMany(x => x.TerminalDevice)
            .HasForeignKey(x => x.TypeID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(TerminalDeviceModelForeignKeyName);

        const string workplaceID = "WorkplaceID";
        builder.Property<int?>(workplaceID).HasColumnName(WorkplaceIDColumnName);
        builder.HasOne(x => x.Workplace)
            .WithMany()
            .HasForeignKey(workplaceID)
            .HasPrincipalKey(x => x.ID)
            .HasConstraintName(WorkplaceForeignKeyName);

        builder.HasOne<Room>(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(RoomForeignKeyName);

        // todo: FK есть в БД. Настроить навигационное свойство когда мигрирует сущность SnmpToken
        // builder.HasOne<SnmpToken>(x => x.SnmpToken).WithMany().HasForeignKey(x => x.SnmpTokenId).HasPrincipalKey(x => x.ID).HasConstraintName(SnmpTokenForeignKeyName);

        builder.HasIndex(x => x.IMObjID).HasDatabaseName(IMObjIDIndexName);
        builder.HasIndex(x => x.TypeID).HasDatabaseName(TerminalDeviceModelIDIndexName);
        builder.HasIndex(workplaceID).HasDatabaseName(WorkplaceIDIndexName);
        
        ConfigureDatabase(builder);
    }
}