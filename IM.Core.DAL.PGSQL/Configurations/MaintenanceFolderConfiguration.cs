using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaintenanceFolderConfiguration : MaintenanceFolderConfigurationBase
{
    protected override string PrimaryKeyName => "pk_maintenance_folder";

    protected override string FolderFK => "fk_maintenance_folder_maintenance_folder";

    protected override string UINameInParent => "ui_name_parent_id_maintenance_folder";

    protected override string UIRootFolder => "ui_name_parent_id_is_null_maintenance_folder";

    protected override void ConfigureDataBase(EntityTypeBuilder<MaintenanceFolder> builder)
    {
        builder.ToTable("maintenance_folder", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.ParentID).HasColumnName("parent_id");
        builder.HasXminRowVersion(p => p.RowVersion);
    }
}