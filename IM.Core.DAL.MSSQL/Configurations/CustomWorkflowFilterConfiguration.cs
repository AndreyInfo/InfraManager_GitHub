using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CustomWorkflowFilterConfiguration : IEntityTypeConfiguration<CustomWorkflowFilter>
    {
        public void Configure(EntityTypeBuilder<CustomWorkflowFilter> builder)
        {
            builder.ToTable("CustomWorkflowFilter", "dbo");
            builder.HasNoKey();

            builder.Property(x => x.UserID)
                   .HasColumnName("WorkflowSchemeID");
            builder.Property(x => x.WorkflowSchemeID)
                   .HasColumnName("UserID");
        }
    }
}
