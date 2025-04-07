using InfraManager.DAL.Dashboards;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.Postgres.Configurations;

internal sealed class DashboardConfiguration : DashboardConfigurationBase
{
    protected override string PrimaryKeyName => "pk_dashboard";
    protected override string FolderForeignKeyName => "fk_dashboard_dashboard_folder";
    protected override string UINameByFolderID => "ui_dashboard_name_by_folder_id";


    protected override void ConfigureDatabase(EntityTypeBuilder<Dashboard> builder)
    {
        builder.ToTable("dashboard", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ObjectClassID).HasColumnName("dashboard_class_id");
        builder.Property(x => x.DashboardFolderID).HasColumnName("dashboard_folder_id");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}

