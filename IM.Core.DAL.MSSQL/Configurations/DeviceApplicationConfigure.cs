using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class DeviceApplicationConfigure : DeviceApplicationConfigureBase
{
    protected override string PrimaryKeyName => "PK_DeviceApplication";

    protected override string ProductCatalogTypeForeignKey => "fk_DeviceApplication_InfrastructureSegment";

    protected override string LifeCycleStateForeignKey => "FK_DeviceApplication_LifeCycleState";

    protected override string CriticalityForeignKey => "fk_DeviceApplication_Criticality";

    protected override string InfrastructureSegmentForeignKey => "FK_DeviceApplication_ProuctCatalogType";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceApplication> builder)
    {
        builder.ToTable("DeviceApplication", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.Note).HasColumnName("Note");
        builder.Property(c => c.DeviceID).HasColumnName("DeviceID");
        builder.Property(c => c.DeviceClassID).HasColumnName("DeviceClassID");
        builder.Property(c => c.OrganizationItemID).HasColumnName("OrganizationItemID");
        builder.Property(c => c.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
        builder.Property(c => c.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(c => c.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(c => c.InfrastructureSegmentID).HasColumnName("InfrastructureSegmentID");
        builder.Property(c => c.CriticalityID).HasColumnName("CriticalityID");
        builder.Property(c => c.DateAnnuled).HasColumnName("DateAnnuled");
        builder.Property(c => c.DateReceived).HasColumnName("DateReceived");
        builder.Property(c => c.ClientID).HasColumnName("ClientID");
        builder.Property(c => c.ClientClassID).HasColumnName("ClientClassID");
        builder.Property(c => c.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(c => c.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        
        builder.Property(c => c.RowVersion).HasColumnName("RowVersion")
            .IsRowVersion()
            .HasColumnType("timestamp");
    }
}
