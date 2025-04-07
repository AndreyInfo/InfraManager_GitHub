using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class WorkflowTrackingConfiguration : WorkflowTrackingConfigurationBase
{
    protected override string KeyName => "PK_WorkflowTracking";

    protected override void ConfigureDataBase(EntityTypeBuilder<WorkflowTracking> builder)
    {
        builder.ToTable("WorkflowTracking", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.WorkflowSchemeID).HasColumnName("WorkflowSchemeID");
        builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
        builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("WorkflowSchemeVersion");
        builder.Property(x => x.EntityClassID).HasColumnName("EntityClassID");
        builder.Property(x => x.EntityID).HasColumnName("EntityID");
        builder.Property(x => x.UtcInitializedAt).HasColumnName("UtcInitializedAt");
        builder.Property(x => x.UtcTerminatedAt).HasColumnName("UtcTerminatedAt");
        
        builder.HasMany(x => x.StateTrackings).WithOne().HasForeignKey(x => x.WorkflowTrackingId);
    }
}
