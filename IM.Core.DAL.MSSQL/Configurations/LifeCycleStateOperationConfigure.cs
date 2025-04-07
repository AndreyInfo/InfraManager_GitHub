using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class LifeCycleStateOperationConfigure : LifeCycleStateOperationConfigureBase
{
    protected override string PrimaryKey => "PK_LifeCycleStateOperation";

    protected override string LifeCycleStateFK => "FK_LifeCycleState_Operation";

    protected override string WorkOrderTemplateFK => "FK_LifeCycleStateOperation_WorkOrderTemplate";

    protected override string LifeCycleStateIX => "IX_LifeCycleStateOperation_LifeCycleState";

    protected override string WorkOrderTemplateIX => "IX_LifeCycleStateOperation_WorkOrderTemplate";

    protected override string StateAndTemplateIX => "IX_LifeCycleStateOperation_LifeCycleState_WorkOrderTemplate";

    protected override string UINameByLifeCycleStateID => "UI_Name_LifeCycleStateOperation_ByLifeCycleStateID";

    protected override string RoleLifeCycleStateOperationsFK => "FK_RoleLifeCycleStateOperation_LifeCycleStateOperation";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperation> builder)
    {
        builder.ToTable("LifeCycleStateOperation", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(e => e.Name).HasColumnName("Name");
        builder.Property(e => e.Icon).HasColumnName("Icon").HasColumnType("image");
        builder.Property(e => e.IconName).HasColumnName("IconName");
        builder.Property(e => e.Sequence).HasColumnName("Sequence");
        builder.Property(e => e.CommandType).HasColumnName("CommandType");
        builder.Property(e => e.LifeCycleStateID).HasColumnName("LifeCycleStateID");
        builder.Property(e => e.WorkOrderTemplateID).HasColumnName("WorkOrderTemplateID");
    }
}
