using InfraManager.DAL;
using InfraManager.DAL.Finance;
using InfraManager.DAL.OrganizationStructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Linq.Expressions;
using InfraManager.DAL.Dashboards;

namespace IM.Core.DAL.PGSQL
{
    /// <summary>
    /// Реализация построения моделей даных для Типа софта
    /// </summary>
    public class DbContextModelBuilder : IBuildDbContextModel
    {
        public void BuildModel(ModelBuilder modelBuilder)
        {
            //modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.HasSequence<int>("asset_deviation_number");

            modelBuilder.HasSequence<int>("call_number").StartsAt(21);

            modelBuilder.HasSequence<int>("cost_distribution_rule_number").StartsAt(0);

            modelBuilder.HasSequence<int>("cost_number").StartsAt(0);

            modelBuilder.HasSequence<int>("cost_rule_number").StartsAt(0);

            modelBuilder.HasSequence<int>("document_internal_id");

            modelBuilder.HasSequence<int>("external_event_number").StartsAt(29);

            modelBuilder.HasSequence<int>("kb_article_number").StartsAt(4);

            modelBuilder.HasSequence<int>("manhours_work_number").StartsAt(0);

            modelBuilder.HasSequence<int>("problem_number").StartsAt(5);

            modelBuilder.HasSequence<int>("project_number");

            modelBuilder.HasSequence<int>("rfc_number");

            modelBuilder.HasSequence<int>("service_contract_number");

            modelBuilder.HasSequence<int>("work_order_number").StartsAt(5);

            #region Configure functions

            modelBuilder
                .HasDbFunction(
                    typeof(Subdivision)
                        .GetMethod(nameof(Subdivision.GetFullSubdivisionName)))
                .HasName("func_get_full_subdivision_name");

            modelBuilder
                .HasDbFunction(
                    typeof(Subdivision)
                        .GetMethod(nameof(Subdivision.SubdivisionIsSibling)))
                .HasName("func_subdivision_is_sibling");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.ServiceDesk.ProblemType)
                        .GetMethod(nameof(InfraManager.DAL.ServiceDesk.ProblemType.GetFullProblemTypeName)))
                .HasName("func_get_full_problem_type_name");

            modelBuilder
                .HasDbFunction(
                    typeof(BudgetUsage)
                        .GetMethod(nameof(BudgetUsage.GetBudgetFullName)))
                .HasName("func_get_full_budget_name");

            modelBuilder
                .HasDbFunction(
                    typeof(User).GetMethod(nameof(User.GetFullName)))
                .HasName("func_get_full_user_name");

            modelBuilder
                .HasDbFunction(
                    typeof(ObjectNote).GetMethod(nameof(ObjectNote.GetUnreadObjectNoteCount)))
                .HasName("func_get_unread_object_note_count");

            modelBuilder
                .HasDbFunction(
                    typeof(ObjectNote).GetMethod(nameof(ObjectNote.GetObjectNoteCount)))
                .HasName("func_get_object_note_count");
            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.Workflow.Workflow).GetMethod(nameof(InfraManager.DAL.Workflow.Workflow.IsFinished)))
                .HasName("func_is_finished");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.Workflow.Workflow).GetMethod(nameof(InfraManager.DAL.Workflow.Workflow.IsOverdue)))
                .HasName("func_is_overdue");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.ServiceDesk.CallType)
                        .GetMethod(nameof(InfraManager.DAL.ServiceDesk.CallType.GetFullCallTypeName)))
                .HasName("func_get_full_call_type_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.ServiceDesk.CallType)
                        .GetMethod(nameof(InfraManager.DAL.ServiceDesk.CallType.GetRootId)))
                .HasName("func_get_root_call_type_id");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.TruncateSeconds)))
                .HasName("func_truncate_seconds_dt")
                .HasStoreType("timestamp");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.AccessIsGranted)))
                .HasName("func_access_is_granted");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetReasonName)))
                .HasName("func_get_reason_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetFullObjectName)))
                .HasName("func_get_full_object_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetFullObjectLocation)))
                .HasName("func_get_full_object_location");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetFullTimeZoneName)))
                .HasName("func_get_full_time_zone_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetFullCalendarWorkScheduleName)))
                .HasName("func_get_full_calendar_work_schedule_name");

            modelBuilder
                .HasDbFunction(
                    typeof(Group).GetMethod(nameof(Group.UserInOrganizationItem)))
                .HasName("func_user_in_organization_item");
            
            modelBuilder
                .HasDbFunction(
                    typeof(Dashboard).GetMethod(nameof(Dashboard.DbFuncDashboardTreeItemIsVisible)))
                .HasName("func_dashboard_treeitem_is_visible");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.ItemInOrganizationItem)))
                .HasName("func_item_in_organization_item");

            modelBuilder
                .HasDbFunction(
                    typeof(Budget).GetMethod(nameof(Budget.GetFullBudgetName)))
                .HasName("func_get_full_budget_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.CastAsString), new[] { typeof(string) } ))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(string),
                        new StringTypeMapping($"text")));

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.CastAsString), new[] { typeof(int) }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(string),
                        new StringTypeMapping($"text")));
            
            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.GetCategoryFullName)))
                .HasName("func_get_full_product_catalog_category_name");

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.CastAsDateTime), new[] { typeof(DateTime?), }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(DateTime?),
                        new DateTimeTypeMapping("timestamp(3)")));

            modelBuilder
                .HasDbFunction(
                    typeof(InfraManager.DAL.DbFunctions).GetMethod(nameof(InfraManager.DAL.DbFunctions.CastAsDecimal), new[] { typeof(decimal?), }))
                .HasTranslation(
                    args => new SqlUnaryExpression(
                        ExpressionType.Convert,
                        args.First(),
                        typeof(decimal?),
                        new DecimalTypeMapping("numeric")));

            #endregion
        }
    }
}
