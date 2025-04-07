using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class TechnicalFailuresCategoryConfiguration : TechnicalFailureCategoryConfigurationBase
{
    protected override string PrimaryKeyName => "PK_TechnicalFailuresCategory";
    protected override string UniqueKeyName => "UK_TechnicalFailuresCategory_Name";
    protected override string IMObjIDUniqueKeyName => "UK_TechnicalFailuresCategory_IMObjID";

    protected override void ConfigureDataProvider(EntityTypeBuilder<TechnicalFailureCategory> builder)
    {
        builder.ToTable("TechnicalFailureCategory", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.IMObjID).HasColumnName("IMObjID").HasDefaultValueSql("NEWID()");
    }
}