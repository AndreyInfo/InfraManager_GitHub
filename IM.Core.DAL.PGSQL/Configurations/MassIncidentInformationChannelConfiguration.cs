using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentInformationChannelConfiguration : MassIncidentInformationChannelConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_mass_incident_information_channel";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentInformationChannel> builder)
        {
            builder.ToTable("mass_incident_information_channel", Options.Scheme);

            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.Name).HasColumnName("name");
        }
    }
}
