using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class PeripheralConfiguration : PeripheralConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Peripheral";
        protected override string PeripheralTypeForeignKeyName => "FK_Peripheral_Type";
        protected override string TerminalDeviceForeignKeyName => "FK_Peripheral_TerminalDevice";
        protected override string NetworkDeviceForeignKeyName => "FK_Peripheral_NetworkDevice";
        protected override string RoomForeignKeyName => "FK_Peripheral_Room";
        protected override string IntIDIndexName => "IX_Peripheral_IntID";
        protected override string NetworkDeviceIDIndexName => "IX_Peripheral_NetworkDeviceID";
        protected override string RoomIDIndexName => "IX_Peripheral_RoomID";
        protected override string TerminalDeviceIDIndexName => "IX_Peripheral_TerminalDeviceID";
        protected override string PeripheralTypeIDIndexName => "IX_Peripheral_TypeID";

        protected override void ConfigureDatabase(EntityTypeBuilder<Peripheral> builder)
        {
            builder.ToTable("Peripheral", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("PeripheralID");
            builder.Property(x => x.PeripheralTypeID).HasColumnName("PeripheralTypeID");
            builder.Property(x => x.TerminalDeviceID).HasColumnName("TerminalDeviceID").HasComputedColumnSql("0");
            builder.Property(x => x.NetworkDeviceID).HasColumnName("NetworkDeviceID").HasComputedColumnSql("0");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.SerialNumber).HasColumnName("SerialNo");
            builder.Property(x => x.Note).HasColumnName("Note");
            builder.Property(x => x.ID).HasColumnName("IntID").HasDefaultValueSql("0");
            builder.Property(x => x.StateID).HasColumnName("StateID").HasDefaultValueSql("0");
            builder.Property(x => x.RoomID).HasColumnName("RoomID").HasDefaultValueSql("0");
            builder.Property(x => x.BWLoad).HasColumnName("BWLoad").HasColumnType("decimal");
            builder.Property(x => x.ColorLoad).HasColumnName("ColorLoad").HasColumnType("decimal");
            builder.Property(x => x.PhotoLoad).HasColumnName("FotoLoad").HasColumnType("decimal");
            builder.Property(x => x.ClassID).HasColumnName("ClassID");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").IsRowVersion();
            builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
            builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
            builder.Property(x => x.ComplementaryIntID).HasColumnName("ComplementaryIntID");
            builder.Property(x => x.Code).HasColumnName("Code");
            builder.Property(x => x.ExternalID).HasColumnName("ExternalID");
        }
    }
}
