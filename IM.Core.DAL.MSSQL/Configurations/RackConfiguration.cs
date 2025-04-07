using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class RackConfiguration : RackConfigurationBase
{
    protected override string KeyName => "PK_Шкаф";
    protected override string RoomFK => "FK_Шкаф_Комната";
    protected override string CabinetTypeFK => "FK_Шкаф_Типы шкафов";
    protected override string DefaultValueExternalID => "('')";
    protected override string DefaultValueIMObjID => "(newid())";
    protected override string IndexRoomID => "IX_Rack_RoomID";
    protected override string IndexIMObjID => "IX_Rack_IMObjID";

    protected override void ConfigureDataBase(EntityTypeBuilder<Rack> builder)
    {
        builder.ToTable("Шкаф", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.Name).HasColumnName("Название");
        builder.Property(e => e.Note).HasColumnName("Примечание");
        builder.Property(e => e.Action).HasColumnName("Действие");
        builder.Property(e => e.Drawing).HasColumnName("Чертеж");
        builder.Property(e => e.NumberingScheme).HasColumnName("NumberingScheme");
        builder.Property(e => e.ComplementaryGuidID).HasColumnName("ComplementaryGuidID");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.VisioID).HasColumnName("Visio_ID");
        builder.Property(e => e.TypeID).HasColumnName("ИД типа");
        builder.Property(e => e.RoomID).HasColumnName("ИД комнаты");
        builder.Property(e => e.FloorID).HasColumnName("ИД этажа");
        builder.Property(e => e.BuildingID).HasColumnName("ИД задания");
        builder.Property(e => e.FillingScheme).HasColumnName("Схема заполнения");
    }
}
