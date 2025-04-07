using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using InfraManager.DAL.Postgres;
using IM.Core.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class RoomConfiguration : RoomConfigurationBase
{
    protected override string UniqueNameConstraint => "ui_room_name_into_floor";

    protected override string IXRoomIMObjID => "ix_room_im_obj_id";

    protected override string IXRoomFloorID => "ix_room_floor_id";

    protected override string PrimaryKey => "pk_room";

    protected override string FloorFK => "fk_room_floor";

    protected override string IDDefaultValueSQL => "nextval('room_id_seq'::regclass)";

    protected override string IMObjIDDefaultValueSQL => "(gen_random_uuid())";

    protected override void OnConfigurePartial(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("room", Options.Scheme);
        
        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.Size).HasColumnName("size");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.VisioID).HasColumnName("visio_id");
        builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
        builder.Property(e => e.TypeID).HasColumnName("type_id");
        builder.Property(e => e.Key).HasColumnName("key");
        builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.Plan).HasColumnName("drawing");
        builder.Property(e => e.TypeID).HasColumnName("type_id");
        builder.Property(e => e.FloorID).HasColumnName("floor_id");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.ServiceZone).HasColumnName("service_zone");
        builder.Property(e => e.LocationPoint).HasColumnName("location_point");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}