using InfraManager.DAL.WF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkflowRequestConfiguration : IEntityTypeConfiguration<WorkflowRequest>
    {
        public void Configure(EntityTypeBuilder<WorkflowRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();

            DatabaseConfigure(builder);
        }

        protected abstract void DatabaseConfigure(EntityTypeBuilder<WorkflowRequest> builder);
    }
}
