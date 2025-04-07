using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class MassIncidentInformationChannelConfigurationBase : IEntityTypeConfiguration<MassIncidentInformationChannel>
    {
        public void Configure(EntityTypeBuilder<MassIncidentInformationChannel> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder.Property(x => x.ID).ValueGeneratedNever();
            builder.Property(x => x.Name).IsRequired(true).HasMaxLength(50);

            ConfigureDataProvider(builder);
        }

        protected abstract string PrimaryKeyName { get; }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<MassIncidentInformationChannel> builder);
    }
}
