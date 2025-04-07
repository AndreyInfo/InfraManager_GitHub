using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class DashboardItemConfiguration : DashboardItemConfigurationBase
{
    protected override string PrimaryKeyName => "pk_dashboard_item";
    protected override string DashboardForeignKeyName => "fk_dashboard_item_dashboard";
    
    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardItem> builder)
    {
        builder.ToTable("dashboard_item", Options.Scheme);

        builder.Property(x => x.ID)
            .HasColumnType("uuid")
            .HasColumnName("id")
            .HasDefaultValueSql("gen_random_uuid()");
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasColumnName("Name");
        builder.Property(x => x.DashboardID)
            .HasColumnType("uuid")
            .HasColumnName("dashboard_id");
        builder.Property(x => x.Left)
            .HasColumnType("numeric")
            .HasColumnName("Left");
        builder.Property(x => x.Top)
            .HasColumnType("numeric")
            .HasColumnName("Top");
        builder.Property(x => x.ZIndex)
            .HasColumnType("int4")
            .HasColumnName("ZIndex");
        builder.Property(x => x.Width)
            .HasColumnType("numeric")
            .HasColumnName("Width");
        builder.Property(x => x.Height)
            .HasColumnType("numeric")
            .HasColumnName("Height");
        builder.Property(x => x.BackgroundColor)
            .HasColumnType("int4")
            .HasColumnName("BackgroundColor");
        builder.Property(x => x.TextColor)
            .HasColumnType("int4")
            .HasColumnName("TextColor");
        builder.Property(x => x.WidgetType)
            .HasColumnType("int2")
            .HasColumnName("WidgetType");
        builder.Property(x => x.FactorType)
            .HasColumnType("varchar")
            .HasColumnName("FactorType");
        builder.Property(x => x.FactorData)
            .HasColumnType("bytea")
            .HasColumnName("FactorData");
    }
}