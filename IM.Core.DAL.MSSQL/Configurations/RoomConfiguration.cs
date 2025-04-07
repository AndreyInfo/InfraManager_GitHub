using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class RoomConfiguration : RoomConfigurationBase
{
    protected override string UniqueNameConstraint => "UI_Room_Name_Into_Floor";

    protected override string PrimaryKey => "PK_Комната";

    protected override string FloorFK => "FK_Комната_Этаж";

    protected override string IDDefaultValueSQL => "(NEXT VALUE FOR [PK_Room_ID_Seq])";

    protected override string IXRoomIMObjID => "IX_Room_IMObjID";

    protected override string IMObjIDDefaultValueSQL => "(newid())";

    protected override string IXRoomFloorID => "IX_Room_FloorID";

    protected override void OnConfigurePartial(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Комната", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.Name).HasColumnName("Название");
        builder.Property(e => e.Note).HasColumnName("Примечание");
        builder.Property(e => e.Size).HasColumnName("Размер");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.VisioID).HasColumnName("Visio_ID");
        builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
        builder.Property(e => e.TypeID).HasColumnName("ИД типа");
        builder.Property(e => e.Key).HasColumnName("Ключ");
        builder.Property(e => e.ServiceZone).HasColumnName("Обслуживаемая зона");
        builder.Property(e => e.LocationPoint).HasColumnName("Точка расположения");
        builder.Property(e => e.Plan).HasColumnName("Чертеж");
        builder.Property(x => x.FloorID).HasColumnName("ИД этажа");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");

        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
