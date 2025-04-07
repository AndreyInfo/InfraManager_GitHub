using InfraManager.DAL.Dashboards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DashboardItemConfigurationBase : IEntityTypeConfiguration<DashboardItem>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string DashboardForeignKeyName { get; }

    public void Configure(EntityTypeBuilder<DashboardItem> builder)
    {
        builder.Property(x => x.ID).IsRequired(true);
        builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
        builder
            .HasOne(x => x.Dashboard)
            .WithMany()
            .HasForeignKey(x => x.DashboardID)
            .HasConstraintName(DashboardForeignKeyName);

        builder.Property(x => x.Name)
            .IsRequired(true)
            .HasMaxLength(250);
        builder.Property(x => x.DashboardID)
            .IsRequired(true);
        builder.Property(x => x.Left)
            .IsRequired(true);
        builder.Property(x => x.Top)
            .IsRequired(true);
        builder.Property(x => x.ZIndex)
            .IsRequired(true);
        builder.Property(x => x.Width)
            .IsRequired(true);
        builder.Property(x => x.Height)
            .IsRequired(true);
        builder.Property(x => x.BackgroundColor)
            .IsRequired(true);
        builder.Property(x => x.TextColor)
            .IsRequired(true);
        builder.Property(x => x.WidgetType)
            .IsRequired(true);
        builder.Property(x => x.FactorType)
            .IsRequired(true)
            .HasMaxLength(500);
        builder.Property(x => x.FactorData)
            .IsRequired(false);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<DashboardItem> builder);
}