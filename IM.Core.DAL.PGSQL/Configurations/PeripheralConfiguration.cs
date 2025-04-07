using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Postgres.Configurations
{
    public class PeripheralConfiguration : PeripheralConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_peripheral";
        protected override string PeripheralTypeForeignKeyName => "fk_peripheral_type";
        protected override string TerminalDeviceForeignKeyName => "fk_peripheral_terminal_device";
        protected override string NetworkDeviceForeignKeyName => "fk_peripheral_network_device";
        protected override string RoomForeignKeyName => "fk_peripheral_room";
        protected override string IntIDIndexName => "ix_peripheral_int_id";
        protected override string NetworkDeviceIDIndexName => "ix_peripheral_network_device_id";
        protected override string RoomIDIndexName => "ix_peripheral_room_id";
        protected override string TerminalDeviceIDIndexName => "ix_peripheral_terminal_device_id";
        protected override string PeripheralTypeIDIndexName => "ix_peripheral_type_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<Peripheral> builder)
        {
            builder.ToTable("peripheral", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("peripheral_id");
            builder.Property(x => x.PeripheralTypeID).HasColumnName("peripheral_type_id");
            builder.Property(x => x.TerminalDeviceID).HasColumnName("terminal_device_id").HasDefaultValueSql("0");
            builder.Property(x => x.NetworkDeviceID).HasColumnName("network_device_id").HasDefaultValueSql("0");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.SerialNumber).HasColumnName("serial_no");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.ID).HasColumnName("int_id").HasDefaultValueSql("0");
            builder.Property(x => x.StateID).HasColumnName("state_id").HasDefaultValueSql("0");
            builder.Property(x => x.RoomID).HasColumnName("room_id").HasDefaultValueSql("0");
            builder.Property(x => x.BWLoad).HasColumnName("bw_load").HasColumnType("numeric(18)");
            builder.Property(x => x.ColorLoad).HasColumnName("color_load").HasColumnType("numeric(18)");
            builder.Property(x => x.PhotoLoad).HasColumnName("foto_load").HasColumnType("numeric(18)");
            builder.Property(x => x.ClassID).HasColumnName("class_id");
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(x => x.ComplementaryIntID).HasColumnName("complementary_int_id");
            builder.Property(x => x.Code).HasColumnName("code");
            builder.Property(x => x.ExternalID).HasColumnName("external_id");
            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}