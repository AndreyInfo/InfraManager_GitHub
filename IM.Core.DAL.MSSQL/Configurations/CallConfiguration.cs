using Core = InfraManager.DAL.EntityConfigurations;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace InfraManager.DAL.Microsoft.SqlServer.Configurations
{
    internal class CallConfiguration : Core.CallConfiguration
    {
        protected override string PrimaryKeyName => "PK_Call_NonClustered";
        protected override string PriorityForeignKeyName => "FK_Call_Priority";
        protected override string AccomplisherForeignKeyName => "FK_Call_AccomplisherDefaultID_User";
        protected override string OwnerForeignKeyName => "FK_Call_OwnerDefaultID_User";
        protected override string InitiatorForeignKeyName => "FK_Call_InitiatorDefaultID_User";
        protected override string ExecutorForeignKeyName => "FK_Call_ExecutorDefaultID_User";
        protected override string InfluenceForeignKeyName => "FK_Call_Influence";
        protected override string UrgencyForeignKeyName => "FK_Call_Urgency";
        protected override string ClientSubdivisionForeignKeyName => "FK_Call_Subdivision";
        protected override string CallTypeForeignKeyName => "FK_Call_CallType";
        protected override string IncidentResultForeignKeyName => "FK_Call_IncidentResult";
        protected override string RFSResultForeignKeyName => "FK_Call_RFSResult";
        protected override string WorkflowSchemeForeignKeyName => "FK_Call_WorkflowScheme";
        protected override string CalendarWorkScheduleForeignKeyName => "FK_Call_CalendarWorkSchedule";
        protected override string TimeZoneForeignKeyName => "FK_Call_TimeZone";
        protected override string CallServiceForeignKeyName => "FK_Call_CallService";
        protected override string BudgetUsageForeignKeyName => "FK_Call_BudgetUsageCauseAggregate";
        protected override string BudgetUsageCauseForeignKeyName => "FK_Call_CallBudgetUsageAggregate";
        protected override string ClientForeignKeyName => "FK_Call_IMObjID_User";
        protected override string QueueForeignKeyName => "FK_Call_Queue";
        protected override string FormValuesForeignKeyName => "FK_Call_FormValues";
        protected override void ConfigureDatabase(EntityTypeBuilder<Call> builder)
        {
            builder.ToTable("Call", "dbo");            

            builder.Property(x => x.IMObjID).HasColumnName("ID");
            builder.Property(x => x.CallTypeID).HasColumnName("CallTypeID");
            builder.Property(x => x.PriorityID).HasColumnName("PriorityID");
            builder.Property(x => x.Number)                
                .HasColumnName("Number")
                .HasDefaultValueSql("NEXT VALUE FOR [dbo].[CallNumber]");
            builder.Property(x => x.ReceiptType)
                .HasColumnType("tinyint")
                .HasColumnName("ReceiptType");
            builder.Property(x => x.InitiatorID).HasColumnName("InitiatorID");
            builder.Property(x => x.ClientID).HasColumnName("ClientID");
            builder.Property(x => x.ClientSubdivisionID).HasColumnName("ClientSubdivisionID");
            builder.Property(x => x.OwnerID).HasColumnName("OwnerID");
            builder.Property(x => x.ExecutorID).HasColumnName("ExecutorID");
            builder.Property(x => x.QueueID).HasColumnName("QueueID");
            builder.Property(x => x.UrgencyID).HasColumnName("UrgencyID");
            builder.Property(x => x.InfluenceID).HasColumnName("InfluenceID");
            builder.Property(x => x.UtcDateModified)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateModified");
            builder.Property(x => x.UtcDateRegistered)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateRegistered");
            builder.Property(x => x.UtcDateOpened)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateOpened");
            builder.Property(x => x.UtcDatePromised)
                .HasColumnType("datetime")
                .HasColumnName("UtcDatePromised");
            builder.Property(x => x.UtcDateAccomplished)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateAccomplished");
            builder.Property(x => x.UtcDateClosed)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateClosed");
            builder.Property(x => x.UtcDateCreated)
                .HasColumnType("datetime")
                .HasColumnName("UtcDateCreated");
            builder.Property(x => x.EscalationCount).HasColumnName("EscalationCount");
            builder.Property(x => x.AccomplisherID).HasColumnName("AccomplisherID");
            builder.Property(x => x.Grade)
                .HasColumnType("tinyint")
                .HasColumnName("Grade");
            builder.Property(x => x.IncidentResultID).HasColumnName("IncidentResultID");
            builder.Property(x => x.RequestForServiceResultID).HasColumnName("RFSResultID");
            builder.Property(x => x.Removed).HasColumnName("Removed");
            builder.Property(x => x.UserField1).HasColumnName("UserField1");
            builder.Property(x => x.UserField2).HasColumnName("UserField2");
            builder.Property(x => x.UserField3).HasColumnName("UserField3");
            builder.Property(x => x.UserField4).HasColumnName("UserField4");
            builder.Property(x => x.UserField5).HasColumnName("UserField5");
            builder.Property(x => x.SLAName).HasColumnName("SLAName");
            builder.Property(x => x.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Price");
            builder.Property(x => x.EntityStateID).HasColumnName("EntityStateID");
            builder.Property(x => x.EntityStateName).HasColumnName("EntityStateName");
            builder.Property(x => x.WorkflowSchemeID).HasColumnName("WorkflowSchemeID");
            builder
                .Property(x => x.WorkflowSchemeIdentifier)
                .HasColumnName("WorkflowSchemeIdentifier");
            builder
                .Property(x => x.WorkflowSchemeVersion)
                .HasColumnName("WorkflowSchemeVersion");
            builder.Property(x => x.CallSummaryName).HasColumnName("CallSummaryName");
            builder
                .Property(x => x.CalendarWorkScheduleID)
                .HasColumnName("CalendarWorkScheduleID");
            builder.Property(x => x.TimeZoneID).HasColumnName("TimeZoneID");
            builder.Property(x => x.HTMLDescription).HasColumnName("HTMLDescription");
            builder.Property(x => x.HTMLSolution).HasColumnName("HTMLSolution");
            builder.Property(x => x.LineNumber).HasColumnType("tinyint").HasColumnName("LineNumber");
            builder.Property(x => x.Description).HasColumnType("ntext").HasColumnName("Description");
            builder.Property(x => x.Solution).HasColumnType("ntext").HasColumnName("Solution");
            builder.Property(x => x.ManhoursNormInMinutes).HasColumnName("ManhoursNormInMinutes");
            builder.Property(x => x.OnWorkOrderExecutorControl).HasColumnName("OnWorkOrderExecutorControl");
            builder.Property(x => x.ServicePlaceID).HasColumnName("ServicePlaceID");
            builder.Property(x => x.ServicePlaceClassID).HasColumnName("ServicePlaceClassID");
            builder.Property(x => x.CallServiceID).IsRequired().HasColumnName("CallServiceID");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
            builder.Property(x => x.FormID).HasColumnName("FormID");
            builder
                .Property(x => x.BudgetUsageCauseAggregateID)
                .HasColumnName("BudgetUsageCauseAggregateID")
                .HasDefaultValue(Guid.Empty);
            builder
                .Property(x => x.BudgetUsageAggregateID)
                .HasColumnName("BudgetUsageAggregateID")
                .HasDefaultValue(Guid.Empty);
            builder
                .Property(x => x.IsActive)                
                .HasColumnName("IsActive")
                .HasComputedColumnSql(
                    "case when [Removed]=(0) AND ([EntityStateID] " +
                    "IS NOT NULL OR [WorkflowSchemeID] IS NOT NULL OR [WorkflowSchemeVersion] " +
                    "IS NULL) then (1) else (0) end");
            builder
                .Property(x => x.InitiatorDefaultID)
                .HasColumnName("InitiatorDefaultID")
                .HasComputedColumnSql("isnull([InitiatorID],'00000000-0000-0000-0000-000000000001')");
            builder
                .Property(x => x.ExecutorDefaultID)
                .HasColumnName("ExecutorDefaultID")
                .HasComputedColumnSql("isnull([ExecutorID],'00000000-0000-0000-0000-000000000001')");
            builder
                .Property(x => x.OwnerDefaultID)
                .HasColumnName("OwnerDefaultID")
                .HasComputedColumnSql("isnull([OwnerID],'00000000-0000-0000-0000-000000000001')");
            builder
                .Property(x => x.AccomplisherDefaultID)
                .HasColumnName("AccomplisherDefaultID")
                .HasComputedColumnSql("isnull([AccomplisherID],'00000000-0000-0000-0000-000000000001')");
            builder
                .Property(x => x.ClientSubdivisionDefaultID)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql("isnull([ClientSubdivisionID],'00000000-0000-0000-0000-000000000001')");
            builder
                .Property(x => x.UrgencyDefaultID)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql("isnull([UrgencyID],'00000000-0000-0000-0000-000000000000')");
            builder
                .Property(x => x.InfluenceDefaultID)
                .ValueGeneratedOnAddOrUpdate()
                .HasComputedColumnSql("isnull([InfluenceID],'00000000-0000-0000-0000-000000000000')");
            builder
                .Property(x => x.IsFinished)
                .HasColumnName("IsFinished")                
                .HasComputedColumnSql(
                    "case when [UtcDateAccomplished] IS NOT NULL AND " +
                    "([UtcDateClosed] IS NOT NULL OR [UtcDateOpened] IS NULL " +
                    "OR [UtcDateOpened]<=[UtcDateAccomplished]) then CONVERT([bit],(1)) " +
                    "else CONVERT([bit],(0)) end");
            builder.Property(x => x.ManhoursInMinutes).HasColumnName("ManhoursInMinutes");
            builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .HasColumnType("timestamp")
                .HasColumnName("RowVersion");
            builder.Property(x => x.FormValuesID).HasColumnName("FormValuesID");
        }
    }
}
