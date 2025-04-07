using InfraManager.DAL.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ExternalEventReferenceConfiguration : IEntityTypeConfiguration<ExternalEventReference>
    {
        public void Configure(EntityTypeBuilder<ExternalEventReference> builder)
        {
            DatabaseConfigure(builder);
            builder.HasKey(x => new { x.ExternalEventId, x.WorkflowId });
            builder.Property(x => x.ExternalEventId).ValueGeneratedNever();
            builder.Property(x => x.WorkflowId).ValueGeneratedNever();
        }

        protected abstract void DatabaseConfigure(EntityTypeBuilder<ExternalEventReference> builder);
    }
}
