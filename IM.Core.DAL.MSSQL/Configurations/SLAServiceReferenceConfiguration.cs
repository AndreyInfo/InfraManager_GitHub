using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class SLAServiceReferenceConfiguration : SLAServiceReferenceConfigurationBase
{
    protected override void ConfigureDatabase(EntityTypeBuilder<SLAServiceReference> builder)
    {
        builder.ToTable("SLAServiceReference", "dbo");
        builder.Property(x => x.ServiceReferenceID).HasColumnName("ServiceReferenceID");
        builder.Property(x => x.SLAID).HasColumnName("SLAID");
    }
}