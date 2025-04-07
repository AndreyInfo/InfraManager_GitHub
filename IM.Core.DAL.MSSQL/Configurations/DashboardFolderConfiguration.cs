using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Microsoft.SqlServer.Configurations;

internal sealed class DashboardFolderConfiguration : DashboardFolderConfigurationBase
{
    protected override string PrimaryKeyName => "PK_DashboardFolder";

    protected override string FolderForeignKeyName => "FK_DashboardFolder_DashboardFolder";

    protected override string FolderNameInParentFolderUI => "UI_DashboardFolder_Name_ByFolderID";

    protected override string NameParentIsNullUI => "UI_DashboardFolder_Name_ParentIsNull";

    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardFolder> builder)
    {
        builder.ToTable("DashboardFolder", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ParentDashboardFolderID).HasColumnName("ParentDashboardFolderID");
        builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
    }
}

