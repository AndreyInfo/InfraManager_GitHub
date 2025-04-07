using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.TechnicalFailuresCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class ServiceTechnicalFailureCategoryConfiguration : ServiceTechnicalFailureCategoryBase
{
    protected override string PrimaryKeyName => "PK_HandlingTechnicalFailures";

    protected override string ServiceCategoryUniqueKeyName => "UI_HandlingTechnicalFailures_Service_Category ";

    protected override string ServiceForeignKeyName => "FK_HandlingTechnicalFailures_Service";

    protected override string CategoryForeignKeyName => "FK_HandlingTechnicalFailures_Category";

    protected override string GroupForeignKeyName => "FK_HandlingTechnicalFailures_Group";

    protected override string IMObjIDDefaultValue => "NEWID()";

    protected override string ServiceIDColumnName => "ServiceID";

    protected override string CategoryIDColumnName => "TechnicalFailureCategoryID";

    protected override void ConfigureDataBase(EntityTypeBuilder<ServiceTechnicalFailureCategory> builder)
    {
        builder.ToTable("ServiceTechnicalFailureCategory", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
        builder.Property(c => c.GroupID).HasColumnName("GroupID");
    }
}
