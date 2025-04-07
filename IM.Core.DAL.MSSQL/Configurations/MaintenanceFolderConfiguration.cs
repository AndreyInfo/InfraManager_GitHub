using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaintenanceFolderConfiguration : MaintenanceFolderConfigurationBase
{
    protected override string PrimaryKeyName => "PK_MaintenanceFolder";

    protected override string FolderFK => "FK_MaintenanceFolder_MaintenanceFolder";

    protected override string UINameInParent => "UI_Name_ParentID_MaintenanceFolder";

    protected override string UIRootFolder => "UI_Name_ParentID_IsNull_MaintenanceFolder";

    protected override void ConfigureDataBase(EntityTypeBuilder<MaintenanceFolder> builder)
    {
        builder.ToTable("MaintenanceFolder", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.ParentID).HasColumnName("ParentID");
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
