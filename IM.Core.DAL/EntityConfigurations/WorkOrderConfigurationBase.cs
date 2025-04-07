using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InfraManager.DAL.ServiceDesk.WorkOrders;

namespace InfraManager.DAL.EntityConfigurations
{
    public abstract class WorkOrderConfigurationBase : IEntityTypeConfiguration<WorkOrder>
    {
        #region configuration

        public void Configure(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.HasKey(x => x.IMObjID).HasName(PrimaryKeyName);

            builder.Property(x => x.IMObjID).ValueGeneratedNever();
            builder.Property(x => x.Number).IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(500);

            builder.Property(x => x.AssigneeDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.HasOne(x => x.Assignee)
                .WithMany()
                .HasForeignKey(x => x.AssigneeDefaultID)
                .HasConstraintName(AssigneeForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);
            builder.Property(x => x.ExecutorDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.HasOne(x => x.Executor)
                .WithMany()
                .HasForeignKey(x => x.ExecutorDefaultID)
                .HasConstraintName(ExecutorForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder.Property(x => x.InitiatorDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.HasOne(x => x.Initiator)
                .WithMany()
                .HasForeignKey(x => x.InitiatorDefaultID)
                .HasConstraintName(InitiatorForeignKeyName)
                .HasPrincipalKey(x => x.IMObjID);

            builder.Property(x => x.QueueDefaultID).ValueGeneratedOnAddOrUpdate();
            builder.HasOne(x => x.Group)
                .WithMany()
                .HasForeignKey(x => x.QueueDefaultID)
                .HasConstraintName(QueueForeignKeyName);

            builder.Property(x => x.UserField1).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserField2).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserField3).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserField4).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserField5).IsRequired().HasMaxLength(250);

            builder.Property(x => x.RowVersion).IsRequired();

            builder.Property(x => x.EntityStateID).HasMaxLength(50);
            builder.Property(x => x.EntityStateName).HasMaxLength(250);
            builder.Property(x => x.WorkflowSchemeIdentifier).HasMaxLength(50);
            builder.Property(x => x.WorkflowSchemeVersion).HasMaxLength(50);

            builder.Property(x => x.HTMLDescription).IsRequired();

            builder.Property(x => x.IsActive).ValueGeneratedOnAddOrUpdate();
            builder.Property(x => x.IsFinished).ValueGeneratedOnAddOrUpdate();
            builder.Ignore(x => x.TargetEntityStateID);

            builder.HasOne(x => x.Type)
                .WithMany()
                .HasForeignKey(x => x.TypeID)
                .HasConstraintName(TypeForeignKeyName);

            builder.HasOne(x => x.Priority)
                .WithMany()
                .HasForeignKey(x => x.PriorityID)
                .HasConstraintName(PriorityForeignKeyName);

            builder.HasOne(x => x.WorkflowScheme)
                .WithMany()
                .HasForeignKey(x => x.WorkflowSchemeID)
                .HasConstraintName(WorkflowSchemeForeignKeyName);

            builder.HasOne(x => x.BudgetUsageCause)
                .WithMany()
                .HasForeignKey(x => x.BudgetUsageCauseAggregateID)
                .HasConstraintName(BudgetUsageCauseForeignKeyName);

            builder.HasOne(x => x.BudgetUsage)
                .WithMany()
                .HasForeignKey(x => x.BudgetUsageAggregateID)
                .HasConstraintName(BudgetUsageForeignKeyName);

            builder.HasOne(x => x.WorkOrderReference)
                .WithMany()
                .HasForeignKey(x => x.WorkOrderReferenceID)
                .HasConstraintName(ReferenceForeignKeyName);

            builder.HasOne(x => x.Aggregate)
                .WithOne()
                .HasForeignKey<WorkOrderAggregate>(x => x.WorkOrderID);

            builder.HasMany(x => x.Manhours)
                .WithOne()
                .HasForeignKey(x => x.ObjectID);

            builder.HasOne(x => x.FormValues)
               .WithMany()
               .HasForeignKey(x => new { x.FormValuesID, x.FormID, })
               .HasPrincipalKey(x => new { FormValuesID = x.ID, FormID = x.FormBuilderFormID, })
               .HasConstraintName(FormValuesForeignKeyName);

            builder
                .HasOne(x => x.FinancePurchase)
                .WithOne()
                .HasForeignKey<WorkOrderFinancePurchase>(x => x.WorkOrderID)
                .HasConstraintName(ForeignKeyFinancePurchase)
                .OnDelete(DeleteBehavior.Cascade);

            ConfigureDatabase(builder);
        }

        protected abstract void ConfigureDatabase(EntityTypeBuilder<WorkOrder> builder);

        #endregion

        #region Keys

        protected abstract string PrimaryKeyName { get; }
        protected abstract string TypeForeignKeyName { get; }
        protected abstract string PriorityForeignKeyName { get; }
        protected abstract string AssigneeForeignKeyName { get; }
        protected abstract string ExecutorForeignKeyName { get; }
        protected abstract string InitiatorForeignKeyName { get; }
        protected abstract string QueueForeignKeyName { get; }
        protected abstract string WorkflowSchemeForeignKeyName { get; }
        protected abstract string BudgetUsageCauseForeignKeyName { get; }
        protected abstract string BudgetUsageForeignKeyName { get; }
        protected abstract string ReferenceForeignKeyName { get; }
        protected abstract string FormValuesForeignKeyName { get; }
        protected abstract string ForeignKeyFinancePurchase { get; }

        #endregion
    }
}
