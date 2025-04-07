using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class ServiceContractTypeConfiguration : ServiceContractTypeConfigurationBase
{
    protected override string PrimaryKeyName => "PK_ServiceContractType";
    protected override void ConfigureDatabase(EntityTypeBuilder<ServiceContractType> builder)
    {
        builder.ToTable("ServiceContractType", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
    }
}