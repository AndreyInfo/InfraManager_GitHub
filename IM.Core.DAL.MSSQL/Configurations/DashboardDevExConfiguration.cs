using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

public class DashboardDevExConfiguration : DashboardDevExConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DashboardDE";
    protected override string DashboardForeignKeyName => "FK_DashboardDE_Dashboard";

    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardDevEx> builder)
    {
        builder.ToTable("DashboardDE", "dbo");

        builder.Property(x => x.DashboardID).HasColumnName("DashboardID");
        builder.Property(x => x.Data).HasColumnName("Data");
    }
}