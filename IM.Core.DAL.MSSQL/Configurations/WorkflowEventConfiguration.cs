using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkflowEventConfiguration : IEntityTypeConfiguration<WorkflowEvent>
    {
        public void Configure(EntityTypeBuilder<WorkflowEvent> builder)
        {
            builder.ToTable("WorkflowEvent", "dbo");
            builder.HasKey(x => x.ID);

            builder.Property(x => x.ID).IsRequired();
            builder.Property(x => x.WorkflowID).IsRequired();
            builder.Property(x => x.UtcTimeStamp).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.Message).IsRequired();
            builder.Property(x => x.StateID).IsRequired();
            builder.Property(x => x.ActivityID).IsRequired();
        }
    }
}
