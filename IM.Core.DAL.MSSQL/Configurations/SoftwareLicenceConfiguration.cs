using InfraManager.DAL.Software;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class SoftwareLicenceConfiguration : SoftwareLicenceConfigurationBase
{
    protected override string PrimaryKeyName => "PK_SoftwareLicence";

    protected override string ParentForeignKey => "FK_SoftwareLicence_SoftwareLicence";
    protected override string SoftwareModelForeignKey => "FK_SoftwareLicence_SoftwareModel";
    protected override string ProductCatalogTypeForeignKey => "FK_SoftwareLicence_ProductCataloType";

    protected override string IndexByRoomIntID => "IX_SoftwareLicence_RoomIntID";
    protected override string IndexBySoftwareModelID => "IX_SoftwareLicence_SoftwareModelID";
    protected override string IndexByProductCatalogTypeID => "IX_SoftwareLicence_ProductCatalogTypeID";
    protected override string IndexBySoftwareLicenceModelID => "IX_SoftwareLicence_SoftwareLicenceModelID";

    protected override void ConfigureDataBase(EntityTypeBuilder<SoftwareLicence> builder)
    {
        builder.ToTable("SoftwareLicence", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.Name).HasColumnName("Name");
        builder.Property(e => e.Note).HasColumnName("Note");
        builder.Property(e => e.IsFull).HasColumnName("IsFull");
        builder.Property(e => e.ParentID).HasColumnName("ParentID");
        builder.Property(e => e.RoomIntID).HasColumnName("RoomIntID");
        builder.Property(e => e.OemdeviceID).HasColumnName("OEMDeviceID");
        builder.Property(e => e.LimitInHours).HasColumnName("LimitInHours");
        builder.Property(e => e.HaspadapterID).HasColumnName("HASPAdapterID");
        builder.Property(e => e.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(e => e.InventoryNumber).HasColumnName("InventoryNumber");
        builder.Property(e => e.SoftwareModelID).HasColumnName("SoftwareModelID");
        builder.Property(e => e.OemdeviceClassID).HasColumnName("OEMDeviceClassID");
        builder.Property(e => e.DowngradeAvailable).HasColumnName("DowngradeAvailable");
        builder.Property(e => e.RestrictionsHzFrom).HasColumnName("RestrictionsHzFrom");
        builder.Property(e => e.RestrictionsHzTill).HasColumnName("RestrictionsHzFrom");
        builder.Property(e => e.SoftwareLicenceType).HasColumnName("SoftwareLicenceType");
        builder.Property(e => e.RestrictionsCpuFrom).HasColumnName("RestrictionsCPUFrom");
        builder.Property(e => e.RestrictionsCpuTill).HasColumnName("RestrictionsCPUTill");
        builder.Property(e => e.RestrictionsCoreFrom).HasColumnName("RestrictionsCoreFrom");
        builder.Property(e => e.RestrictionsCoreTill).HasColumnName("RestrictionsCoreTill");
        builder.Property(e => e.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(e => e.SoftwareLicenceScheme).HasColumnName("SoftwareLicenceScheme");
        builder.Property(e => e.SoftwareExecutionCount).HasColumnName("SoftwareExecutionCount");
        builder.Property(e => e.SoftwareLicenceModelID).HasColumnName("SoftwareLicenceModelID");
        builder.Property(e => e.SoftwareLicenceSchemeEnum).HasColumnName("software_licence_scheme_enum");

        builder.Property(e => e.BeginDate).HasColumnType("datetime").HasColumnName("BeginDate");
        builder.Property(e => e.EndDate).HasColumnType("datetime").HasColumnName("EndDate");

        builder.Property(e => e.RowVersion)
            .IsRequired()
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
