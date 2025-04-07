using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;

namespace IM.Core.DAL.Postgres.Configurations;

internal class WorkOrderTemplateFolderConfiguration : WorkOrderTemplateFolderConfigurationBase
{
    protected override string ForeignKeySubFolders => "fk_work_order_template_folder_work_order_template_folder";

    protected override string ForeignKeyTemplates => "fk_work_order_template_work_order_template_folder";

    protected override string KeyName => "pk_work_order_template_folder";

    protected override string DefaultValueID => "gen_random_uuid()";


    protected override void ConfigureDataBase(EntityTypeBuilder<WorkOrderTemplateFolder> builder)
    {
        builder.ToTable("work_order_template_folder", Options.Scheme);

        builder.Property(x => x.ID).HasColumnName("id");
        builder.Property(x => x.ParentID).HasColumnName("parent_id");
        builder.Property(x => x.Name).HasColumnName("name");
        builder.HasXminRowVersion(e => e.RowVersion);
    }
}