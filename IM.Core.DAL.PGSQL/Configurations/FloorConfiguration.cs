using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using InfraManager.DAL.Postgres;
using IM.Core.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class FloorConfiguration : FloorConfigurationBase
{
    protected override string UniqueNameConstraint => "ui_floor_name_into_building";

    protected override string PrimaryKey => "pk_floor";

    protected override string BuildingFK => "fk_floor_building";

    protected override string IMObjIDDefaultValueSQL => "(gen_random_uuid())";

    protected override string IDDefaultValueSQL => "nextval('pk_floor_id_seq'::regclass)";

    protected override string IXFloorIMObjID => "ix_floor_im_obj_id";

    protected override string IXFloorBuildingID => "ix_floor_building_id";

    protected override void OnConfigurePartial(EntityTypeBuilder<Floor> builder)
    {
        builder.ToTable("floor", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.VisioID).HasColumnName("visio_id");
        builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
        builder.Property(e => e.MethodNamingRoom).HasColumnName("room_naming_way");
        builder.Property(e => e.FloorDrawing).HasColumnName("floor_drawing");
        builder.Property(e => e.BuildingID).HasColumnName("building_id");
        builder.HasXminRowVersion(e => e.RowVersion);
        
        builder.Property(e => e.Note)
            .HasColumnName("note")
            .HasColumnType("text");

        builder.Property(e => e.Vsdfile)
            .HasColumnType("bytea")
            .HasColumnName("vsd_file");
    }
}