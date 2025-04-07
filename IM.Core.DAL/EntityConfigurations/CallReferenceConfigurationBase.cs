using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CallReferenceConfigurationBase : IEntityTypeConfiguration<CallReference>
    {
        public void Configure(EntityTypeBuilder<CallReference> builder)
        {
            builder.HasKey(x => new { x.CallID, x.ObjectID }).HasName(PrimaryKeyName);
            builder.HasDiscriminator(x => x.ObjectClassID)
                .HasValue<CallReference<Problem>>(ObjectClass.Problem)
                .HasValue<CallReference<ChangeRequest>>(ObjectClass.ChangeRequest);

            ConfigureDatabase(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<CallReference> builder);
    }
}
