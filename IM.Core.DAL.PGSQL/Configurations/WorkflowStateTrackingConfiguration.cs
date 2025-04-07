using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkflowStateTrackingConfiguration : IEntityTypeConfiguration<WorkflowStateTracking>
    {
        public void Configure(EntityTypeBuilder<WorkflowStateTracking> builder)
        {
            builder.ToTable("workflow_state_tracking", Options.Scheme);
            builder.HasKey(x => new {x.WorkflowTrackingId, x.StateId});

            builder.Property(x => x.WorkflowTrackingId)
                .IsRequired()
                .HasColumnName("workflow_tracking_id");
            builder.Property(x => x.StateId)
                .IsRequired()
                .HasColumnName("state_id");
            builder.Property(x => x.StateName)
                .IsRequired()
                .HasColumnName("state_name");
            builder.Property(x => x.UtcLeavedAt)
                .HasColumnName("utc_leaved_at");
            builder.Property(x => x.ExecutorId)
                .HasColumnName("executor_id");
            builder.Property(x => x.UtcEnteredAt)
                .IsRequired()
                .HasColumnName("utc_entered_at");
            builder.Property(x => x.TimeSpanInWorkMinutes)
                .HasColumnName("time_span_in_work_minutes");
        }
    }
}