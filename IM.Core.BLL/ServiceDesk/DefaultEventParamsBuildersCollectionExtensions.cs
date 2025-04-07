using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.Manhours;
using InfraManager.DAL.Settings.UserFields;
using InfraManager.ResourcesArea;
using System;
using System.Linq.Expressions;

namespace InfraManager.BLL.ServiceDesk
{
    internal static class DefaultEventParamsBuildersCollectionExtensions
    {
        public static void WithNumber<T>(
            this IDefaultEventParamsBuildersCollection<T> collection) where T : IServiceDeskEntity
        {
            collection.HasProperty(x => x.Number).HasName("Номер");
        }

        public static void WithEntityStateName<T>(
            this IDefaultEventParamsBuildersCollection<T> collection) where T : IWorkflowEntity
        {
            collection.HasProperty(x => x.EntityStateName).HasName("Состояние");
        }

        public static void WithDescription<T>(
            this IDefaultEventParamsBuildersCollection<T> collection) where T : IServiceDeskEntity
        {
            collection.HasProperty(x => x.Description).HasName("Описание");
        }

        public static void WithPriority<T>(
            this IDefaultEventParamsBuildersCollection<T> collection,
            IFinder<Priority> priorityFinder) where T : IServiceDeskEntity
        {
            collection
                .HasProperty(x => x.PriorityID)
                .HasName("Приоритет")
                .HasFinderRequiredLookupConverter(priorityFinder, x => x.Name);
        }

        public static void WithBudgetAggregate<T>(
            this IDefaultEventParamsBuildersCollection<T> collection,
            IFinder<CallBudgetUsageAggregate> budgetUsageFinder,
            IFinder<CallBudgetUsageCauseAggregate> budgetUsageCauseFinder) where T : IHaveBudget
        {
           collection
                .HasProperty(x => x.BudgetUsageAggregateID)
                .HasName("Бюджет")
                .HasFinderRequiredLookupConverter(budgetUsageFinder, x => x.FullName);
            collection
                .HasProperty(x => x.BudgetUsageCauseAggregateID)
                .HasName("Основание")
                .HasFinderRequiredLookupConverter(budgetUsageCauseFinder, x => x.Name);
        }

        public static void WithManhours<T>(
            this IDefaultEventParamsBuildersCollection<T> collection,
            ILocalizeText localizer) where T : IHaveManhours
        {
            Func<int, string> manhoursConverter = val => localizer.ManhoursToString(Global.RU, val);
            collection
                .HasProperty(x => x.ManhoursInMinutes)
                .HasName("Трудозатраты")
                .HasConverter(manhoursConverter);
            collection
                .HasProperty(x => x.ManhoursNormInMinutes)
                .HasName("Оценка трудозатрат")
                .HasConverter(manhoursConverter);
        }

        public static void WithUserFields<T>(
            this IDefaultEventParamsBuildersCollection<T> collection,
            IUserFieldNameBLL userFieldNameProvider) where T : IHaveUserFields
        {
            collection.HasProperty(x => x.UserField1).HasName(userFieldNameProvider.GetName(FieldNumber.UserField1));
            collection.HasProperty(x => x.UserField2).HasName(userFieldNameProvider.GetName(FieldNumber.UserField2));
            collection.HasProperty(x => x.UserField3).HasName(userFieldNameProvider.GetName(FieldNumber.UserField3));
            collection.HasProperty(x => x.UserField4).HasName(userFieldNameProvider.GetName(FieldNumber.UserField4));
            collection.HasProperty(x => x.UserField5).HasName(userFieldNameProvider.GetName(FieldNumber.UserField5));
        }

        public static void WithDescription<T>(
            this IDefaultEventParamsBuildersCollection<T> collection,
            string name,
            Expression<Func<T, Description>> description)
        {
            collection.HasParam(new DescriptionEventSubjectParamBuilder<T>(name, description));
        }
    }
}
