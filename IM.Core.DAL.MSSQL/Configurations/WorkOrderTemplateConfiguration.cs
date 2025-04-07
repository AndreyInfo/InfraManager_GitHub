using IM.Core.DAL.Microsoft.SqlServer;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderTemplateConfiguration : WorkOrderTemplateConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_WorkOrderTemplate";
        protected override string TypeForeignKeyName => "FK_WorkOrderTemplate_WorkOrderType";
        protected override string PriorityForeignKeyName => "FK_WorkOrderTemplate_WorkOrderPriority";
        protected override string FormForeignKey => "FK_WorkOrderTemplate_Form";
        protected override string FolderForeignKeyName => "FK_WorkOrderTemplate_WorkOrderTemplateFolder";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderTemplate> builder)
        {
            builder.ToTable("WorkOrderTemplate", Options.Scheme);

            builder.Property(x => x.DatePromisedDelta).HasColumnName("DatePromisedDelta");
            builder.Property(x => x.DateStartedDelta).HasColumnName("DateStartedDelta");
            builder.Property(x => x.Description).HasColumnName("Description");
            builder.Property(x => x.ExecutorAssignmentType).HasColumnName("ExecutorAssignmentType");
            builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
            builder.Property(x => x.FolderID).HasColumnName("WorkOrderTemplateFolderID");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
            builder.Property(x => x.ManhoursNormInMinutes).HasColumnName("ManhoursNormInMinutes");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.QueueID).HasColumnName("QueueID");
            builder.Property(x => x.UserField1).HasColumnName("UserField1");
            builder.Property(x => x.UserField2).HasColumnName("UserField2");
            builder.Property(x => x.UserField3).HasColumnName("UserField3");
            builder.Property(x => x.UserField4).HasColumnName("UserField4");
            builder.Property(x => x.UserField5).HasColumnName("UserField5");
            builder.Property(x => x.WorkOrderPriorityID).HasColumnName("WorkOrderPriorityID");
            builder.Property(x => x.WorkOrderTypeID).HasColumnName("WorkOrderTypeID");
            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
            builder.Property(x => x.FormID).HasColumnName("FormID");
        }
    }
}
