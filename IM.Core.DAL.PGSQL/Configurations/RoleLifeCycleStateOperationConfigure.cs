using IM.Core.DAL.Postgres;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal sealed class RoleLifeCycleStateOperationConfigure : RoleLifeCycleStateOperationConfigureBase
{
    protected override string PrimaryKey => "pk_role_life_cycle_state_operation";

    protected override string RoleFK => "fk_role_life_cycle_state_operation_role";

    protected override string LifeCycleOperationFK => "fk_role_life_cycle_state_operation_life_cycle_state_operation";

    protected override void ConfigureDataBase(EntityTypeBuilder<RoleLifeCycleStateOperation> builder)
    {
        builder.ToTable("role_life_cycle_state_operation", Options.Scheme);

        builder.Property(e => e.RoleID).HasColumnName("role_id");
        builder.Property(e => e.LifeCycleStateOperationID).HasColumnName("life_cycle_state_operation_id");
    }
}