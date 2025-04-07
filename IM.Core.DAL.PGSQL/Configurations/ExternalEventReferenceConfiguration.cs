using IM.Core.DAL.Postgres;
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
            builder.ToTable("external_event_reference", Options.Scheme);
            builder.Property(e => e.ConsiderationCount).HasColumnName("consideration_count").HasColumnType("INT");
            builder.Property(e => e.ExternalEventId).HasColumnName("external_event_id").HasColumnType("UUID");
            builder.Property(e => e.WorkflowId).HasColumnName("workflow_id").HasColumnType("UUID");
            // ExternalEventReference => external_event_reference
            builder.HasKey(e => new {e.ExternalEventId, e.WorkflowId}).HasName("pk_external_event_reference");
            builder.HasIndex(e => e.ExternalEventId, "ix_external_event_reference_external_event_id");
            builder.HasIndex(e => e.WorkflowId, "ix_external_event_reference_workflow_id");
        }
    }
}