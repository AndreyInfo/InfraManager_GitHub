using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;
internal sealed class LifeCycleStateOperationTransitionConfiguration : LifeCycleStateOperationTransitionConfigurationBase
{
    protected override string PrimaryKeyName => "PK_LifeCycleStateOperationTransition";

    protected override string StateFK => "FK_LifeCycleStateOperationTransition_LifeCycleState_Finish";

    protected override string OperationFK => "FK_LifeCycleStateOperationTransition_LifeCycleStateOperation";

    protected override string OperationIX => "IX_LifeCycleStateOperationTransition_LifeCycleStateOperation";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleStateOperationTransition> builder)
    {
        builder.ToTable("LifeCycleStateOperationTransition", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Mode).HasColumnName("Mode");
        builder.Property(c => c.OperationID).HasColumnName("LifeCycleStateOperationID");
        builder.Property(c => c.FinishStateID).HasColumnName("FinishLifeCycleStateID");
    }
}
