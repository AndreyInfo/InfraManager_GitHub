using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentInformationChannelConfiguration : MassIncidentInformationChannelConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_MassIncidentInformationChannel";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncidentInformationChannel> builder)
        {
            builder.ToTable("MassIncidentInformationChannel", "dbo");

            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.Name).HasColumnName("Name");
        }
    }
}
