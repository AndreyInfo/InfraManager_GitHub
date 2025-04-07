using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal class WorkflowTrackingConfiguration : WorkflowTrackingConfigurationBase
{
    protected override string KeyName => "pk_workflow_tracking";

    protected override void ConfigureDataBase(EntityTypeBuilder<WorkflowTracking> builder)
    {
        builder.ToTable("workflow_tracking", Options.Scheme);


        builder.Property(x => x.ID)
            .HasColumnName("id");
        builder.Property(x => x.WorkflowSchemeID)
            .HasColumnName("workflow_scheme_id");
        builder.Property(x => x.WorkflowSchemeIdentifier)
            .HasColumnName("workflow_scheme_identifier");
        builder.Property(x => x.WorkflowSchemeVersion)
            .HasColumnName("workflow_scheme_version");
        builder.Property(x => x.EntityClassID)
            .HasColumnName("entity_class_id");
        builder.Property(x => x.EntityID)
            .HasColumnName("entity_id");
        builder.Property(x => x.UtcInitializedAt)
            .HasColumnName("utc_initialized_at");
        builder.Property(x => x.UtcTerminatedAt)
            .HasColumnName("utc_terminated_at");
        
        builder.HasMany(x => x.StateTrackings).WithOne().HasForeignKey(x => x.WorkflowTrackingId);
    }
}