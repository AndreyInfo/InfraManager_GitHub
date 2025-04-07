using InfraManager.DAL.Dashboards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations;

public abstract class DashboardDevExConfigurationBase : IEntityTypeConfiguration<DashboardDevEx>
{
    protected abstract string PrimaryKeyName { get; }
    protected abstract string DashboardForeignKeyName { get; }

    public void Configure(EntityTypeBuilder<DashboardDevEx> builder)
    {
        builder.HasKey(x => x.DashboardID).HasName(PrimaryKeyName);

        builder.Property(x => x.DashboardID)
            .IsRequired(true);
        builder.Property(x => x.Data)
            .IsRequired(false);

        builder
            .HasOne(x => x.Dashboard)
            .WithMany()
            .HasForeignKey(x => x.DashboardID)
            .HasConstraintName(DashboardForeignKeyName);

        ConfigureDatabase(builder);
    }

    protected abstract void ConfigureDatabase(EntityTypeBuilder<DashboardDevEx> builder);
}