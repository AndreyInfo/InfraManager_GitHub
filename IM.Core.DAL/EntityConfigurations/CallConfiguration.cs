using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Calls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class CallConfiguration : IEntityTypeConfiguration<Call>
    {
        public void Configure(EntityTypeBuilder<Call> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.Number).ValueGeneratedOnAdd();
            builder.Property(x => x.IsFinished).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.IsActive)
                .HasConversion<int>(val => val ? 1 : 0, dbVal => dbVal > 0)
                .ValueGeneratedOnAddOrUpdate();

            builder.Property(x => x.AccomplisherDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.OwnerDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.InitiatorDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.ExecutorDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.UrgencyDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.InfluenceDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.ClientSubdivisionDefaultID).ValueGeneratedOnAddOrUpdate();

            builder
                .Property(x => x.UserField1)
                .IsRequired(true)
                .HasMaxLength(250);
            builder
                .Property(x => x.UserField2)
                .IsRequired(true)
                .HasMaxLength(250);
            builder
                .Property(x => x.UserField3)
                .IsRequired(true)
                .HasMaxLength(250);
            builder
                .Property(x => x.UserField4)
                .IsRequired(true)
                .HasMaxLength(250);
            builder
                .Property(x => x.UserField5)
                .IsRequired(true)
                .HasMaxLength(250);

            builder.Property(x => x.SLAName).HasMaxLength(250);
            builder.Property(x => x.EntityStateID).HasMaxLength(50);
            builder.Property(x => x.EntityStateName).HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).HasMaxLength(50);
            builder.Property(x => x.CallSummaryName).HasMaxLength(250).IsRequired(true);
            builder.Property(x => x.TimeZoneID).HasMaxLength(250);
            builder.Property(x => x.HTMLDescription).IsRequired(true);
            builder.Property(x => x.HTMLSolution).IsRequired(true);

            builder.IsMarkableForDelete();
            builder.Ignore(x => x.TargetEntityStateID);

            builder
                .HasOne(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName(PriorityForeignKeyName);

            builder
                .HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientID)
                .HasConstraintName(ClientForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder
                .HasOne(x => x.Accomplisher)
                .WithMany()
                .HasForeignKey(x => x.AccomplisherDefaultID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(AccomplisherForeignKeyName);

            builder
                .HasOne(x => x.Owner)
                .WithMany()
                .HasForeignKey(x => x.OwnerDefaultID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(OwnerForeignKeyName);

            builder
                .HasOne(x => x.Initiator)
                .WithMany()
                .HasForeignKey(x => x.InitiatorDefaultID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(InitiatorForeignKeyName);

            builder
                .HasOne(x => x.Executor)
                .WithMany()
                .HasForeignKey(x => x.ExecutorDefaultID)
                .HasPrincipalKey(x => x.IMObjID)
                .HasConstraintName(ExecutorForeignKeyName);

            builder
                .HasOne(x => x.Queue)
                .WithMany()
                .HasForeignKey(x => x.QueueID)
                .HasConstraintName(QueueForeignKeyName);

            builder
                .HasOne(x => x.Influence)
                .WithMany()
                .HasForeignKey(x => x.InfluenceDefaultID)
                .HasConstraintName(InfluenceForeignKeyName);

            builder
                .HasOne(x => x.Urgency)
                .WithMany()
                .HasForeignKey(x => x.UrgencyDefaultID)
                .HasConstraintName(UrgencyForeignKeyName);

            builder
                .HasOne(x => x.ClientSubdivision)
                .WithMany()
                .HasForeignKey(x => x.ClientSubdivisionDefaultID)
                .HasConstraintName(ClientSubdivisionForeignKeyName);

            builder
                .HasOne(x => x.CallType)
                .WithMany()
                .HasForeignKey(x => x.CallTypeID)
                .HasConstraintName(CallTypeForeignKeyName);

            builder
                .HasOne(x => x.IncidentResult)
                .WithMany()
                .HasForeignKey(x => x.IncidentResultID)
                .HasConstraintName(IncidentResultForeignKeyName);

            builder
                .HasOne(x => x.RequestForServiceResult)
                .WithMany()
                .HasForeignKey(x => x.RequestForServiceResultID)
                .HasConstraintName(RFSResultForeignKeyName);

            builder
                .HasOne(x => x.WorkflowScheme)
                .WithMany()
                .HasForeignKey(x => x.WorkflowSchemeID)
                .HasConstraintName(WorkflowSchemeForeignKeyName);

            builder
                .HasOne(x => x.CalendarWorkSchedule)
                .WithMany()
                .HasForeignKey(x => x.CalendarWorkScheduleID)
                .HasConstraintName(CalendarWorkScheduleForeignKeyName);

            builder
                .HasOne(x => x.TimeZone)
                .WithMany()
                .HasForeignKey(x => x.TimeZoneID)
                .HasConstraintName(TimeZoneForeignKeyName);

            builder
                .HasOne(x => x.CallService)
                .WithMany()
                .HasForeignKey(x => x.CallServiceID)
                .HasConstraintName(CallServiceForeignKeyName);

            builder
                .HasOne(x => x.BudgetUsageAggregate)
                .WithMany()
                .HasForeignKey(x => x.BudgetUsageAggregateID)
                .HasConstraintName(BudgetUsageForeignKeyName);

            builder
                .HasOne(x => x.BudgetUsageCauseAggregate)
                .WithMany()
                .HasForeignKey(x => x.BudgetUsageCauseAggregateID)
                .HasConstraintName(BudgetUsageCauseForeignKeyName);

            builder
                .HasOne(x => x.Aggregate)
                .WithOne()
                .HasForeignKey<CallAggregate>(x => x.CallID);

            builder
                .HasMany(x => x.Manhours)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder
                .HasOne(x => x.FormValues)
                .WithMany()
                .HasForeignKey(x => new { x.FormValuesID, x.FormID, })
                .HasPrincipalKey(x => new { FormValuesID = x.ID, FormID = x.FormBuilderFormID, })
                .HasConstraintName(FormValuesForeignKeyName);

            builder.HasMany(x => x.WorkOrderReferences).WithOne().HasForeignKey(x => x.ObjectID);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<Call> builder);

        protected abstract string PrimaryKeyName { get; }
        protected abstract string PriorityForeignKeyName { get; }
        protected abstract string ClientForeignKeyName { get; }
        protected abstract string AccomplisherForeignKeyName { get; }
        protected abstract string OwnerForeignKeyName { get; }
        protected abstract string InitiatorForeignKeyName { get; }
        protected abstract string ExecutorForeignKeyName { get; }
        protected abstract string InfluenceForeignKeyName { get; }
        protected abstract string UrgencyForeignKeyName { get; }
        protected abstract string ClientSubdivisionForeignKeyName { get; }
        protected abstract string CallTypeForeignKeyName { get; }
        protected abstract string IncidentResultForeignKeyName { get; }
        protected abstract string RFSResultForeignKeyName { get; }
        protected abstract string WorkflowSchemeForeignKeyName { get; }
        protected abstract string CalendarWorkScheduleForeignKeyName { get; }
        protected abstract string TimeZoneForeignKeyName { get; }
        protected abstract string CallServiceForeignKeyName { get; }
        protected abstract string BudgetUsageForeignKeyName { get; }
        protected abstract string BudgetUsageCauseForeignKeyName { get; }
        protected abstract string QueueForeignKeyName { get; }
        protected abstract string FormValuesForeignKeyName { get; }
    }
}
