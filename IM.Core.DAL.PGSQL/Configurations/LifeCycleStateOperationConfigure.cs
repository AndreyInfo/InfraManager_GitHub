using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

internal class LifeCycleStateOperationConfigure : LifeCycleStateOperationConfigureBase
{
    protected override string PrimaryKey => "pk_life_cycle_state_operation";

    protected override string LifeCycleStateFK => "fk_life_cycle_state_operation";

    protected override string WorkOrderTemplateFK => "fk_life_cycle_state_operation_work_order_template";

    protected override string LifeCycleStateIX => "ix_life_cycle_state_operation_life_cycle_state";

    protected override string WorkOrderTemplateIX => "ix_life_cycle_state_operation_work_order_template";

    protected override string StateAndTemplateIX => "ix_life_cycle_state_operation_life_cycle_state_work_order_template";

    protected override string UINameByLifeCycleStateID => "ui_name_life_cycle_state_operation_by_life_cycle_state_id";

    protected override string RoleLifeCycleStateOperationsFK => "fk_role_life_cycle_state_operation_life_cycle_state_operation";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperation> builder)
    {
        builder.ToTable("life_cycle_state_operation", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("id");
        builder.Property(e => e.Name).HasColumnName("name");
        builder.Property(e => e.Icon).HasColumnName("icon").HasColumnType("bytea");
        builder.Property(e => e.IconName).HasColumnName("icon_name");
        builder.Property(e => e.Sequence).HasColumnName("sequence");
        builder.Property(e => e.CommandType).HasColumnName("command_type");
        builder.Property(e => e.LifeCycleStateID).HasColumnName("life_cycle_state_id");
        builder.Property(e => e.WorkOrderTemplateID).HasColumnName("work_order_template_id");
    }
}