using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using InfraManager.DAL.Location;
using IM.Core.DAL.Microsoft.SqlServer;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class BuildingConfiguration : BuildingConfigurationBase
{
    protected override string UniqueNameConstraint => "UI_Building_Name_Into_Organization";

    protected override string PrimaryKey => "PK_Здание";

    protected override string IXBuildingIMObjID => "IX_Building_IMObjID";

    protected override string IXBuildingOrganizationID => "IX_Building_OrganizationID";

    protected override string IDDefaultValueSQL => "(NEXT VALUE FOR [PK_Buiding_ID_Seq])";

    protected override string IMObjIDDefaultValueSQL => "(newid())";

    protected override void OnConfigurePartial(EntityTypeBuilder<Building> builder)
    {
        builder.ToTable("Здание", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("Идентификатор");
        builder.Property(e => e.Name).HasColumnName("Название");
        builder.Property(e => e.IMObjID).HasColumnName("IMObjID");
        builder.Property(e => e.Index).HasColumnName("Индекс");
        builder.Property(e => e.Region).HasColumnName("Область-Край");
        builder.Property(e => e.City).HasColumnName("Город");
        builder.Property(e => e.Area).HasColumnName("Район");
        builder.Property(e => e.Street).HasColumnName("Улица");
        builder.Property(e => e.HousePart).HasColumnName("Строение");
        builder.Property(e => e.Housing).HasColumnName("Корпус");
        builder.Property(e => e.WiringScheme).HasColumnName("Схема Разводки");
        builder.Property(e => e.Image).HasColumnName("Изображение");
        builder.Property(e => e.Note).HasColumnName("Примечание");
        builder.Property(e => e.VisioID).HasColumnName("Visio_ID");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(e => e.SubdivisionID).HasColumnName("ИД подразделения");
        builder.Property(e => e.OrganizationID).HasColumnName("ИД организации");
        builder.Property(e => e.House).HasColumnName("House");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.TimeZoneID).HasColumnName("TimeZoneID");


        builder.Property(e => e.RowVersion)
            .IsRequired()
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
