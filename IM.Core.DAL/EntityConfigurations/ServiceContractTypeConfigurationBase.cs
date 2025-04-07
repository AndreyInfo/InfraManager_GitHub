using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class ServiceContractTypeConfigurationBase : IEntityTypeConfiguration<ServiceContractType>
{
    protected abstract string PrimaryKeyName { get; }

    public void Configure(EntityTypeBuilder<ServiceContractType> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.RowVersion).IsRequired(true);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<ServiceContractType> builder);
}