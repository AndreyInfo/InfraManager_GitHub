using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class DashboardFolderConfiguration : DashboardFolderConfigurationBase
{
    protected override string PrimaryKeyName => "pk_dashboard_folder";

    protected override string FolderForeignKeyName => "fk_dashboard_folder_dashboard_folder";

    protected override string FolderNameInParentFolderUI => "ui_dashboard_folder_name_by_folder_id";

    protected override string NameParentIsNullUI => "ui_dashboard_folder_name_parent_is_null";

    protected override void ConfigureDatabase(EntityTypeBuilder<DashboardFolder> builder)
    {
        builder.ToTable("dashboard_folder", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ParentDashboardFolderID).HasColumnName("parent_dashboard_folder_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}
