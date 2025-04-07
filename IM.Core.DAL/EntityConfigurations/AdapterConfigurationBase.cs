using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class AdapterConfigurationBase : IEntityTypeConfiguration<Adapter>
    {
        protected abstract string PrimaryKeyName { get; }
        protected abstract string AdapterTypeForeignKeyName { get; }
        protected abstract string TerminalDeviceForeignKeyName { get; }
        protected abstract string NetworkDeviceForeignKeyName { get; }
        protected abstract string RoomForeignKeyName { get; }
        protected abstract string SlotTypeForeignKeyName { get; }
        protected abstract string AdapterTypeIDIndexName { get; }
        protected abstract string TerminalDeviceIDIndexName { get; }
        protected abstract string NetworkDeviceIDIndexName { get; }
        protected abstract string IntIDIndexName { get; }
        protected abstract string RoomIDIndexName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Adapter> builder);

        public void Configure(EntityTypeBuilder<Adapter> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(255);
            builder.Property(x => x.SerialNumber).IsRequired(false).HasMaxLength(255);
            builder.Property(x => x.Note).IsRequired(false).HasMaxLength(1000);
            builder.Property(x => x.RowVersion).IsRequired(true);
            builder.Property(x => x.Identifier).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.Code).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.ExternalID).IsRequired(true).HasMaxLength(250);
            builder.Property(x => x.InventoryNumber).IsRequired(false).HasMaxLength(50);

            builder.HasOne(x => x.Model)
                .WithMany()
                .HasForeignKey(x => x.AdapterTypeID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(AdapterTypeForeignKeyName);

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

            builder.HasOne(x => x.SlotType)
                .WithMany()
                .HasForeignKey(x => x.SlotTypeID)
                .HasPrincipalKey(x => x.ID)
                .IsRequired(false)
                .HasConstraintName(SlotTypeForeignKeyName);

            builder.HasIndex(x => x.AdapterTypeID).HasDatabaseName(AdapterTypeIDIndexName);
            builder.HasIndex(x => x.TerminalDeviceID).HasDatabaseName(TerminalDeviceIDIndexName);
            builder.HasIndex(x => x.NetworkDeviceID).HasDatabaseName(NetworkDeviceIDIndexName);
            builder.HasIndex(x => x.ID).HasDatabaseName(IntIDIndexName);
            builder.HasIndex(x => x.RoomID).HasDatabaseName(RoomIDIndexName);

            ConfigureDatabase(builder);
        }
    }
}