using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.Location;
using IM.Core.DAL.Postgres;

namespace IM.Core.DAL.PGSQL.Configurations;

public partial class BuildingConfiguration : BuildingConfigurationBase
{
    protected override string UniqueNameConstraint => "ui_building_name_into_organization";

    protected override string PrimaryKey => "pk_building";

    protected override string IXBuildingIMObjID => "ix_building_im_obj_id";

    protected override string IXBuildingOrganizationID => "ix_building_organization_id";

    protected override string IDDefaultValueSQL => "nextval('pk_building_id_seq'::regclass)";

    protected override string IMObjIDDefaultValueSQL => "(gen_random_uuid())";

    protected override void OnConfigurePartial(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("building", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("identificator");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.IMObjID).HasColumnName("im_obj_id");
        builder.Property(e => e.Index).HasColumnName("index");
        builder.Property(e => e.Region).HasColumnName("region");
        builder.Property(e => e.City).HasColumnName("city");
        builder.Property(e => e.Area).HasColumnName("district");
        builder.Property(e => e.Street).HasColumnName("street");
        builder.Property(e => e.HousePart).HasColumnName("building");
        builder.Property(e => e.Housing).HasColumnName("block");
        builder.Property(e => e.WiringScheme).HasColumnName("wiring_scheme");
        builder.Property(e => e.Image).HasColumnName("image");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.VisioID).HasColumnName("visio_id");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.SubdivisionID).HasColumnName("department_id");
        builder.Property(e => e.OrganizationID).HasColumnName("organization_id");
        builder.Property(e => e.House).HasColumnName("house");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(e => e.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(e => e.TimeZoneID).HasColumnName("time_zone_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}
