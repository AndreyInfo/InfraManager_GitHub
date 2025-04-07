using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class LifeCycleStateConfiguration : LifeCycleStateConfigurationBase
{
    protected override string PrimaryKey => "pk_life_cycle_state";

    protected override string DefaultValueID => "gen_random_uuid()";

    protected override string LifeCycleIX => "ix_life_cycle_state_life_cycle";

    protected override string LifeCycleFK => "fk_life_cycle_state_life_cycle";

    protected override string UINameByLifeCycleID => "ui_name_life_cycle_state_by_life_cycle_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleState> builder)
    {
        builder.ToTable("life_cycle_state", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.LifeCycleID).HasColumnName("life_cycle_id");
        builder.Property(e => e.IsApplied).HasColumnName("is_applied");
        builder.Property(e => e.IsDefault).HasColumnName("is_default");
        builder.Property(e => e.IsInRepair).HasColumnName("is_in_repair");
        builder.Property(e => e.IsWrittenOff).HasColumnName("is_written_off");
        builder.Property(e => e.CanCreateAgreement).HasColumnName("can_create_agreement");
        builder.Property(e => e.CanIncludeInPurchase).HasColumnName("can_include_in_purchase");
        builder.Property(e => e.CanIncludeInInfrastructure).HasColumnName("can_include_in_infrastructure");
        builder.Property(e => e.CanIncludeInActiveRequest).HasColumnName("can_include_in_active_request");
    }
}