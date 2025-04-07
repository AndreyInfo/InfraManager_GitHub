using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class LifeCycleConfiguration : LifeCycleConfigurationBase
{
    protected override string PrimaryKeyName => "PK_LifeCycleType";
    protected override string LifeCycleStateFK => "FK_LifeCycle_States";
    protected override string DefaultValueID => "(newid())";

    protected override string NameUI => "UI_Name_LifeCycle";

    protected override string FormForeignKey => "FK_LifeCycle_FormID";

    protected override void ConfigureDataBase(EntityTypeBuilder<LifeCycle> builder)
    {
        builder.ToTable("LifeCycle", Options.Scheme);

        builder.Property(e => e.ID).HasColumnName("ID");
        builder.Property(i => i.Name).HasColumnName("Name");
        builder.Property(e => e.Removed).HasColumnName("Removed");
        builder.Property(e => e.IsFixed).HasColumnName("Fixed");
        builder.Property(e => e.Type).HasColumnName("Type");
        builder.Property(e => e.FormID).HasColumnName("FormID");
        builder.Property(x => x.RowVersion)
            .HasColumnType("timestamp")
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
