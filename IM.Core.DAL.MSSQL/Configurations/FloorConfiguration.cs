using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.Location;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class FloorConfiguration : FloorConfigurationBase
{
    protected override string PrimaryKey => "PK_Этаж";

    protected override string UniqueNameConstraint => "UI_Floor_Name_Into_Building";

    protected override string BuildingFK => "FK_Этаж_Здание";

    protected override string IMObjIDDefaultValueSQL => "(newid())";

    protected override string IDDefaultValueSQL => "(NEXT VALUE FOR [PK_Floor_ID_Seq])";

    protected override string IXFloorIMObjID => "IX_Floor_IMObjID";

    protected override string IXFloorBuildingID => "IX_Floor_BuildingID";

    protected override void OnConfigurePartial(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("Этаж", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.Name).HasColumnName("Название");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.BuildingID).HasColumnName("ИД здания");
        builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
        builder.Property(e => e.MethodNamingRoom).HasColumnName("Способ именования комнат");
        builder.Property(e => e.FloorDrawing).HasColumnName("Чертеж этажа");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.VisioID).HasColumnName("Visio_ID");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");

        builder.Property(e => e.RowVersion)
           .IsRowVersion()
           .HasColumnName("RowVersion");

        builder.Property(e => e.Note)
           .HasColumnName("Примечание")
           .HasColumnType("ntext");


        builder.Property(e => e.Vsdfile)
            .HasColumnType("image")
            .HasColumnName("VSDFile");
    }
}
