using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class LifeCycleStateConfiguration : LifeCycleStateConfigurationBase
{
    protected override string PrimaryKey => "PK_LifeCycleState";

    protected override string DefaultValueID => "NEWID()";

    protected override string LifeCycleIX => "IX_LifeCycleState_LifeCycle";

    protected override string LifeCycleFK => "FK_LifeCycleState_LifeCycle";

    protected override string UINameByLifeCycleID => "UI_Name_LifeCycleStateOperation_ByLifeCycleStateID";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycleState> builder)
    {
        builder.ToTable("LifeCycleState", Options.Scheme);

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.IsApplied).HasColumnName("IsApplied");
        builder.Property(c => c.IsDefault).HasColumnName("IsDefault");
        builder.Property(c => c.IsInRepair).HasColumnName("IsInRepair");
        builder.Property(c => c.LifeCycleID).HasColumnName("LifeCycleID");
        builder.Property(c => c.IsWrittenOff).HasColumnName("IsWrittenOff");
        builder.Property(c => c.CanCreateAgreement).HasColumnName("CanCreateAgreement");
        builder.Property(c => c.CanIncludeInPurchase).HasColumnName("CanIncludeInPurchase");
        builder.Property(c => c.CanIncludeInActiveRequest).HasColumnName("CanIncludeInActiveRequest");
        builder.Property(c => c.CanIncludeInInfrastructure).HasColumnName("CanIncludeInInfrastructure");
    }
}
