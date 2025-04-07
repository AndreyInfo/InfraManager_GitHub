using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal sealed class MaintenanceConfiguration : MaintenanceConfigurationBase
{
    protected override string PrimaryKeyName => "PK_Maintenance";
    protected override string UINameInFolder => "UI_Name_FolderID_Maintenance";
    protected override string WorkOrderTemplateForeignKey => "FK_Maintenance_WorkOrderTemplate";

    protected override void ConfigureDatabase(EntityTypeBuilder<Maintenance> builder)
    {
        builder.ToTable("Maintenance", Options.Scheme );
        
        builder.Property(x => x.ID).HasColumnName("ID");
        builder.Property(x => x.Name).HasColumnName("Name");
        builder.Property(x => x.Note).HasColumnName("Note");
        builder.Property(x => x.FolderID).HasColumnName("MaintenanceFolderID");
        builder.Property(x => x.WorkOrderTemplateID).HasColumnName("WorkOrderTemplateId");
        builder.Property(x => x.State).HasColumnName("State");
        builder.Property(x => x.Multiplicity).HasColumnName("Multiplicity");
        builder.Property(e => e.RowVersion)
            .IsRowVersion()
            .HasColumnName("RowVersion");
    }
}
