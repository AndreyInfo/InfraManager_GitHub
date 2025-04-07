using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class MassIncidentConfiguration : MassIncidentConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_MassIncident";
        protected override string IMObjIDUniqueKeyName => "UK_MassIncident_IMObjID";
        protected override string InformationChannelForeignKeyName => "FK_MassIncident_InformationChannel";
        protected override string CreatedByForeignKeyName => "FK_MassIncident_CreatedBy";
        protected override string OwnedByForeignKeyName => "FK_MassIncident_OwnedBy";
        protected override string PriorityForeignKeyConstraintName => "FK_MassIncident_Priority";
        protected override string OlaForeignKeyConstraintName => "FK_MassiveIncident_OlaID";
        protected override string TypeForeignKeyConstraintName => "FK_MassIncident_Type";
        protected override string GroupForeignKeyConstraintName => "FK_MassIncident_Group";
        protected override string CauseForeignKeyConstraintName => "FK_MassIncident_Cause";
        protected override string CriticalityForeignKeyConstraintName => "FK_MassIncident_Criticality";
        protected override string ExecutorForeignKeyName => "FK_MassIncident_ExecutorID";
        protected override string TechFailureCategoryForeignKeyName => "FK_MassIncident_TechnicalFailureCategory";
        protected override string ServiceForeignKeyName => "FK_MassIncident_Service";
        protected override string FormValuesForeignKeyName => "FK_MassIncident_FormValues";
        protected override string DescriptionColumnName => "Description";
        protected override string DescriptionPlainColumnName => "DescriptionPlain";
        protected override string SolutionColumnName => "Solution";
        protected override string SolutionPlainColumnName => "SolutionPlain";
        protected override string CauseColumnName => "Cause";
        protected override string CausePlainColumnName => "CausePlain";

        protected override void ConfigureDataProvider(EntityTypeBuilder<MassIncident> builder)
        {
            builder.ToTable("MassIncident", "dbo");

            builder.Property(x => x.CauseID).HasColumnName("CauseID");
            builder.Property(x => x.CriticalityID).HasColumnName("CriticalityID");
            builder.Property(x => x.EntityStateID).HasColumnName("EntityStateID");
            builder.Property(x => x.EntityStateName).HasColumnName("EntityStateName");
            builder.Property(x => x.ExecutedByGroupID).HasColumnName("ExecutedByGroupID");
            builder.Property(x => x.ID).HasColumnName("ID");
            builder.Property(x => x.IMObjID).HasColumnName("IMObjID");
            builder.Property(x => x.InformationChannelID).HasColumnName("InformationChannelID");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.PriorityID).HasColumnName("PriorityID");
            builder.Property(x => x.RowVersion).IsRowVersion().HasColumnName("RowVersion");
            builder.Property(x => x.OperationalLevelAgreementID).HasColumnName("OlaID");
            builder.Property(x => x.TypeID).HasColumnName("TypeID");
            builder.Property(x => x.UtcRegisteredAt).HasColumnName("UtcRegisteredAt").HasColumnType("datetime");
            builder.Property(x => x.UtcCloseUntil).HasColumnName("UtcCloseUntil").HasColumnType("datetime");
            builder.Property(x => x.UtcCreatedAt).HasColumnName("UtcCreatedAt").HasColumnType("datetime"); 
            builder.Property(x => x.UtcDateModified).HasColumnName("UtcLastModifiedAt").HasColumnType("datetime");
            builder.Property(x => x.UtcOpenedAt).HasColumnName("UtcOpenedAt").HasColumnType("datetime");
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("WorkflowSchemeID");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("WorkflowSchemeVersion");
            builder.Property(x => x.OwnedByUserID).HasColumnName("OwnedByUserID");
            builder.Property(x => x.CreatedByUserID).HasColumnName("CreatedByUserID");
            builder.Property(x => x.ServiceID).HasColumnName("ServiceID");
            builder.Property(x => x.UtcDateClosed).HasColumnName("UtcDateClosed").HasColumnType("datetime");
            builder.Property(x => x.UtcDateAccomplished).HasColumnName("UtcDateAccomplished").HasColumnType("datetime");
            builder.Property(x => x.ExecutedByUserID).HasColumnName("ExecutedByUserID");
            builder.Property(x => x.TechnicalFailureCategoryID).HasColumnName("TechnicalFailureCategoryID");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
            builder.Property(x => x.FormID).HasColumnName("FormID");
        }
    }
}
