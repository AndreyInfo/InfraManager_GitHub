using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.Finance;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Dashboards;

namespace InfraManager.DAL.Microsoft.SqlServer.ModelBuilders
{
    /// <summary>
    /// Реализация построения моделей даных для Типа софта
    /// </summary>
    public class DbContextModelBuilder : IBuildDbContextModel
    {
        public void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.HasSequence<int>("AssetDeviationNumber");

            modelBuilder.HasSequence<int>("CallNumber").StartsAt(1);

            modelBuilder.HasSequence<int>("CostDistributionRuleNumber").StartsAt(0);

            modelBuilder.HasSequence<int>("CostNumber").StartsAt(0);

            modelBuilder.HasSequence<int>("CostRuleNumber").StartsAt(0);

            modelBuilder.HasSequence<int>("DocumentInternalID");

            modelBuilder.HasSequence<int>("ExternalEventNumber").StartsAt(29);

            modelBuilder.HasSequence<int>("KBArticleNumber").StartsAt(4);

            modelBuilder.HasSequence<int>("ManhoursWorkNumber").StartsAt(0);

            modelBuilder.HasSequence<int>("ProblemNumber").StartsAt(5);

            modelBuilder.HasSequence<int>("ProjectNumber");

            modelBuilder.HasSequence<int>("RFCNumber");

            modelBuilder.HasSequence<int>("ServiceContractNumber");

            modelBuilder.HasSequence<int>("WorkOrderNumber").StartsAt(5);

            modelBuilder.HasSequence<long>("ExternalEventNumber", "dbo").StartsAt(1);

            #region Configure functions

            modelBuilder
                .HasDbFunction(
                    typeof(Subdivision)
                        .GetMethod(nameof(Subdivision.GetFullSubdivisionName)))
                .HasName("func_GetFullSubdivisionName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(Subdivision)
                        .GetMethod(nameof(Subdivision.SubdivisionIsSibling)))
                .HasName("func_SubdivisionIsSibling")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(ServiceDesk.ProblemType)
                        .GetMethod(nameof(ServiceDesk.ProblemType.GetFullProblemTypeName)))
                .HasName("func_GetFullProblemTypeName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(BudgetUsage)
                        .GetMethod(nameof(BudgetUsage.GetBudgetFullName)))
                .HasName("func_GetFullBudgetName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(User).GetMethod(nameof(User.GetFullName)))
                .HasName("func_GetFullUserName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(ObjectNote).GetMethod(nameof(ObjectNote.GetUnreadObjectNoteCount)))
                .HasName("func_GetUnreadObjectNoteCount")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(ObjectNote).GetMethod(nameof(ObjectNote.GetObjectNoteCount)))
                .HasName("func_GetObjectNoteCount")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(Workflow.Workflow).GetMethod(nameof(Workflow.Workflow.IsFinished)))
                .HasName("func_IsFinished")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(Workflow.Workflow).GetMethod(nameof(Workflow.Workflow.IsOverdue)))
                .HasName("func_IsOverdue")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(CallType)
                        .GetMethod(nameof(CallType.GetFullCallTypeName)))
                .HasName("func_GetFullCallTypeName")
                .HasSchema("dbo");


            modelBuilder
                .HasDbFunction(
                    typeof(CallType)
                        .GetMethod(nameof(CallType.GetRootId)))
                .HasName("func_GetRootCallTypeID")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.TruncateSeconds)))
                .HasName("func_TruncateSecondsDT")
                .HasStoreType("datetime")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.AccessIsGranted)))
                .HasName("func_AccessIsGranted")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.GetReasonName)))
                .HasName("func_GetReasonName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(Group).GetMethod(nameof(Group.UserInOrganizationItem)))
                .HasName("func_UserInOrganizationItem")
                .HasSchema("dbo");
            
            modelBuilder
                .HasDbFunction(
                    typeof(Dashboard).GetMethod(nameof(Dashboard.DbFuncDashboardTreeItemIsVisible)))
                .HasName("func_DashboardTreeItemIsVisible")
                .HasSchema("dbo");
            
            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.ItemInOrganizationItem)))
                .HasName("func_ItemInOrganizationItem")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.ItemInOrganizationItem)))
                .HasName("func_ItemInOrganizationItem")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(Budget).GetMethod(nameof(Budget.GetFullBudgetName)))
                .HasName("func_GetFullBudgetName")
                .HasSchema("dbo");

            modelBuilder
             .HasDbFunction(
                 typeof(DbFunctions).GetMethod(nameof(DbFunctions.GetFullObjectName)))
             .HasName("func_GetFullObjectName")
             .HasSchema("dbo");

            modelBuilder
             .HasDbFunction(
                 typeof(DbFunctions).GetMethod(nameof(DbFunctions.GetFullObjectLocation)))
             .HasName("func_GetFullObjectLocation")
             .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.GetFullTimeZoneName)))
                .HasName("func_GetFullTimeZoneName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.GetFullCalendarWorkScheduleName)))
                .HasName("func_GetFullCalendarWorkScheduleName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.CastAsString), new[] { typeof(string) }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(string),
                        new StringTypeMapping($"nvarchar(max)")));


            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.CastAsString), new[] { typeof(int) } ))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(string),
                        new StringTypeMapping($"nvarchar(50)")));

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetCategoryFullName)))
                .HasName("func_GetFullProductCatalogCategoryName")
                .HasSchema("dbo");

            modelBuilder
                .HasDbFunction(
                    typeof(DbFunctions).GetMethod(nameof(DbFunctions.CastAsDateTime), new[] { typeof(DateTime?) }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(DateTime?),
                        new DateTimeTypeMapping("datetime")));

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.CastAsDecimal), new[] { typeof(decimal?), }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(decimal?),
                        new DecimalTypeMapping("decimal")));
            #endregion
        }
    }
}
