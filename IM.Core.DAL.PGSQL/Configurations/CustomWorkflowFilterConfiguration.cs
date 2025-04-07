using IM.Core.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CustomWorkflowFilterConfiguration : IEntityTypeConfiguration<CustomWorkflowFilter>
    {
        public void Configure(EntityTypeBuilder<CustomWorkflowFilter> builder)
        {
            builder.ToTable("custom_workflow_filter", Options.Scheme);
            builder.HasNoKey();

            builder.Property(x => x.UserID)
                .HasColumnName("workflow_scheme_id");
            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("user_id");
        }
    }
}