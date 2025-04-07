using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class PeripheralConfigurationBase : IEntityTypeConfiguration<Peripheral>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string PeripheralTypeForeignKeyName { get; }
    protected abstract string TerminalDeviceForeignKeyName { get; }
    protected abstract string NetworkDeviceForeignKeyName { get; }
    protected abstract string RoomForeignKeyName { get; }
    protected abstract string IntIDIndexName { get; }
    protected abstract string NetworkDeviceIDIndexName { get; }
    protected abstract string RoomIDIndexName { get; }
    protected abstract string TerminalDeviceIDIndexName { get; }
    protected abstract string PeripheralTypeIDIndexName { get; }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<Peripheral> builder);

    public void Configure(EntityTypeBuilder<Peripheral> builder)
    {
        builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
        builder.Property(x => x.SerialNumber).IsRequired(false).HasMaxLength(255);
        builder.Property(x => x.Note).IsRequired(false).HasMaxLength(1000);
        builder.Property(x => x.RowVersion).IsRequired(true);
        builder.Property(x => x.Code).IsRequired(true).HasMaxLength(50);
        builder.Property(x => x.ExternalID).IsRequired(true).HasMaxLength(250);

        builder.HasOne(x => x.Model)
            .WithMany()
            .HasForeignKey(x => x.PeripheralTypeID)
            .HasPrincipalKey(x => x.IMObjID)
            .HasConstraintName(PeripheralTypeForeignKeyName);

        builder.HasOne(x => x.TerminalDevice)
            .WithMany()
            .HasForeignKey(x => x.TerminalDeviceID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(TerminalDeviceForeignKeyName);

        builder.HasOne(x => x.NetworkDevice)
            .WithMany()
            .HasForeignKey(x => x.NetworkDeviceID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(NetworkDeviceForeignKeyName);

        builder.HasOne(x => x.Room)
            .WithMany()
            .HasForeignKey(x => x.RoomID)
            .HasPrincipalKey(x => x.ID)
            .IsRequired(false)
            .HasConstraintName(RoomForeignKeyName);

        builder.HasIndex(x => x.ID).HasDatabaseName(IntIDIndexName);
        builder.HasIndex(x => x.NetworkDeviceID).HasDatabaseName(NetworkDeviceIDIndexName);
        builder.HasIndex(x => x.RoomID).HasDatabaseName(RoomIDIndexName);
        builder.HasIndex(x => x.TerminalDeviceID).HasDatabaseName(TerminalDeviceIDIndexName);
        builder.HasIndex(x => x.PeripheralTypeID).HasDatabaseName(PeripheralTypeIDIndexName);
        
        ConfigureDatabase(builder);
    }
}