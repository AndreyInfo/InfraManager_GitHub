using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.MaintenanceWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations;

internal sealed class MaintenanceConfiguration : MaintenanceConfigurationBase
{
    protected override string PrimaryKeyName => "pk_maintenance";
    protected override string UINameInFolder => "ui_name_folder_id_maintenance";
    protected override string WorkOrderTemplateForeignKey => "fk_maintenance_work_order_template";

    protected override void ConfigureDatabase(EntityTypeBuilder<Maintenance> builder)
    {
        builder.ToTable("maintenance", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.Property(x => x.Note).HasColumnName("note");
        builder.Property(x => x.FolderID).HasColumnName("maintenance_folder_id");
        builder.Property(x => x.WorkOrderTemplateID).HasColumnName("work_order_template_id");
        builder.Property(x => x.State).HasColumnName("state");
        builder.Property(x => x.Multiplicity).HasColumnName("multiplicity");
        builder.HasXminRowVersion(x => x.RowVersion);
    }
}