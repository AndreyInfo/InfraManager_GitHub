using InfraManager.DAL.ServiceDesk.Negotiations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class NegotiationConfigurationBase : IEntityTypeConfiguration<Negotiation>
    {
        public void Configure(EntityTypeBuilder<Negotiation> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.IMObjID).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(Negotiation.NameMaxLength);

            builder.HasMany(x => x.NegotiationUsers)
                .WithOne()
                .HasForeignKey(x => x.NegotiationID);

            builder.HasDiscriminator(x => x.ObjectClassID)
                .HasValue<WorkOrderNegotiation>(ObjectClass.WorkOrder)
                .HasValue<ProblemNegotiation>(ObjectClass.Problem)
                .HasValue<ChangeRequestNegotiation>(ObjectClass.ChangeRequest)
                .HasValue<CallNegotiation>(ObjectClass.Call)
                .HasValue<MassiveIncidentNegotiation>(ObjectClass.MassIncident);

            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Negotiation> builder);
    }
}
