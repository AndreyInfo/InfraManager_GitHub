using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core = InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    public class ExternalEventReferenceConfiguration : Core.ExternalEventReferenceConfiguration
    {
        protected override void DatabaseConfigure(EntityTypeBuilder<ExternalEventReference> builder)
        {
            builder.ToTable("ExternalEventReference", "dbo");
            builder.Property(x => x.ExternalEventId).HasColumnName("ExternalEventId");
            builder.Property(x => x.WorkflowId).HasColumnName("WorkflowId");
            builder.Property(x => x.ConsiderationCount).HasColumnName("ConsiderationCount");
        }
    }
}
