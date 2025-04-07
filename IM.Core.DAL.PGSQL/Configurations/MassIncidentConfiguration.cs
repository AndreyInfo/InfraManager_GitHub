using IM.Core.DAL.Postgres;
using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Postgres.Configurations
{
    internal class MassIncidentConfiguration : MassIncidentConfigurationBase
    {
        protected override string PrimaryKeyName => "pk_mass_incident";
        protected override string IMObjIDUniqueKeyName => "uk_mass_incident_im_obj_id";
        protected override string InformationChannelForeignKeyName => "fk_mass_incident_information_channel";
        protected override string CreatedByForeignKeyName => "fk_mass_incident_created_by";
        protected override string OwnedByForeignKeyName => "fk_mass_incident_owned_by";
        protected override string PriorityForeignKeyConstraintName => "fk_mass_incident_priority";
        protected override string OlaForeignKeyConstraintName => "fk_mass_incident_ola";
        protected override string TypeForeignKeyConstraintName => "fk_mass_incident_type";
        protected override string GroupForeignKeyConstraintName => "fk_mass_incident_group";
        protected override string CauseForeignKeyConstraintName => "fk_mass_incident_cause";
        protected override string CriticalityForeignKeyConstraintName => "fk_mass_incident_criticality";
        protected override string ExecutorForeignKeyName => "fk_mass_incident_executor_user_id";
        protected override string TechFailureCategoryForeignKeyName => "fk_mass_incident_tech_failure_category";
        protected override string ServiceForeignKeyName => "fk_mass_incident_service";
        protected override string FormValuesForeignKeyName => "fk_mass_incident_form_values";
        protected override string DescriptionColumnName => "description";
        protected override string DescriptionPlainColumnName => "description_plain";
        protected override string SolutionColumnName => "solution";
        protected override string SolutionPlainColumnName => "solution_plain";
        protected override string CauseColumnName => "cause";
        protected override string CausePlainColumnName => "cause_plain";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncident> builder)
        {
            builder.ToTable("mass_incident", Options.Scheme);

            builder.Property(x => x.CauseID).HasColumnName("cause_id");
            builder.Property(x => x.CriticalityID).HasColumnName("criticality_id");
            builder.Property(x => x.EntityStateID).HasColumnName("entity_state_id");
            builder.Property(x => x.EntityStateName).HasColumnName("entity_state_name");
            builder.Property(x => x.ExecutedByGroupID).HasColumnName("executed_by_group_id");
            builder.Property(x => x.ID).HasColumnName("id");
            builder.Property(x => x.IMObjID).HasColumnName("im_obj_id");
            builder.Property(x => x.InformationChannelID).HasColumnName("information_channel_id");
            builder.Property(x => x.Name).HasColumnName("name");
            builder.Property(x => x.PriorityID).HasColumnName("priority_id");           
            builder.Property(x => x.OperationalLevelAgreementID).HasColumnName("ola_id");
            builder.Property(x => x.TypeID).HasColumnName("type_id");
            builder.Property(x => x.UtcRegisteredAt).HasColumnName("utc_registered_at").HasColumnType("timestamp(3)");
            builder.Property(x => x.UtcCloseUntil).HasColumnName("utc_close_until").HasColumnType("timestamp(3)");
            builder.Property(x => x.UtcCreatedAt).HasColumnName("utc_created_at").HasColumnType("timestamp(3)"); 
            builder.Property(x => x.UtcDateModified).HasColumnName("utc_last_modified_at").HasColumnType("timestamp(3)");
            builder.Property(x => x.UtcOpenedAt).HasColumnName("utc_opened_at").HasColumnType("timestamp(3)");
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("workflow_scheme_id");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("workflow_scheme_identifier");
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("workflow_scheme_version");
            builder.Property(x => x.OwnedByUserID).HasColumnName("owned_by_user_id");
            builder.Property(x => x.CreatedByUserID).HasColumnName("created_by_user_id");
            builder.Property(x => x.ServiceID).HasColumnName("service_id");
            builder.Property(x => x.ExecutedByUserID).HasColumnName("executed_by_user_id");
            builder
                .Property(x => x.UtcDateClosed)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_closed");
            builder
                .Property(x => x.UtcDateAccomplished)
                .HasColumnType("timestamp(3)")
                .HasColumnName("utc_date_accomplished");
            builder.Property(x => x.TechnicalFailureCategoryID).HasColumnName("tech_failure_category_id");
            builder.Property(x => x.FormValuesID).HasColumnName("form_values_id");
            builder.Property(x => x.FormID).HasColumnName("form_id");

            builder.HasXminRowVersion(x => x.RowVersion);
        }
    }
}
