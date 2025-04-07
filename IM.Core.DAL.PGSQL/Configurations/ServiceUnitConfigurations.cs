using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.EntityConfigurations;

namespace IM.Core.DAL.Postgres.Configurations;

internal class ServiceUnitConfigurations : ServiceUnitConfigurationBase
{
    protected override string KeyName => "pk_service_unit";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceUnit> builder)
    {
        builder.ToTable("service_unit", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id");
        builder.Property(c => c.Name).HasColumnName("name");
        builder.Property(c => c.ResponsibleID).HasColumnName("responsible_id");
        builder.HasXminRowVersion(c => c.RowVersion);
    }
}