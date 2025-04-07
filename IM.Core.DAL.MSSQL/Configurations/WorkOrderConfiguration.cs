using InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class WorkOrderConfiguration : WorkOrderConfigurationBase
    {
        protected override string PrimaryKeyName => "PK_WorkOrder";
        protected override string TypeForeignKeyName => "FK_WorkOrder_WorkOrderType";
        protected override string PriorityForeignKeyName => "FK_WorkOrder_WorkOrderPriority";
        protected override string AssigneeForeignKeyName => "FK_WorkOrder_AssignorDefaultID_User";
        protected override string ExecutorForeignKeyName => "FK_WorkOrder_ExecutorDefaultID_User";
        protected override string InitiatorForeignKeyName => "FK_WorkOrder_InitiatorDefaultID_User";
        protected override string QueueForeignKeyName => "FK_WorkOrder_QueueDefaultID";
        protected override string WorkflowSchemeForeignKeyName => "FK_WorkOrder_WorkflowScheme";
        protected override string BudgetUsageCauseForeignKeyName => "FK_WorkOrder_BudgetUsageCauseAggregate";
        protected override string BudgetUsageForeignKeyName => "FK_WorkOrder_CallBudgetUsageAggregate";
        protected override string ReferenceForeignKeyName => "FK_WorkOrderReference_WorkOrder";
        protected override string FormValuesForeignKeyName => "FK_WorkOrder_FormValues";
        protected override string ForeignKeyFinancePurchase => "FK_WorkOrderFinancePurchase_WorkOrderID";

        protected override void ConfigureDatabase(EntityTypeBuilder<WorkOrder> builder)
        {
            builder.ToTable("WorkOrder", "dbo");

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.Number).HasColumnName("Number");
            builder.Property(x => x.Name).HasColumnName("Name");
            builder.Property(x => x.TypeID).HasColumnName("WorkOrderTypeID");
            builder.Property(x => x.PriorityID).HasColumnName("WorkOrderPriorityID");

            builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
            builder.Property(x => x.InitiatorDefaultID).HasColumnName("InitiatorDefaultID");

            builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
            builder.Property(x => x.ExecutorDefaultID).HasColumnName("ExecutorDefaultID");

            builder.Property(x => x.QueueID).HasColumnName("QueueID");
            builder.Property(x => x.QueueDefaultID).HasColumnName("QueueDefaultID");

            builder.Property(x => x.AssigneeID).HasColumnName("AssignorID");     
            builder.Property(x => x.AssigneeDefaultID).HasColumnName("AssignorDefaultID");

            builder.Property(x => x.UtcDateCreated).HasColumnName("UtcDateCreated").HasColumnType("datetime");
            builder.Property(x => x.UtcDateModified).HasColumnName("UtcDateModified").HasColumnType("datetime");
            builder.Property(x => x.UtcDateAssigned).HasColumnName("UtcDateAssigned").HasColumnType("datetime");
            builder.Property(x => x.UtcDateAccepted).HasColumnName("UtcDateAccepted").HasColumnType("datetime");
            builder.Property(x => x.UtcDateStarted).HasColumnName("UtcDateStarted").HasColumnType("datetime");
            builder.Property(x => x.UtcDatePromised).HasColumnName("UtcDatePromised").HasColumnType("datetime");
            builder.Property(x => x.UtcDateAccomplished).HasColumnName("UtcDateAccomplished").HasColumnType("datetime");

            builder.Property(x => x.UserField1).HasColumnName("UserField1");
            builder.Property(x => x.UserField2).HasColumnName("UserField2");
            builder.Property(x => x.UserField3).HasColumnName("UserField3");
            builder.Property(x => x.UserField4).HasColumnName("UserField4");
            builder.Property(x => x.UserField5).HasColumnName("UserField5");

            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");

            builder.Property(x => x.EntityStateID).HasColumnName("EntityStateID");
            builder.Property(x => x.EntityStateName).HasColumnName("EntityStateName");
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("WorkflowSchemeID");
            builder.Property(x => x.WorkflowSchemeIdentifier).HasColumnName("WorkflowSchemeIdentifier");
            builder.Property(x => x.WorkflowSchemeVersion).HasColumnName("WorkflowSchemeVersion");

            builder.Property(x => x.HTMLDescription).HasColumnName("HTMLDescription");
            builder.Property(x => x.Description).HasColumnName("Description").HasColumnType("ntext");
            builder.Property(x => x.ManhoursNormInMinutes).HasColumnName("ManhoursNormInMinutes");

            builder.Property(x => x.BudgetUsageAggregateID).HasColumnName("BudgetUsageAggregateID");
            builder.Property(x => x.BudgetUsageCauseAggregateID).HasColumnName("BudgetUsageCauseAggregateID");

            builder.Property(x => x.IsActive).HasColumnName("IsActive");
            builder.Property(x => x.IsFinished).HasColumnName("IsFinished");

            builder.Property(x => x.BudgetUsageCauseAggregateID)
                .IsRequired()
                .HasColumnName("BudgetUsageCauseAggregateID")
                .HasDefaultValue(Guid.Empty);

            builder.Property(x => x.BudgetUsageAggregateID)
                .IsRequired()
                .HasColumnName("BudgetUsageAggregateID")
                .HasDefaultValue(Guid.Empty);

            builder.Property(x => x.WorkOrderReferenceID)
                .IsRequired()
                .HasColumnType("bigint")
                .HasColumnName("WorkOrderReferenceID");

            builder.Property(x => x.ManhoursInMinutes).HasColumnName("ManhoursInMinutes");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
            builder.Property(x => x.FormID).HasColumnName("FormID");
        }
    }
}
