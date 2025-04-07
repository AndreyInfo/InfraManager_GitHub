using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class RoleLifeCycleStateOperationConfigure : RoleLifeCycleStateOperationConfigureBase
{
    protected override string PrimaryKey => "PK_RoleLifeCycleStateOperation";

    protected override string RoleFK => "FK_RoleLifeCycleStateOperation_Role";

    protected override string LifeCycleOperationFK => "FK_RoleLifeCycleStateOperation_LifeCycleStateOperation";

    protected override void ConfigureDataBase(EntityTypeBuilder<RoleLifeCycleStateOperation> builder)
    {
        builder.ToTable("RoleLifeCycleStateOperation", Options.Scheme);

        builder.Property(c=> c.RoleID).HasColumnName("RoleID");
        builder.Property(c=> c.LifeCycleStateOperationID).HasColumnName("LifeCycleStateOperationID");
    }
}
