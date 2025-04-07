using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests.Events
{
    internal class ChangeRequestEventParamsConfigurer : IConfigureDefaultEventParamsBuilderCollection<ChangeRequest>
    {
        private readonly IFinder<ChangeRequestType> _typeFinder;
        private readonly IFinder<Priority> _priorityFinder;
        private readonly IFinder<Urgency> _urgencyFinder;
        private readonly IFinder<Influence> _influenceFinder;
        private readonly IFinder<ChangeRequestCategory> _categoryFinder;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IFinder<Group> _groupFinder;
        private readonly ILocalizeText _localizer;

        public ChangeRequestEventParamsConfigurer(
            IFinder<ChangeRequestType> typeFinder,
            IFinder<Priority> priorityFinder,
            IFinder<Urgency> urgencyFinder,
            IFinder<Influence> influenceFinder,
            IFinder<ChangeRequestCategory> categoryFinder,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IFinder<Group> groupFinder,
            ILocalizeText localizer)
        {
            _typeFinder = typeFinder;
            _priorityFinder = priorityFinder;
            _urgencyFinder = urgencyFinder;
            _influenceFinder = influenceFinder;
            _categoryFinder = categoryFinder;
            _userFinder = userFinder;
            _groupFinder = groupFinder;
            _localizer = localizer;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<ChangeRequest> collection)
        {
            collection.WithNumber();
            collection
                .HasProperty(x => x.RFCTypeID)
                .HasName("Тип запроса")
                .HasFinderRequiredLookupConverter(_typeFinder, x => x.Name);
            collection
                .HasProperty(x => x.UrgencyID)
                .HasName("Срочность")
                .HasFinderLookupConverter(_urgencyFinder, x => x.Name);
            collection
                .HasProperty(x => x.InfluenceID)
                .HasName("Влияние")
                .HasFinderLookupConverter(_influenceFinder, x => x.Name);
            collection.WithPriority(_priorityFinder);
            collection.WithDescription();
            collection.HasProperty(x => x.Summary).HasName("Краткое описание");
            collection.HasProperty(x => x.Target).HasName("Цель");
            collection.HasProperty(x => x.FundingAmount).HasName("Сумма финансирования");
            collection.HasProperty(x => x.UtcDateDetected).HasName("Обнаружен"); //обнаружен запроса на изменение ... странно
            collection.HasProperty(x => x.UtcDatePromised).HasName("Закрыть до");
            collection.HasProperty(x => x.UtcDateSolved).HasName("Решен"); //запрос на изменение обычно выполнен, а не решен ... попахивает копипастой из проблемы
            collection
                .HasProperty(x => x.CategoryID)
                .HasName("Категория запроса")
                .HasFinderLookupConverter(_categoryFinder, x => x.Name);
            collection.HasProperty(x => x.ServiceName).HasName("Элемент сервиса");
            collection.HasProperty(x => x.UtcDateClosed).HasName("Закрыта");
            collection.HasProperty(x => x.UtcDateStarted).HasName("Начать до");
            collection
                .HasProperty(x => x.OwnerID)
                .HasName("Владелец")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.InitiatorID)
                .HasName("Инициатор")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.QueueID)
                .HasName("Группа")
                .HasFinderLookupConverter(_groupFinder, x => x.Name);
            collection.WithEntityStateName();
            collection.WithManhours(_localizer);
        }
    }
}
