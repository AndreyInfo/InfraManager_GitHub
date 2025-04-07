using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class DashboardConfiguration : DashboardConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Dashboard";
    protected override string FolderForeignKeyName => "FK_Dashboard_DashboardFolder";
    protected override string UINameByFolderID => "UI_Dashboard_Name_ByFolderID";

    protected override void ConfigureDatabase(EntityTypeBuilder<Dashboard> builder)
    {
        builder.ToTable("Dashboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ObjectClassID).HasColumnName("DashboardClassID");
        builder.Property(x => x.DashboardFolderID).HasColumnName("DashboardFolderID");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
    }
}

