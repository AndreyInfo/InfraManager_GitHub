using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using InfraManager.DAL.Postgres;
using IM.Core.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class WorkplaceConfiguration : WorkplaceConfigurationBase
{
    protected override string UniqueNameConstraint => "ui_workplace_name_into_room";

    protected override string IXWorkplaceIMObjID => "ix_workplace_im_obj_id";

    protected override string PrimaryKey => "pk_workplace";

    protected override string WorkplacesFK => "fk_workplace_room";

    protected override string DFID => "nextval('pk_workplace_id_seq'::regclass)";

    protected override string DFIMObjID => "(gen_random_uuid())";

    protected override string IXWorkplaceRoomID => "ix_workplace_room_id";

    protected override void OnConfigurePartial(EntityTypeBuilder<Workplace> builder)
    {
        builder.ToTable("workplace", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("identificator");

        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
        builder.Property(e => e.RoomID).HasColumnName("room_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}
