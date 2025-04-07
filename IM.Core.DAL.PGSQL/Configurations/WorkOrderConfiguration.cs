using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class WorkOrderConfiguration : WorkOrderConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_WorkOrder";
        protected override string TypeForeignKeyName => "fk_work_order_work_order_type";
        protected override string PriorityForeignKeyName => "fk_work_order_work_order_priority";
        protected override string AssigneeForeignKeyName => "fk_work_order_assignor_default_id_user";
        protected override string ExecutorForeignKeyName => "fk_work_order_executor_default_id_user";
        protected override string InitiatorForeignKeyName => "fk_work_order_initiator_default_id_User";
        protected override string QueueForeignKeyName => "fk_work_order_queue_default_id";
        protected override string WorkflowSchemeForeignKeyName => "fk_work_order_workflow_scheme";
        protected override string BudgetUsageCauseForeignKeyName => "fk_work_order_budget_usage_cause_aggregate";
        protected override string BudgetUsageForeignKeyName => "fk_work_order_call_budget_usage_aggregate";
        protected override string ReferenceForeignKeyName => "fk_work_order_reference_work_order";
        protected override string FormValuesForeignKeyName => "fk_work_order_form_values";
        protected override string ForeignKeyFinancePurchase => "fk_work_order_finance_purchase_work_order_id";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.ToTable("work_order", Options.Scheme);

            builder.Property(e => e.IMObjID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.Number).HasColumnName("number");
            builder.Property(e => e.Name).HasColumnName("name");

            builder.Property(e => e.TypeID).HasColumnName("work_order_type_id");

            builder.Property(e => e.PriorityID).HasColumnName("work_order_priority_id");

            builder.Property(e => e.AssigneeID).HasColumnName("assignor_id");
            builder.Property(e => e.AssigneeDefaultID).HasColumnName("assignor_default_id");
            builder.Property(e => e.InitiatorID).HasColumnName("initiator_id");
            builder.Property(e => e.InitiatorDefaultID).HasColumnName("initiator_default_id");
            builder.Property(e => e.ExecutorID).HasColumnName("executor_id");
            builder.Property(e => e.ExecutorDefaultID).HasColumnName("executor_default_id");
            builder.Property(e => e.QueueDefaultID).HasColumnName("queue_default_id");
            builder.Property(e => e.QueueID).HasColumnName("queue_id");
            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(e => e.UtcDateAccepted).HasColumnName("utc_date_accepted").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateAccomplished).HasColumnName("utc_date_accomplished")
                .HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateAssigned).HasColumnName("utc_date_assigned").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateCreated).HasColumnName("utc_date_created").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateModified).HasColumnName("utc_date_modified").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDatePromised).HasColumnName("utc_date_promised").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateStarted).HasColumnName("utc_date_started").HasColumnType("timestamp(3)");

            builder.Property(e => e.UserField1).HasColumnName("user_field1");
            builder.Property(e => e.UserField2).HasColumnName("user_field2");
            builder.Property(e => e.UserField3).HasColumnName("user_field3");
            builder.Property(e => e.UserField4).HasColumnName("user_field4");
            builder.Property(e => e.UserField5).HasColumnName("user_field5");

            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(e => e.EntityStateID).HasColumnName("entity_state_id");
            builder.Property(e => e.EntityStateName).HasColumnName("entity_state_name");
            builder.Property(e => e.WorkflowSchemeID).HasColumnName("workflow_scheme_id");
            builder.Property(e => e.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
            builder.Property(e => e.WorkflowSchemeVersion).HasColumnName("workflow_scheme_version");
            builder.Property(e => e.IsActive).HasColumnName("is_active");
            builder.Property(e => e.IsFinished).HasColumnName("is_finished");

            builder.Property(e => e.HTMLDescription).HasColumnName("html_description");
            builder.Property(e => e.Description).HasColumnName("description");
            builder.Property(e => e.ManhoursNormInMinutes).HasColumnName("manhours_norm_in_minutes");

            builder.Property(e => e.BudgetUsageAggregateID).HasColumnName("budget_usage_aggregate_id");
            builder.Property(e => e.BudgetUsageCauseAggregateID).HasColumnName("budget_usage_cause_aggregate_id");
            builder.Property(e => e.WorkOrderReferenceID).HasColumnName("work_order_reference_id");
            builder.Property(x => x.ManhoursInMinutes).HasColumnName("manhours_in_minutes");
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormID).HasColumnName("form_id");
        }
    }
}