using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

public class DashboardDevExConfiguration : DashboardDevExConfigurationBase
{
    protected override string PrimaryKeyName => "pk_dashboard_de";
    protected override string DashboardForeignKeyName => "fk_dashboard_de_dashboard";
    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardDevEx> builder)
    {
        builder.ToTable("dashboard_de", Options.Scheme);

        builder.Property(x => x.DashboardID)
            .HasColumnType("uuid")
            .HasColumnName("dashboard_id");
        builder.Property(x => x.Data)
            .HasColumnType("text")
            .HasColumnName("data");
    }
}