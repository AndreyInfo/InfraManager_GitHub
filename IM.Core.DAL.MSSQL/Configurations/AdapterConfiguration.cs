using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class AdapterConfiguration : AdapterConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Adapter";
        protected override string AdapterTypeForeignKeyName => "FK_Adapter_Type";
        protected override string TerminalDeviceForeignKeyName => "FK_Adapter_TerminalDevice";
        protected override string RoomForeignKeyName => "FK_Adapter_Room";
        protected override string SlotTypeForeignKeyName => "FK_Adapter_SlotType";
        protected override string NetworkDeviceForeignKeyName => "FK_Adapter_NetworkDevice";
        protected override string IntIDIndexName => "IX_Adapter_IntID";
        protected override string NetworkDeviceIDIndexName => "IX_Adapter_NetworkDeviceID";
        protected override string RoomIDIndexName => "IX_Adapter_RoomID";
        protected override string TerminalDeviceIDIndexName => "IX_Adapter_TerminalDeviceID";
        protected override string AdapterTypeIDIndexName => "IX_Adapter_TypeID";

        protected override void ConfigureDatabase(EntityTypeBuilder<Adapter> builder)
        {
            builder.ToTable("Adapter", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("AdapterID").HasDefaultValueSql("newid()");
            builder.Property(x => x.AdapterTypeID).HasColumnName("AdapterTypeID");
            builder.Property(x => x.TerminalDeviceID).HasColumnName("TerminalDeviceID").HasDefaultValueSql("0");
            builder.Property(x => x.NetworkDeviceID).HasColumnName("NetworkDeviceID").HasDefaultValueSql("0");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.SerialNumber).HasColumnName("SerialNo");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ID).HasColumnName("IntID").HasDefaultValueSql("0");
            builder.Property(x => x.Integrated).HasColumnName("Integrated").HasDefaultValueSql("0");
            builder.Property(x => x.StateID).HasColumnName("StateID").HasDefaultValueSql("0");
            builder.Property(x => x.RoomID).HasColumnName("RoomID").HasDefaultValueSql("0");
            builder.Property(x => x.ClassID).HasColumnName("ClassID");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion().IsConcurrencyToken();
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
            builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(x => x.ComplementaryIntID).HasColumnName("ComplementaryIntID");
            builder.Property(x => x.Identifier).HasColumnName("Identifier");
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.SlotTypeID).HasColumnName("SlotTypeID");
            builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
            builder.Property(x => x.InventoryNumber).HasColumnName("InventoryNumber");
        }
    }
}