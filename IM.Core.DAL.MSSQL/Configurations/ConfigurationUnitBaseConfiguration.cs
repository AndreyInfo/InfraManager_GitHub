using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ConfigurationUnitBaseConfiguration : ConfigurationUnitBaseConfigurationBase
{
    protected override string KeyName => "PK_ConfigurationUnitBase";

    protected override string CriticalityFK => "FK_ConfigurationUnitBase_Criticality";

    protected override string InfrastructureSegmentFK => "FK_ConfigurationUnitBase_InfrastructureSegment";

    protected override string LifeCycleStateFK => "FK_ConfigurationUnitBase_LifeCycleState";

    protected override string ProductCatalogTypeFK => "FK_ConfigurationUnitBase_ProductCatalogType";

    protected override void ConfigureDataBase(EntityTypeBuilder<ConfigurationUnitBase> builder)
    {
        builder.ToTable("ConfigurationUnitBase", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.Number).HasColumnName("Number");
        builder.Property(e => e.Name).HasColumnName("Name");
        builder.Property(e => e.Description).HasColumnName("Description");
        builder.Property(e => e.Note).HasColumnName("Note");
        builder.Property(e => e.ExternalID).HasColumnName("ExternalID");
        builder.Property(e => e.Tags).HasColumnName("Tags");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.DateReceived).HasColumnName("DateReceived").HasColumnType("datetime");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("ProductCatalogTypeID");
        builder.Property(e => e.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(e => e.InfrastructureSegmentID).HasColumnName("InfrastructureSegmentID");
        builder.Property(e => e.CriticalityID).HasColumnName("CriticalityID");
        builder.Property(e => e.DateChanged).HasColumnName("DateChanged").HasColumnType("datetime");
        builder.Property(e => e.ChangedBy).HasColumnName("ChangedBy");
        builder.Property(e => e.DateLastInquired).HasColumnName("DateLastInquired").HasColumnType("datetime");
        builder.Property(e => e.DateAnnulated).HasColumnName("DateAnnulated").HasColumnType("datetime");
        builder.Property(e => e.OrganizationItemID).HasColumnName("OrganizationItemID");
        builder.Property(e => e.OrganizationItemClassID).HasColumnName("OrganizationItemClassID");
        builder.Property(e => e.OwnerID).HasColumnName("OwnerID");
        builder.Property(e => e.ClientID).HasColumnName("ClientID");
        builder.Property(e => e.ConfigurationUnitSchemeID).HasColumnName("ConfigurationUnitSchemeID");
        builder.Property(e => e.RowVersion)
            .HasColumnType("timestamp")
            .HasColumnName("RowVersion")
            .IsRowVersion();
    }
}
