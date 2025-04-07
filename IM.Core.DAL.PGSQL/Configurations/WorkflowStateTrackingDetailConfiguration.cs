using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using IM.Core.DAL.Postgres;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkflowStateTrackingDetailConfiguration : IEntityTypeConfiguration<WorkflowStateTrackingDetail>
    {
        public void Configure(EntityTypeBuilder<WorkflowStateTrackingDetail> builder)
        {
            builder.ToTable("workflow_state_tracking_detail", Options.Scheme);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired()
                .HasColumnName("id");
            builder.Property(x => x.NextUtcDate)
                .HasColumnName("next_utc_date");
            builder.Property(x => x.TimeSpanInWorkMinutes)
                .HasColumnName("time_span_in_work_minutes");
            builder.Property(x => x.StageTimeSpanInMinutes)
                .HasColumnName("stage_time_span_in_minutes");
            builder.Property(x => x.StageTimeSpanInWorkMinutes)
                .HasColumnName("stage_time_span_in_work_minutes");
        }
    }
}