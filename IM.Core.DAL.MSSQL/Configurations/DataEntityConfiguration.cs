using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.ConfigurationData;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class DataEntityConfiguration : DataEntityConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DataEntity";
    protected override string DeviceApplicationForeignKeyName => "FK_DataEntity_DeviceApplication";
    protected override string VolumeForeignKeyName => "FK_DataEntity_Volume";
    protected override string InfrastructureSegmentForeignKeyName => "fk_DataEntity_InfrastructureSegment";
    protected override string CriticalityForeignKeyName => "fk_DataEntity_Criticality";
    protected override string LifeCycleStateForeignKeyName => "FK_DataEntity_LifeCycleState";
    protected override string TypeForeignKeyName => "FK_DataEntity_ProductCatalogType";

    protected override void ConfigureDatabase(EntityTypeBuilder<DataEntity> builder)
    {
        builder.ToTable("DataEntity", Options.Scheme);
        
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.OrganizationItemID).HasColumnName("OrganizationItemID");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
        builder.Property(x => x.DeviceApplicationID).HasColumnName("DeviceApplicationID");
        builder.Property(x => x.Size).HasColumnName("Size").HasColumnType("decimal(18, 2)");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("PeripheralDatabaseID");
        builder.Property(x => x.ComplementaryID).HasColumnName("ComplementaryID");
        builder.Property(x => x.VolumeID).HasColumnName("VolumeID");
        builder.Property(x => x.InfrastructureSegmentID).HasColumnName("InfrastructureSegmentID");
        builder.Property(x => x.CriticalityID).HasColumnName("CriticalityID");
        builder.Property(x => x.ClientID).HasColumnName("ClientID");
        builder.Property(x => x.ClientClassID).HasColumnName("ClientClassID");
        builder.Property(x => x.DateAnnuled).HasColumnName("DateAnnuled").HasColumnType("datetime");
        builder.Property(x => x.DateReceived).HasColumnName("DateReceived").HasColumnType("datetime");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
    }
}