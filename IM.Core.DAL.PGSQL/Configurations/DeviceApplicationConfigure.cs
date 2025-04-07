using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class DeviceApplicationConfigure : DeviceApplicationConfigureBase
{
    protected override string PrimaryKeyName => "pk_device_application";

    protected override string ProductCatalogTypeForeignKey => "fk_device_application_prouct_catalog_type";

    protected override string LifeCycleStateForeignKey => "fk_device_application_life_cycle_state";

    protected override string CriticalityForeignKey => "fk_device_application_criticality";

    protected override string InfrastructureSegmentForeignKey => "fk_device_application_infrastructure_segment";

    protected override void ConfigureDataBase(EntityTypeBuilder<DeviceApplication> builder)
    {
        builder.ToTable("device_application", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.Note).HasColumnName("note");
        builder.Property(c => c.DeviceID).HasColumnName("device_id");
        builder.Property(c => c.DeviceClassID).HasColumnName("device_class_id");
        builder.Property(c => c.OrganizationItemID).HasColumnName("organization_item_id");
        builder.Property(c => c.OrganizationItemClassID).HasColumnName("organization_item_class_id");
        builder.Property(c => c.PeripheralDatabaseID).HasColumnName("peripheral_database_id");
        builder.Property(c => c.ComplementaryID).HasColumnName("complementary_id");
        builder.Property(c => c.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");
        builder.Property(c => c.CriticalityID).HasColumnName("criticality_id");
        builder.Property(c => c.DateAnnuled).HasColumnName("date_annuled");
        builder.Property(c => c.DateReceived).HasColumnName("date_received");
        builder.Property(c => c.ClientID).HasColumnName("client_id");
        builder.Property(c => c.ClientClassID).HasColumnName("client_class_id");
        builder.Property(c => c.LifeCycleStateID).HasColumnName("lifecycle_state_id");
        builder.Property(c => c.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");

        builder.HasXminRowVersion(c=> c.RowVersion);
    }
}
