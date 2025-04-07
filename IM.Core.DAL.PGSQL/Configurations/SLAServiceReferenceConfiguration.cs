using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

public class SLAServiceReferenceConfiguration : SLAServiceReferenceConfigurationBase
{
    protected override void ConfigureDatabase(EntityTypeBuilder<SLAServiceReference> builder)
    {
        builder.ToTable("sla_service_reference", Options.Scheme);

        builder.Property(x => x.SLAID).HasColumnName("sla_id");
        builder.Property(x => x.ServiceReferenceID).HasColumnName("service_reference_id");
    }
}