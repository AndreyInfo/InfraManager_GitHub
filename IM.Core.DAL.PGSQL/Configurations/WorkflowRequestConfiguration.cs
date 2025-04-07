using IM.Core.DAL.Postgres;
using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class WorkflowRequestConfiguration : Core.WorkflowRequestConfiguration
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<WorkflowRequest> builder)
        {
            builder.ToTable("workflow_request", Options.Scheme);
            builder.Property(e => e.Id).HasColumnName("id").HasColumnType("UUID");
            // WorkflowRequest => workflow_request
            builder.HasKey(e => e.Id).HasName("pk_workflow_request");
        }
    }
}