using InfraManager.DAL.ConfigurationUnits;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class ConfigurationUnitBaseConfiguration : ConfigurationUnitBaseConfigurationBase
{
    protected override string KeyName => "pk_configuration_unit_base";

    protected override string CriticalityFK => "fk_configuration_unit_base_criticality";

    protected override string InfrastructureSegmentFK => "fk_configuration_unit_base_infrastructure_segment";

    protected override string LifeCycleStateFK => "fk_configuration_unit_base_life_cycle_state";

    protected override string ProductCatalogTypeFK => "fk_configuration_unit_base_product_catalog_type";

    protected override void ConfigureDataBase(EntityTypeBuilder<ConfigurationUnitBase> builder)
    {
        builder.ToTable("configuration_unit_base", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Number).HasColumnName("number");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Description).HasColumnName("description");
        builder.Property(e => e.Note).HasColumnName("note");
        builder.Property(e => e.ExternalID).HasColumnName("external_id");
        builder.Property(e => e.Tags).HasColumnName("tags");
        builder.Property(e => e.CreatedBy).HasColumnName("created_by");
        builder.Property(e => e.DateReceived).HasColumnName("date_received").HasColumnType("timestamp(3) without time zone");
        builder.Property(e => e.ProductCatalogTypeID).HasColumnName("product_catalog_type_id");
        builder.Property(e => e.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(e => e.InfrastructureSegmentID).HasColumnName("infrastructure_segment_id");
        builder.Property(e => e.CriticalityID).HasColumnName("criticality_id");
        builder.Property(e => e.DateChanged).HasColumnName("date_changed").HasColumnType("timestamp(3) without time zone");
        builder.Property(e => e.ChangedBy).HasColumnName("changed_by");
        builder.Property(e => e.DateLastInquired).HasColumnName("date_last_inquired").HasColumnType("timestamp(3) without time zone");
        builder.Property(e => e.DateAnnulated).HasColumnName("date_annulated").HasColumnType("timestamp(3) without time zone");
        builder.Property(e => e.OrganizationItemID).HasColumnName("organization_item_id");
        builder.Property(e => e.OrganizationItemClassID).HasColumnName("organization_item_class_id");
        builder.Property(e => e.OwnerID).HasColumnName("owner_id");
        builder.Property(e => e.ClientID).HasColumnName("client_id");
        builder.Property(e => e.ConfigurationUnitSchemeID).HasColumnName("configuration_unit_scheme_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}
