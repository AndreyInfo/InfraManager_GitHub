using IM.Core.DAL.Postgres;
using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkflowEventConfiguration : IEntityTypeConfiguration<WorkflowEvent>
    {
        public void Configure(EntityTypeBuilder<WorkflowEvent> builder)
        {
            builder.ToTable("workflow_event", Options.Scheme);
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID)
                .HasColumnName("id")
                .IsRequired();
            builder.Property(x => x.WorkflowID)
                .HasColumnName("workflow_id")
                .IsRequired();
            builder.Property(x => x.UtcTimeStamp)
                .HasColumnName("utc_time_stamp")
                .IsRequired();
            builder.Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();
            builder.Property(x => x.Message)
                .HasColumnName("message")
                .IsRequired();
            builder.Property(x => x.StateID)
                .HasColumnName("state_id")
                .IsRequired();
            builder.Property(x => x.ActivityID)
                .HasColumnName("activity_id")
                .IsRequired();
        }
    }
}