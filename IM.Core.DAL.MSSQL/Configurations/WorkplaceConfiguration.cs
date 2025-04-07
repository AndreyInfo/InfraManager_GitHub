using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class WorkplaceConfiguration : WorkplaceConfigurationBase
{
    protected override string UniqueNameConstraint => "UI_Workplace_Name_Into_Room";

    protected override string IXWorkplaceIMObjID => "IX_Workplace_IMObjID";

    protected override string PrimaryKey => "PK_Рабочее место";

    protected override string WorkplacesFK => "FK_Рабочее место_Комната";

    protected override string DFID => "(NEXT VALUE FOR [PK_Workplace_ID_Seq])";

    protected override string DFIMObjID => "(newid())";

    protected override string IXWorkplaceRoomID => "IX_Workplace_RoomID";

    protected override void OnConfigurePartial(EntityTypeBuilder<Workplace> builder)
    {
        builder.ToTable("Рабочее место", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.Name).HasColumnName("Название");
        builder.Property(e => e.Note).HasColumnName("Примечание");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(x => x.RoomID).HasColumnName("ИД комнаты");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");

        builder.Property(c => c.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
