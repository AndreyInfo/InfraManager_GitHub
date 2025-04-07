using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

public class FinanceCenterConfiguration : FinanceCenterConfigurationBase
{
    protected override string PrimaryKeyName => "PK_FinanceCenter";

    protected override void ConfigureDatabase(EntityTypeBuilder<FinanceCenter> builder)
    {
        builder.ToTable("FinanceCenter", "dbo");

        builder.Property(x => x.ID).HasColumnName("ObjectID");
        builder.Property(x => x.ObjectClassID).HasColumnName("ObjectClassID");
        builder.Property(x => x.Identifier).HasColumnName("Identifier");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
    }
}