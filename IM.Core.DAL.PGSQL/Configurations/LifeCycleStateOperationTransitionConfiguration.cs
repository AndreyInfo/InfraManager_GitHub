using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;
internal sealed class LifeCycleStateOperationTransitionConfiguration : LifeCycleStateOperationTransitionConfigurationBase
{
    protected override string PrimaryKeyName => "pk_life_cycle_state_operation_transition";

    protected override string StateFK => "fk_life_cycle_state_operation_transition_life_cycle_state_finis";

    protected override string OperationFK => "fk_life_cycle_state_operation_transition_life_cycle_state_opera";

    protected override string OperationIX => "ix_life_cycle_state_operation_transition_life_cycle_state_opera";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperationTransition> builder)
    {
        builder.ToTable("life_cycle_state_operation_transition", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.Mode).HasColumnName("mode");
        builder.Property(c => c.OperationID).HasColumnName("life_cycle_state_operation_id");
        builder.Property(c => c.FinishStateID).HasColumnName("finish_life_cycle_state_id");
    }
}
