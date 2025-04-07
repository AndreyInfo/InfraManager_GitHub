using InfraManager.DAL.Asset;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class FinanceCenterConfigurationBase : IEntityTypeConfiguration<FinanceCenter>
{
    protected abstract string PrimaryKeyName { get; }

    public void Configure(EntityTypeBuilder<FinanceCenter> builder)
    {
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);

        builder.Property(x => x.Identifier).IsRequired(true).HasMaxLength(250);
        builder.Property(x => x.RowVersion).IsRequired(true);
        builder.Property(x => x.ExternalID).IsRequired(true).HasMaxLength(250);
        
        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<FinanceCenter> builder);
}