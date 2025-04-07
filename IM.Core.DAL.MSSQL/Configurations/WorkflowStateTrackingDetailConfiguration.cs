using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkflowStateTrackingDetailConfiguration : IEntityTypeConfiguration<WorkflowStateTrackingDetail>
    {
        public void Configure(EntityTypeBuilder<WorkflowStateTrackingDetail> builder)
        {
            builder.ToTable("WorkflowStateTrackingDetail", "dbo");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();
            builder.Property(x => x.NextUtcDate);
            builder.Property(x => x.TimeSpanInWorkMinutes);
            builder.Property(x => x.StageTimeSpanInMinutes);
            builder.Property(x => x.StageTimeSpanInWorkMinutes);
        }
    }
}
