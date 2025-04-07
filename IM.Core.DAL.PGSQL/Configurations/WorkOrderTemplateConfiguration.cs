using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.WorkOrders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkOrderTemplateConfiguration : WorkOrderTemplateConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_work_order_template";
        protected override string TypeForeignKeyName => "fk_work_order_template_work_order_type";
        protected override string PriorityForeignKeyName => "fk_work_order_template_work_order_priority";
        protected override string FormForeignKey => "fk_work_order_template_form";
        protected override string FolderForeignKeyName => "fk_work_order_template_work_order_template_folder";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrderTemplate> builder)
        {
            builder.ToTable("work_order_template", Options.Scheme);

            builder.Property(x => x.DatePromisedDelta).HasColumnName("date_promised_delta");
            builder.Property(x => x.DateStartedDelta).HasColumnName("date_started_delta");
            builder.Property(x => x.Description).HasColumnName("description");
            builder.Property(x => x.ExecutorAssignmentType).HasColumnName("executor_assignment_type");
            builder.Property(x => x.ExecutorID).HasColumnName("executor_id");
            builder.Property(x => x.FolderID).HasColumnName("work_order_template_folder_id");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.InitiatorID).HasColumnName("initiator_id");
            builder.Property(x => x.ManhoursNormInMinutes).HasColumnName("manhours_norm_in_minutes");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.QueueID).HasColumnName("queue_id");
            builder.Property(x => x.UserField1).HasColumnName("user_field1");
            builder.Property(x => x.UserField2).HasColumnName("user_field2");
            builder.Property(x => x.UserField3).HasColumnName("user_field3");
            builder.Property(x => x.UserField4).HasColumnName("user_field4");
            builder.Property(x => x.UserField5).HasColumnName("user_field5");
            builder.Property(x => x.WorkOrderPriorityID).HasColumnName("work_order_priority_id");
            builder.Property(x => x.WorkOrderTypeID).HasColumnName("work_order_type_id");
            builder.HasXminRowVersion(x => x.RowVersion);
            builder.Property(x => x.FormID).HasColumnName("form_id");
        }
    }
}