using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class ProblemConfiguration : ProblemConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_problem";
        protected override string UrgencyForeignKey => "fk_problem_urgency";
        protected override string InfluenceForeignKey => "fk_problem_influence";
        protected override string PriorityForeignKey => "fk_problem_priority";
        protected override string TypeForeignKey => "fk_problem_problem_type";
        protected override string ProblemCauseForeignKey => "fk_problem_problem_cause";
        protected override string FormValuesForeignKeyName => "fk_problem_form_values";
        protected override string InitiatorForeignKeyName => "fk_problem_initiator";
        protected override string QueueForeignKeyName => "fk_problem_queue";
        protected override string ExecutorForeignKeyName => "fk_problem_executor";
        protected override string ServiceForeignKeyName => "fr_problem_service";

        protected override void ConfigureDatabase(EntityTypeBuilder<Problem> builder)
        {
            builder.ToTable("problem", Options.Scheme);
            builder.Property(x => x.IMObjID).HasColumnName("id").HasColumnType("uuid");
            builder.Property(e => e.Cause).HasColumnName("cause").HasColumnType("Text");
            builder.Property(e => e.Description).HasColumnName("description").HasColumnType("Text");
            builder.Property(e => e.EntityStateID).HasColumnName("entity_state_id");
            builder.Property(e => e.EntityStateName).HasColumnName("entity_state_name");
            builder.Property(e => e.Fix).HasColumnName("fix").HasColumnType("Text");
            builder.Property(e => e.HTMLCause).HasColumnName("html_cause");
            builder.Property(e => e.HTMLDescription).HasColumnName("html_description");
            builder.Property(e => e.HTMLFix).HasColumnName("html_fix");
            builder.Property(e => e.HTMLSolution).HasColumnName("html_solution");
            builder.Property(e => e.IMObjID).HasColumnName("id");
            builder.Property(e => e.ManhoursNormInMinutes).HasColumnName("manhours_norm_in_minutes");
            builder.Property(e => e.Number).HasColumnName("number");
            builder.Property(e => e.OnWorkOrderExecutorControl).HasColumnName("on_work_order_executor_control");
            builder.Property(e => e.OwnerID).HasColumnName("owner_id");
            builder.HasXminRowVersion(e => e.RowVersion);
            builder.Property(e => e.Solution).HasColumnName("solution").HasColumnType("Text");
            builder.Property(e => e.Summary).HasColumnName("summary");
            builder.Property(e => e.UserField1).HasColumnName("user_field1");
            builder.Property(e => e.UserField2).HasColumnName("user_field2");
            builder.Property(e => e.UserField3).HasColumnName("user_field3");
            builder.Property(e => e.UserField4).HasColumnName("user_field4");
            builder.Property(e => e.UserField5).HasColumnName("user_field5");
            builder.Property(e => e.UtcDateClosed).HasColumnName("utc_date_closed").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateDetected).HasColumnName("utc_date_detected").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateModified).HasColumnName("utc_date_modified").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDatePromised).HasColumnName("utc_date_promised").HasColumnType("timestamp(3)");
            builder.Property(e => e.UtcDateSolved).HasColumnName("utc_date_solved").HasColumnType("timestamp(3)");
            builder.Property(e => e.WorkflowSchemeID).HasColumnName("workflow_scheme_id");
            builder.Property(e => e.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
            builder.Property(e => e.WorkflowSchemeVersion).HasColumnName("workflow_scheme_version");
            builder.Property(e => e.UrgencyId).HasColumnName("urgency_id");
            builder.Property(e => e.InfluenceId).HasColumnName("influence_id");
            builder.Property(e => e.PriorityID).HasColumnName("priority_id");
            builder.Property(e => e.TypeID).HasColumnName("problem_type_id");
            builder.Property(e => e.ProblemCauseId).HasColumnName("problem_cause_id");
            builder.Property(x => x.ManhoursInMinutes).HasColumnName("manhours_in_minutes");

            builder.HasMany(x => x.Dependencies)
                .WithOne()
                .HasForeignKey(x => x.OwnerObjectID)
                .HasConstraintName("fk_problem_problem_dependency");
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormID).HasColumnName("form_id");

            builder.Property(x => x.InitiatorID).HasColumnName("initiator_id");
            builder.Property(x => x.QueueID).HasColumnName("queue_id");
            builder.Property(x => x.ExecutorID).HasColumnName("executor_id");
            builder.Property(x => x.ServiceID).HasColumnName("service_id");

        }
    }
}