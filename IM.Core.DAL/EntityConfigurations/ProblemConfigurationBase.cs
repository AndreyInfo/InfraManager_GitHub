using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class ProblemConfigurationBase : IEntityTypeConfiguration<Problem>
    {
        public void Configure(EntityTypeBuilder<Problem> builder)
        {
            builder.HasKey(x => x.IMObjID);

            builder.Property(x => x.IMObjID).ValueGeneratedNever();
            builder.Property(x => x.Number).IsRequired(true).ValueGeneratedOnAdd();
            builder.Property(x => x.Summary).HasMaxLength(500);
            builder.Property(x => x.UserField1).HasMaxLength(250);
            builder.Property(x => x.UserField2).HasMaxLength(250);
            builder.Property(x => x.UserField3).HasMaxLength(250);
            builder.Property(x => x.UserField4).HasMaxLength(250);
            builder.Property(x => x.UserField5).HasMaxLength(250);
            builder.Property(x => x.EntityStateID).HasMaxLength(50);
            builder.Property(x => x.EntityStateName).HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).HasMaxLength(50);

            builder.Ignore(x => x.TargetEntityStateID);

            ConfigureDatabase(builder);

            builder.HasOne(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.UrgencyId)
                .HasConstraintName(UrgencyForeignKey);

            builder.HasOne(x => x.Influence)
                .WithMany()
                .HasForeignKey(x => x.InfluenceId)
                .HasConstraintName(InfluenceForeignKey);

            builder.HasOne(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName(PriorityForeignKey);

            builder.HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeID)
                .HasConstraintName(TypeForeignKey);

            builder.HasOne(x => x.ProblemCause)
                .WithMany()
                .HasForeignKey(x => x.ProblemCauseId)
                .HasConstraintName(ProblemCauseForeignKey);

            builder.HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerID)
                .HasPrincipalKey(x => x.IMObjID); // TODO: Надо создать FK в БД

            builder.HasMany(x => x.Negotiations)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasMany(x => x.Manhours)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasMany(x => x.WorkOrderReferences)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasMany(x => x.CallReferences)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasMany(x => x.Notes)
                .WithOne()
                .HasForeignKey(x => x.ParentObjectID);

            builder.Navigation(x => x.Type).IsRequired();

            builder.HasOne(x => x.FormValues)
                .WithMany()
                .HasForeignKey(x => new { x.FormValuesID, x.FormID, })
                .HasPrincipalKey(x => new { FormValuesID = x.ID, FormID = x.FormBuilderFormID, })
                .HasConstraintName(FormValuesForeignKeyName);

            builder.HasOne(x => x.Initiator)
                .WithMany()
                .HasForeignKey(x => x.InitiatorID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(InitiatorForeignKeyName)
                .IsRequired(false);

            builder.HasOne(x => x.Queue)
                .WithMany()
                .HasForeignKey(x => x.QueueID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(QueueForeignKeyName)
                .IsRequired(false);

            builder.HasOne(x => x.Executor)
                .WithMany()
                .HasForeignKey(x => x.ExecutorID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(ExecutorForeignKeyName)
                .IsRequired(false);

            builder.HasOne(x => x.Service)
                .WithMany()
                .HasForeignKey(x => x.ServiceID)
                .HasPrincipalKey(x => x.ID)
                .HasConstraintName(ServiceForeignKeyName)
                .IsRequired(false);
        }

        protected abstract string PrimaryKeyName { get; }
        protected abstract string UrgencyForeignKey { get; }
        protected abstract string InfluenceForeignKey { get; }
        protected abstract string PriorityForeignKey { get; }
        protected abstract string TypeForeignKey { get; }
        protected abstract string ProblemCauseForeignKey { get; }
        protected abstract string FormValuesForeignKeyName { get; }
        protected abstract string InitiatorForeignKeyName { get; }
        protected abstract string QueueForeignKeyName { get; }
        protected abstract string ExecutorForeignKeyName { get; }
        protected abstract string ServiceForeignKeyName { get; }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Problem> builder);
    }
}
