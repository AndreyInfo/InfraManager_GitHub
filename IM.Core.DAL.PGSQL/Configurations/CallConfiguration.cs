using IM.Core.DAL.Postgres;
using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class CallConfiguration : Core.CallConfiguration
    {
        protected override string PrimaryKeyName => "pk_call_non_clustered";
        protected override string PriorityForeignKeyName => "fk_call_priority";
        protected override string AccomplisherForeignKeyName => "fk_call_accomplisher_default_id_user";
        protected override string OwnerForeignKeyName => "fk_call_owner_default_id_user";
        protected override string InitiatorForeignKeyName => "fk_call_initiator_default_id_user";
        protected override string ExecutorForeignKeyName => "fk_call_executor_default_id_user";
        protected override string InfluenceForeignKeyName => "fk_call_influence";
        protected override string UrgencyForeignKeyName => "fk_call_urgency";
        protected override string ClientSubdivisionForeignKeyName => "fk_call_subdivision";
        protected override string CallTypeForeignKeyName => "fk_call_call_type";
        protected override string IncidentResultForeignKeyName => "fk_call_incident_result";
        protected override string RFSResultForeignKeyName => "fk_call_rfs_result";
        protected override string WorkflowSchemeForeignKeyName => "fk_call_workflow_scheme";
        protected override string CalendarWorkScheduleForeignKeyName => "fk_call_calendar_work_schedule";
        protected override string TimeZoneForeignKeyName => "fk_call_time_zone";
        protected override string CallServiceForeignKeyName => "fk_call_call_service";
        protected override string BudgetUsageForeignKeyName => "fk_call_budget_usage_cause_aggregate";
        protected override string BudgetUsageCauseForeignKeyName => "fk_call_call_budget_usage_aggregate";
        protected override string ClientForeignKeyName => "fk_call_im_obj_id_user";
        protected override string QueueForeignKeyName => "fk_call_queue";
        protected override string FormValuesForeignKeyName => "fk_call_form_values";

        protected override void ConfigureDatabase(EntityTypeBuilder<Call> builder)
        {
            builder.ToTable("call", Options.Scheme);

            builder.Property(x => x.IMObjID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(x => x.CallTypeID).HasColumnName("call_type_id");
            builder.Property(x => x.Number).HasColumnName("number");
            builder.Property(x => x.PriorityID).HasColumnName("priority_id");
            builder
                .Property(x => x.ReceiptType)
                .HasColumnType("smallint")
                .HasColumnName("receipt_type");
            builder.Property(x => x.InitiatorID).HasColumnName("initiator_id");
            builder.Property(x => x.ClientID).HasColumnName("client_id");
            builder.Property(x => x.ClientSubdivisionID).HasColumnName("client_subdivision_id");
            builder.Property(x => x.OwnerID).HasColumnName("owner_id");
            builder.Property(x => x.ExecutorID).HasColumnName("executor_id");
            builder.Property(x => x.QueueID).HasColumnName("queue_id");
            builder.Property(x => x.UrgencyID).HasColumnName("urgency_id");
            builder.Property(x => x.InfluenceID).HasColumnName("influence_id");
            builder
                .Property(x => x.UtcDateModified)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_modified");
            builder
                .Property(x => x.UtcDateRegistered)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_registered");
            builder
                .Property(x => x.UtcDateOpened)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_opened");
            builder
                .Property(x => x.UtcDatePromised)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_promised");
            builder
                .Property(x => x.UtcDateAccomplished)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_accomplished");
            builder
                .Property(x => x.UtcDateClosed)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_closed");
            builder.Property(x => x.EscalationCount).HasColumnName("escalation_count");
            builder.Property(x => x.AccomplisherID).HasColumnName("accomplisher_id");
            builder
                .Property(x => x.Grade)
                .HasColumnType("tinyint")
                .HasColumnName("grade");
            builder.Property(x => x.IncidentResultID).HasColumnName("incident_result_id");
            builder.Property(x => x.RequestForServiceResultID).HasColumnName("rfs_result_id");
            builder.Property(x => x.Removed).HasColumnName("removed");
            builder.Property(x => x.UserField1).HasColumnName("user_field1");
            builder.Property(x => x.UserField2).HasColumnName("user_field2");
            builder.Property(x => x.UserField3).HasColumnName("user_field3");
            builder.Property(x => x.UserField4).HasColumnName("user_field4");
            builder.Property(x => x.UserField5).HasColumnName("user_field5");
            builder.Property(x => x.SLAName).HasColumnName("sla_name");
            builder
                .Property(x => x.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            builder.Property(x => x.EntityStateID).HasColumnName("entity_state_id");
            builder.Property(x => x.EntityStateName).HasColumnName("entity_state_name");
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("workflow_scheme_id");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("workflow_scheme_version");
            builder.Property(x => x.CallSummaryName).HasColumnName("call_summary_name");
            builder.Property(x => x.CalendarWorkScheduleID).HasColumnName("calendar_work_schedule_id");
            builder.Property(x => x.TimeZoneID).HasColumnName("time_zone_id");
            builder.Property(x => x.HTMLDescription).HasColumnName("html_description");
            builder.Property(x => x.HTMLSolution).HasColumnName("html_solution");
            builder
                .Property(x => x.LineNumber)
                .HasColumnType("smallint")
                .HasColumnName("line_number");
            builder
                .Property(x => x.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            builder
                .Property(x => x.Solution)
                .HasColumnType("text")
                .HasColumnName("solution");
            builder.Property(x => x.ManhoursNormInMinutes).HasColumnName("manhours_norm_in_minutes");
            builder
                .Property(x => x.UtcDateCreated)
                .HasColumnType("timestamp")
                .HasColumnName("utc_date_created");
            builder
                .Property(x => x.OnWorkOrderExecutorControl)
                .HasColumnName("on_work_order_executor_control");
            builder.Property(x => x.ServicePlaceID).HasColumnName("service_place_id");
            builder.Property(x => x.ServicePlaceClassID).HasColumnName("service_place_class_id");
            builder
                .Property(x => x.CallServiceID)
                .HasColumnName("call_service_id")
                .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");
            builder.Property(x => x.BudgetUsageCauseAggregateID)
                .HasColumnName("budget_usage_cause_aggregate_id")
                .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");
            builder.Property(x => x.BudgetUsageAggregateID)
                .HasColumnName("budget_usage_aggregate_id")
                .HasDefaultValueSql("('00000000-0000-0000-0000-000000000000')");
            builder.Property(x => x.IsActive).HasColumnName("is_active");
            builder.Property(x => x.IsFinished).HasColumnName("is_finished");
            builder.Property(x => x.AccomplisherDefaultID).HasColumnName("accomplisher_default_id");
            builder.Property(x => x.OwnerDefaultID).HasColumnName("owner_default_id");
            builder.Property(x => x.InitiatorDefaultID).HasColumnName("initiator_default_id");
            builder.Property(x => x.ExecutorDefaultID).HasColumnName("executor_default_id");
            builder.Property(x => x.UrgencyDefaultID).HasColumnName("urgency_default_id");
            builder.Property(x => x.InfluenceDefaultID).HasColumnName("influence_default_id");
            builder.Property(x => x.ClientSubdivisionDefaultID).HasColumnName("client_subdivision_default_id");
            builder.Property(x => x.ManhoursInMinutes).HasColumnName("manhours_in_minutes");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormID).HasColumnName("form_id");
        }
    }
}