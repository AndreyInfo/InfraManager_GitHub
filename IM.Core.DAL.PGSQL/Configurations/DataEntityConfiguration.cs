using IM.Core.DAL.Postgres;
using InfraManager.DAL.ConfigurationData;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

public class DataEntityConfiguration : DataEntityConfigurationBase
{
    protected override string PrimaryKeyName => "pk_data_entity";
    protected override string DeviceApplicationForeignKeyName => "fk_data_entity_device_application";
    protected override string VolumeForeignKeyName => "fk_data_entity_volume";
    protected override string InfrastructureSegmentForeignKeyName => "fk_data_entity_infrastructure_segment";
    protected override string CriticalityForeignKeyName => "fk_data_entity_criticality";
    protected override string LifeCycleStateForeignKeyName => "fk_data_entity_life_cycle_state";
    protected override string TypeForeignKeyName => "fk_data_entity_product_catalog_type";

    protected override void ConfigureDatabase(EntityTypeBuilder<DataEntity> builder)
    {
        builder.ToTable("data_entity", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.OrganizationItemID).HasColumnName("organization_item_id");
        builder.Property(x => x.OrganizationItemClassID).HasColumnName("organization_item_class_id");
        builder.Property(x => x.DeviceApplicationID).HasColumnName("device_application_id");
        builder.Property(x => x.Size).HasColumnName("size").HasColumnType("numeric(18, 2)");
        builder.HasXminRowVersion(x => x.RowVersion);
        builder.Property(x => x.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(x => x.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(x => x.VolumeID).HasColumnName("volume_id");
        builder.Property(x => x.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");
        builder.Property(x => x.CriticalityID).HasColumnName("criticality_id");
        builder.Property(x => x.ClientID).HasColumnName("client_id");
        builder.Property(x => x.ClientClassID).HasColumnName("client_class_id");
        builder.Property(x => x.DateAnnuled).HasColumnName("date_annuled").HasColumnType("timestamp(3)");
        builder.Property(x => x.DateReceived).HasColumnName("date_received").HasColumnType("timestamp(3)");
        builder.Property(x => x.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(x => x.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
    }
}