using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class ProblemConfiguration : ProblemConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_Problem";
        protected override string UrgencyForeignKey => "FK_Problem_Urgency";
        protected override string InfluenceForeignKey => "FK_Problem_Influence";
        protected override string PriorityForeignKey => "FK_Problem_Priority";
        protected override string TypeForeignKey => "FK_Problem_ProblemType";
        protected override string ProblemCauseForeignKey => "FK_Problem_ProblemCause";
        protected override string FormValuesForeignKeyName => "FK_Problem_FormValues";
        protected override string InitiatorForeignKeyName => "FK_Problem_Initiator";
        protected override string QueueForeignKeyName => "FK_Problem_Queue";
        protected override string ExecutorForeignKeyName => "FK_Problem_Executor";
        protected override string ServiceForeignKeyName => "FK_Problem_Service";

        protected override void ConfigureDatabase(EntityTypeBuilder<Problem> builder)
        {
            builder.ToTable("Problem", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.Number).HasColumnName("Number");
            builder.Property(x => x.Summary).HasColumnName("Summary");
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("Text");
            builder.Property(x => x.Solution).HasColumnName("Solution").HasColumnType("Text");
            builder.Property(x => x.Fix).HasColumnName("Fix").HasColumnType("Text");
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("Text");
            builder.Property(x => x.Cause).HasColumnName("Cause").HasColumnType("Text");
            builder.Property(x => x.UserField1).HasColumnName("UserField1");
            builder.Property(x => x.UserField2).HasColumnName("UserField2");
            builder.Property(x => x.UserField3).HasColumnName("UserField3");
            builder.Property(x => x.UserField4).HasColumnName("UserField4");
            builder.Property(x => x.UserField5).HasColumnName("UserField5");
            builder.Property(x => x.UtcDateSolved).HasColumnName("UtcDateSolved").HasColumnType("datetime");
            builder.Property(x => x.UtcDateClosed).HasColumnName("UtcDateClosed").HasColumnType("datetime");
            builder.Property(x => x.UtcDateDetected).HasColumnName("UtcDateDetected").HasColumnType("datetime");
            builder.Property(x => x.UtcDateModified).HasColumnName("UtcDateModified").HasColumnType("datetime");
            builder.Property(x => x.UtcDatePromised).HasColumnName("UtcDatePromised").HasColumnType("datetime");
            builder.Property(x => x.EntityStateID).HasColumnName("EntityStateID");
            builder.Property(x => x.EntityStateName).HasColumnName("EntityStateName");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.RowVersion).HasColumnName("RowVersion").HasColumnType("timestamp").IsRowVersion();
            builder.Property(x => x.UrgencyId).HasColumnName("UrgencyID");
            builder.Property(x => x.InfluenceId).HasColumnName("InfluenceID");
            builder.Property(x => x.PriorityID).HasColumnName("PriorityID");
            builder.Property(x => x.TypeID).HasColumnName("ProblemTypeID");
            builder.Property(x => x.OwnerID).HasColumnName("OwnerID");
            builder.Property(x => x.ProblemCauseId).HasColumnName("ProblemCauseID");
            builder.Property(x => x.ManhoursInMinutes).HasColumnName("ManhoursInMinutes");

            builder.HasMany(x => x.Dependencies)
                .WithOne()
                .HasForeignKey(x => x.OwnerObjectID)
                .HasConstraintName("FK_Problem_ProblemDependency");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");

            builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
            builder.Property(x => x.QueueID).HasColumnName("QueueID");
            builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
            builder.Property(x => x.ServiceID).HasColumnName("ServiceID");
        }
    }
}
