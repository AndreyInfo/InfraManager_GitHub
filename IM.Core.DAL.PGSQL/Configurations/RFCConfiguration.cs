using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.Postgres;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IM.Core.DAL.PGSQL.Configurations
{
    internal class RFCConfiguration : RfcConfigurationBase
    {
        protected override string ServiceForeignKey => "fk_rfc_service";
        protected override string PriorityForeignKey => "fk_rfc_priority";
        protected override string TypeForeignKey => "fk_rfc_rfc_type";
        protected override string FormValuesForeignKey => "fk_rfc_form_values";
        protected override string UrgencyForeignKey => "fk_rfc_urgency";
        protected override string InfluenceForeignKey => "fk_rfc_influence";
        protected override string OwnerForeignKey => "fk_rfc_owner";
        protected override string InitiatorForeignKey => "fk_rfc_initiator";
        protected override string FormForeignKey => "fk_rfc_form";

        protected override void ConfigureDatabase(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder.ToTable("rfc", Options.Scheme);
            builder.HasKey(x => x.IMObjID).HasName("pk_rfc");

            ConfigureColumns(builder);
        }

        private void ConfigureColumns(EntityTypeBuilder<ChangeRequest> builder)
        {
            builder.Property(x => x.IMObjID)
                .HasColumnName("id");

            builder.Property(x => x.Number)
                .HasColumnName("number");

            builder.Property(x => x.UrgencyID)
                .HasColumnName("urgency_id");

            builder.Property(x => x.InfluenceID)
                .HasColumnName("influence_id");

            builder.Property(x => x.ServiceName)
                .HasColumnName("service_name")
                .HasMaxLength(250);

            builder.Property(x => x.Summary)
                .HasColumnName("summary");

            builder.Property(x => x.UtcDateDetected)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_detected");

            builder.Property(x => x.UtcDatePromised)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_promised");

            builder.Property(x => x.UtcDateClosed)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_closed");

            builder.Property(x => x.UtcDateSolved)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_solved");

            builder.Property(x => x.UtcDateModified)
                .IsRequired()
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_modified");

            builder.Property(x => x.UtcDateStarted)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_started");

            builder.Property(x => x.HTMLDescription)
                .HasColumnName("html_description");

            builder.Property(x => x.Description)
                .HasColumnType("text")
                .HasColumnName("description");

            builder.Property(x => x.Target)
                .HasColumnName("target");

            builder.Property(x => x.InitiatorID)
                .HasColumnName("initiator_id");

            builder.Property(x => x.OwnerID)
                .HasColumnName("owner_id");

            builder.HasXminRowVersion(e => e.RowVersion);

            builder.Property(x => x.ReasonObjectID)
                .HasColumnName("reason_object_id");

            builder.Property(x => x.ReasonObjectClassID)
                .HasColumnName("reason_object_class_id");

            builder.Property(x => x.FundingAmount)
                .HasColumnName("funding_amount");

            builder.Property(x => x.RealizationDocumentID)
                .HasColumnName("realization_document_id");

            builder.Property(x => x.RollbackDocumentID)
                .HasColumnName("rollback_document_id");

            builder.Property(x => x.CategoryID)
                .HasColumnName("category_id");

            builder.Property(x => x.ManhoursNormInMinutes)
                .HasColumnName("manhours_norm_in_minutes");

            builder.Property(x => x.EntityStateID)
                .HasColumnName("entity_state_id");

            builder.Property(x => x.EntityStateName)
                .HasColumnName("entity_state_name");

            builder.Property(x => x.WorkflowSchemeID)
                .HasColumnName("workflow_scheme_id");

            builder.Property(x => x.WorkflowSchemeIdentifier)
                .HasColumnName("workflow_scheme_identifier");

            builder.Property(x => x.WorkflowSchemeVersion)
                .HasColumnName("workflow_scheme_version");

            builder.Property(x => x.OnWorkOrderExecutorControl)
                .HasColumnName("on_work_order_executor_control");

            builder.Property(x => x.InRealization)
                .HasColumnName("in_realization");

            builder.Property(x => x.QueueID)
                .HasColumnName("queue_id");

            builder.Property(x => x.RFCTypeID)
                .HasColumnName("rfc_type_id");

            builder.Property(x => x.ServiceID)
                .HasColumnName("service_id");

            builder.Property(x => x.PriorityID)
                .HasColumnName("priority_id");

            builder.Property(x => x.ManhoursInMinutes).HasColumnName("manhours_in_minutes");
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormID).HasColumnName("form_id");
        }
    }
}