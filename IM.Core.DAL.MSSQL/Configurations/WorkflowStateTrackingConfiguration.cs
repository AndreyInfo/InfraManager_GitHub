using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkflowStateTrackingConfiguration : IEntityTypeConfiguration<WorkflowStateTracking>
    {
        public void Configure(EntityTypeBuilder<WorkflowStateTracking> builder)
        {
            builder.ToTable("WorkflowStateTracking", "dbo");
            builder.HasKey(x => new { x.WorkflowTrackingId, x.StateId });

            builder.Property(x => x.WorkflowTrackingId)
                .IsRequired();
            builder.Property(x => x.StateId)
                .IsRequired();
            builder.Property(x => x.StateName)
                .IsRequired();
            builder.Property(x => x.UtcLeavedAt);
            builder.Property(x => x.ExecutorId);
            builder.Property(x => x.UtcEnteredAt)
                .IsRequired();
            builder.Property(x => x.TimeSpanInWorkMinutes);
        }
    }
}
