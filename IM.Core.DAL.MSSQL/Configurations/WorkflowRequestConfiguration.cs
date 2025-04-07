using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkflowRequestConfiguration : Core.WorkflowRequestConfiguration
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<WorkflowRequest> builder)
        {
            builder.ToTable("WorkflowRequest", "dbo");
            builder.Property(x => x.Id).HasColumnName("ID");
        }
    }
}
