using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderReferenceConfigurationBase : IEntityTypeConfiguration<WorkOrderReference>
    {
        protected abstract string PrimaryKeyName { get; }

        public void Configure(EntityTypeBuilder<WorkOrderReference> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.ReferenceName).IsRequired().HasMaxLength(16);

            builder.HasDiscriminator(x => x.ObjectClassID)
                .HasValue<WorkOrderReference<Problem>>(ObjectClass.Problem)
                .HasValue<NullWorkOrderReference>(ObjectClass.Unknown)
                .HasValue<WorkOrderReference<Call>>(ObjectClass.Call)
                .HasValue<WorkOrderReference<ChangeRequest>>(ObjectClass.ChangeRequest)
                .HasValue<WorkOrderReference<MassIncident>>(ObjectClass.MassIncident);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkOrderReference> builder);
    }
}
