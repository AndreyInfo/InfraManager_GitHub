using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class DashboardItemConfiguration : DashboardItemConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DashboardItem";
    protected override string DashboardForeignKeyName => "FK_DashboardItem_Dashboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardItem> builder)
    {
        builder.ToTable("DashboardItem", "dbo");

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name)
            .HasColumnType("varchar")
            .HasColumnName("Name");
        builder.Property(x => x.DashboardID).HasColumnName("DashboardID");
        builder.Property(x => x.Left)
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("Left");
        builder.Property(x => x.Top)
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("Top");
        builder.Property(x => x.ZIndex)
            .HasColumnName("ZIndex");
        builder.Property(x => x.Width)
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("Width");
        builder.Property(x => x.Height)
            .HasColumnType("decimal(18, 2)")
            .HasColumnName("Height");
        builder.Property(x => x.BackgroundColor)
            .HasColumnName("BackgroundColor");
        builder.Property(x => x.TextColor)
            .HasColumnName("TextColor");
        builder.Property(x => x.WidgetType)
            .HasColumnType("tinyint")
            .HasColumnName("WidgetType");
        builder.Property(x => x.FactorType)
            .HasColumnType("varchar")
            .HasColumnName("FactorType");
        builder.Property(x => x.FactorData)
            .HasColumnType("image")
            .HasColumnName("FactorData");
    }
}