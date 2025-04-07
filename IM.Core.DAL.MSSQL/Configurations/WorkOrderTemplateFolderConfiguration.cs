using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.EntityConfigurations;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations;

internal class WorkOrderTemplateFolderConfiguration : WorkOrderTemplateFolderConfigurationBase
{
    protected override string ForeignKeySubFolders => "FK_WorkOrderTemplateFolder_WorkOrderTemplateFolder";

    protected override string ForeignKeyTemplates => "FK_WorkOrderTemplate_WorkOrderTemplateFolder";

    protected override string KeyName => "PK_WorkOrderTemplateFolder";

    protected override string DefaultValueID => "NEWID()";

    protected override void ConfigureDataBase(EntityTypeBuilder<WorkOrderTemplateFolder> builder)
    {
        builder.ToTable("WorkOrderTemplateFolder", "dbo");

        builder.Property(c => c.ID).HasColumnName("ID");
        builder.Property(c => c.Name).HasColumnName("Name");
        builder.Property(c => c.ParentID).HasColumnName("ParentID");
        builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
    }
}
