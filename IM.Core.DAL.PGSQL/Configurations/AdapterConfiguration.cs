using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class AdapterConfiguration : AdapterConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_adapter";
        protected override string AdapterTypeForeignKeyName => "fk_adapter_type";
        protected override string TerminalDeviceForeignKeyName => "fk_adapter_terminal_device";
        protected override string RoomForeignKeyName => "fk_adapter_room";
        protected override string SlotTypeForeignKeyName => "fk_adapter_slot_type";
        protected override string NetworkDeviceForeignKeyName => "fk_adapter_network_device";
        protected override string IntIDIndexName => "ix_adapter_int_id";
        protected override string NetworkDeviceIDIndexName => "ix_adapter_network_device_id";
        protected override string RoomIDIndexName => "ix_adapter_room_id";
        protected override string TerminalDeviceIDIndexName => "ix_adapter_terminal_device_id";
        protected override string AdapterTypeIDIndexName => "ix_adapter_type_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<Adapter> builder)
        {
            builder.ToTable("adapter", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("adapter_id").HasDefaultValueSql("gen_random_uuid()");
            builder.Property(x => x.AdapterTypeID).HasColumnName("adapter_type_id");
            builder.Property(x => x.TerminalDeviceID).HasColumnName("terminal_device_id").HasDefaultValueSql("0");
            builder.Property(x => x.NetworkDeviceID).HasColumnName("network_device_id").HasDefaultValueSql("0");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.SerialNumber).HasColumnName("serial_no");
            builder.Property(x => x.Note).HasColumnName("note");
            builder.Property(x => x.ID).HasColumnName("int_id").HasDefaultValueSql("0");
            builder.Property(x => x.Integrated).HasColumnName("integrated").HasDefaultValueSql("false");
            builder.Property(x => x.StateID).HasColumnName("state_id").HasDefaultValueSql("0");
            builder.Property(x => x.RoomID).HasColumnName("room_id").HasDefaultValueSql("0");
            builder.Property(x => x.ClassID).HasColumnName("class_id");
            builder.HasXminRowVersion(x => x.RowVersion);
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
            builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
            builder.Property(x => x.ComplementaryIntID).HasColumnName("complementary_int_id");
            builder.Property(x => x.Identifier).HasColumnName("identifier");
            builder.Property(x => x.Code).HasColumnName("code");
            builder.Property(x => x.SlotTypeID).HasColumnName("slot_type_id");
            builder.Property(x => x.ExternalID).HasColumnName("external_id");
            builder.Property(x => x.InventoryNumber).HasColumnName("inventory_number");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}