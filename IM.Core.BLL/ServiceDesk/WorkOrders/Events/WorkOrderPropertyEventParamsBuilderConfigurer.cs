using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.WorkOrders.Events
{
    internal class WorkOrderPropertyEventParamsBuilderConfigurer : IConfigureDefaultEventParamsBuilderCollection<WorkOrder>
    {
        private readonly ILocalizeText _localizer;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder; // TODO: Когда будет исправлена структура таблицы WorkOrder и в ней будут FK пользователей, ссылаться на PK, а не на что попало, тогда тут нужен обычный Finder, чтобы не делать запросов в БД
        private readonly IFinder<Group> _queueFinder;
        private readonly IServiceMapper<UserFieldType, IUserFieldNameBLL> _userFieldNamesMapping;
        private readonly IFinder<CallBudgetUsageAggregate> _budgetUsageFinder;
        private readonly IFinder<CallBudgetUsageCauseAggregate> _budgetUsageCauseFinder;
        private readonly IFinder<WorkOrderType> _typeFinder;
        private readonly IFinder<WorkOrderPriority> _priorityFinder;

        public WorkOrderPropertyEventParamsBuilderConfigurer(
            ILocalizeText localizer,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IFinder<Group> queueFinder,
            IServiceMapper<UserFieldType, IUserFieldNameBLL> userFieldNamesMapping,
            IFinder<CallBudgetUsageAggregate> budgetUsageFinder,
            IFinder<CallBudgetUsageCauseAggregate> budgetUsageCauseFinder,
            IFinder<WorkOrderType> typeFinder,
            IFinder<WorkOrderPriority> priorityFinder)
        {
            _localizer = localizer;
            _userFinder = userFinder;
            _queueFinder = queueFinder;
            _userFieldNamesMapping = userFieldNamesMapping;
            _budgetUsageFinder = budgetUsageFinder;
            _budgetUsageCauseFinder = budgetUsageCauseFinder;
            _typeFinder = typeFinder;
            _priorityFinder = priorityFinder;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<WorkOrder> collection)
        {            
            collection.WithNumber();
            collection.HasProperty(x => x.Name).HasName("Название");
            collection
                .HasProperty(x => x.TypeID)
                .HasName("Тип задания")
                .HasFinderRequiredLookupConverter(_typeFinder, x => x.Name);
            collection
                .HasProperty(x => x.PriorityID)
                .HasName("Приоритет задания")
                .HasFinderRequiredLookupConverter(_priorityFinder, x => x.Name);
            collection.WithDescription();
            collection
                .HasProperty(x => x.InitiatorID)
                .HasName("Инициатор")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.AssigneeID)
                .HasName("Назначивший задание")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.ExecutorID)
                .HasName("Исполнитель")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection.WithManhours(_localizer);
            collection
                .HasProperty(x => x.QueueID)
                .HasName("Группа")
                .HasFinderLookupConverter(_queueFinder, x => x.Name);
            collection.HasProperty(x => x.UtcDateAssigned).HasName("Назначена");
            collection.HasProperty(x => x.UtcDatePromised).HasName("Закрыть до");
            collection.HasProperty(x => x.UtcDateAccepted).HasName("Принята");
            collection.HasProperty(x => x.UtcDateStarted).HasName("Начало выполнения");
            collection.HasProperty(x => x.UtcDateAccomplished).HasName("Выполнена");
            collection.WithUserFields(_userFieldNamesMapping.Map(UserFieldType.WorkOrder));
            collection.WithBudgetAggregate(_budgetUsageFinder, _budgetUsageCauseFinder);
            collection.WithEntityStateName();
        }
    }
}
