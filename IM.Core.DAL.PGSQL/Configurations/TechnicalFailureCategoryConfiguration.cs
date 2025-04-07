using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class TechnicalFailureCategoryConfiguration : TechnicalFailureCategoryConfigurationBase
{
    protected override string PrimaryKeyName => "technical_failure_category_id";
    protected override string UniqueKeyName => "uk_technical_failure_category_name";
    protected override string IMObjIDUniqueKeyName => "uk_technical_failures_category_im_obj_id";

    protected override void ConfigureDataProvider(EntityTypeBuilder<TechnicalFailureCategory> builder)
    {
        builder.ToTable("technical_failure_category", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.IMObjID).HasColumnName("im_obj_id").HasDefaultValueSql("gen_random_uuid()");
    }
}