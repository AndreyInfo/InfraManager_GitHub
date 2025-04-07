using IM.Core.DAL.Postgres;
using InfraManager.DAL.Asset;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations;

public class FinanceCenterConfiguration : FinanceCenterConfigurationBase
{
    protected override string PrimaryKeyName => "pk_finance_center";

    protected override void ConfigureDatabase(EntityTypeBuilder<FinanceCenter> builder)
    {
        builder.ToTable("finance_center", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("object_id");
        builder.Property(x => x.ObjectClassID).HasColumnName("object_class_id").IsRequired();
        builder.Property(x => x.Identifier).HasColumnName("identifier");
        builder.HasXminRowVersion(x => x.RowVersion);
        builder.Property(x => x.ExternalID).HasColumnName("external_id");
    }
}