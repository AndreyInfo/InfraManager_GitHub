using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class MassIncidentConfigurationBase : IEntityTypeConfiguration<MassIncident>
    {
        public void Configure(EntityTypeBuilder<MassIncident> builder)
        {
            builder.HasKey(x => x.ID).HasName(PrimaryKeyName);
            builder.HasIndex(x => x.IMObjID).HasDatabaseName(IMObjIDUniqueKeyName);

            builder.Property(x => x.ID).ValueGeneratedOnAdd();
            builder.Property(x => x.IMObjID).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(255).IsRequired(true);

            ConfigureDescription(
                builder.WithDescription(x => x.Description),
                DescriptionColumnName,
                DescriptionPlainColumnName)
                .WithOwner();
            ConfigureDescription(
                builder.WithDescription(x => x.Solution),
                SolutionColumnName,
                SolutionPlainColumnName)
                .WithOwner();
            ConfigureDescription(
                builder.WithDescription(x => x.Cause),
                CauseColumnName,
                CausePlainColumnName);

            builder.Property(x => x.EntityStateID).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.EntityStateName).IsRequired(false).HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeIdentifier).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).IsRequired(false).HasMaxLength(50);
            builder.Ignore(x => x.TargetEntityStateID);

            ConfigureDataProvider(builder);

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceID)
                .HasConstraintName(ServiceForeignKeyName)
                .IsRequired(true);

            builder.HasOne(x => x.MassIncidentInformationChannel)
                .WithMany()
                .HasForeignKey(x => x.InformationChannelID)
                .HasConstraintName(InformationChannelForeignKeyName);

            builder.HasOne(x => x.CreatedBy)
                .WithMany()
                .HasForeignKey(x => x.CreatedByUserID)
                .HasConstraintName(CreatedByForeignKeyName)
                .IsRequired(true);

            builder.HasOne(x => x.OwnedBy)
                .WithMany()
                .HasForeignKey(x => x.OwnedByUserID)
                .HasConstraintName(OwnedByForeignKeyName)
                .IsRequired(false);

            builder.HasOne(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName(PriorityForeignKeyConstraintName);

            builder.HasOne<OperationalLevelAgreement>()
                .WithMany()
                .HasForeignKey(x => x.OperationalLevelAgreementID)
                .HasConstraintName(OlaForeignKeyConstraintName);

            builder.HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeID)
                .HasConstraintName(TypeForeignKeyConstraintName);

            builder.HasOne(x => x.ExecutedByGroup)
                .WithMany()
                .HasForeignKey(x => x.ExecutedByGroupID)
                .HasConstraintName(GroupForeignKeyConstraintName);

            builder.HasOne(x => x.MassIncidentCause)
                .WithMany()
                .HasForeignKey(x => x.CauseID)
                .HasConstraintName(CauseForeignKeyConstraintName);

            builder.HasOne(x => x.Criticality)
                .WithMany()
                .HasForeignKey(x => x.CriticalityID)
                .HasConstraintName(CriticalityForeignKeyConstraintName);
            
            builder.HasOne(x => x.ExecutedByUser)
                .WithMany()
                .HasForeignKey(x => x.ExecutedByUserID)
                .HasConstraintName(ExecutorForeignKeyName);

            builder.HasOne(x => x.TechnicalFailureCategory)
                .WithMany()
                .HasForeignKey(x => x.TechnicalFailureCategoryID)
                .HasConstraintName(TechFailureCategoryForeignKeyName);
            
            builder.HasOne(x => x.FormValues)
                .WithMany()
                .HasForeignKey(x => new { x.FormValuesID, x.FormID, })
                .HasPrincipalKey(x => new { FormValuesID = x.ID, FormID = x.FormBuilderFormID, })
                .HasConstraintName(FormValuesForeignKeyName);
        }

        protected abstract string PrimaryKeyName { get; }
        protected abstract string IMObjIDUniqueKeyName { get; }
        protected abstract string InformationChannelForeignKeyName { get; }
        protected abstract string CreatedByForeignKeyName { get; }
        protected abstract string OwnedByForeignKeyName { get; }
        protected abstract string PriorityForeignKeyConstraintName { get; }
        protected abstract string OlaForeignKeyConstraintName { get; }
        protected abstract string TypeForeignKeyConstraintName { get; }
        protected abstract string GroupForeignKeyConstraintName { get; }
        protected abstract string CauseForeignKeyConstraintName { get; }
        protected abstract string CriticalityForeignKeyConstraintName { get; }
        protected abstract string ExecutorForeignKeyName { get; }
        protected abstract string TechFailureCategoryForeignKeyName { get; }
        protected abstract string ServiceForeignKeyName { get; }
        protected abstract string FormValuesForeignKeyName { get; }
        protected abstract string DescriptionColumnName { get; }
        protected abstract string DescriptionPlainColumnName { get; }
        protected abstract string SolutionColumnName { get; }
        protected abstract string SolutionPlainColumnName { get; }
        protected abstract string CauseColumnName { get; }
        protected abstract string CausePlainColumnName { get; }

        protected abstract void ConfigureDataProvider(EntityTypeBuilder<MassIncident> builder);
        private OwnedNavigationBuilder<MassIncident, Description> ConfigureDescription(
            OwnedNavigationBuilder<MassIncident, Description> builder,
            string columnName,
            string plainColumnName)
        {
            builder
                .Property(x => x.Plain)
                .HasColumnName(columnName)
                .HasMaxLength(Description.PlainMaxLength);
            builder
                .Property(x => x.Formatted)
                .HasColumnName(plainColumnName)
                .HasMaxLength(Description.FormattedmaxLength);

            return builder;
        }
    }
}
