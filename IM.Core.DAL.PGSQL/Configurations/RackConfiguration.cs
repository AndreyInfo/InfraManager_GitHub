using IM.Core.DAL.Postgres;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class RackConfiguration : RackConfigurationBase
{
    protected override string KeyName => "pk_cabinet";
    protected override string RoomFK => "fk_cabinet_room";
    protected override string CabinetTypeFK => "fk_cabinet_cabinet_types";
    protected override string DefaultValueExternalID => "('')";
    protected override string DefaultValueIMObjID => "(gen_random_uuid())";
    protected override string IndexRoomID => "ix_rack_room_id";
    protected override string IndexIMObjID => "ix_rack_im_obj_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<Rack> builder)
    {
        builder.ToTable("cabinet", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.Action).HasColumnName("action");
        builder.Property(e => e.Drawing).HasColumnName("drawing");
        builder.Property(e => e.NumberingScheme).HasColumnName("numbering_scheme");
        builder.Property(e => e.ComplementaryGuidID).HasColumnName("complementary_guid_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.VisioID).HasColumnName("visio_id");
        builder.Property(e => e.TypeID).HasColumnName("type_id");
        builder.Property(e => e.RoomID).HasColumnName("room_id");
        builder.Property(e => e.FloorID).HasColumnName("floor_id");
        builder.Property(e => e.BuildingID).HasColumnName("work_order_id");
        builder.Property(e => e.FillingScheme).HasColumnName("filling_scheme");
    }
}
