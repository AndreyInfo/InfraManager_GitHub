using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class ServiceContractTypeConfiguration : ServiceContractTypeConfigurationBase
{
    protected override string PrimaryKeyName => "pk_service_contract_type";

    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContractType> builder)
    {
        builder.ToTable("service_contract_type", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}