using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class ServiceTechnicalFailureCategoryConfiguration : ServiceTechnicalFailureCategoryBase
{
    protected override string PrimaryKeyName => "service_technical_failure_category_pkey";

    protected override string ServiceCategoryUniqueKeyName => "uk_service_technical_failure_category";

    protected override string ServiceForeignKeyName => "fk_service_technical_failure_category_service";

    protected override string CategoryForeignKeyName => "fk_service_technical_failure_category_category";

    protected override string GroupForeignKeyName => "fk_service_technical_failure_category_group";

    protected override string IMObjIDDefaultValue => "(gen_random_uuid())";

    protected override string ServiceIDColumnName => "service_id";

    protected override string CategoryIDColumnName => "technical_failure_category_id";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceTechnicalFailureCategory> builder)
    {
        builder.ToTable("service_technical_failure_category", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("id").HasDefaultValueSql("nexval('service_technical_failure_category')");
        builder.Property(c => c.IMObjID).HasColumnName("im_obj_id");
        builder.Property(c => c.GroupID).HasColumnName("group_id");
    }
}
