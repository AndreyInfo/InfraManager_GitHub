using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using System;
using System.Globalization;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Calls.Events
{
    internal class CallPropertyEventParamsBuilderConfigurer : IConfigureDefaultEventParamsBuilderCollection<Call>
    {
        private readonly ILocalizeText _localizer;
        private readonly ILocalizeEnum<CallReceiptType> _receiptTypelLocalizer;
        private readonly IReadonlyRepository<CallType> _callTypesRepository;
        private readonly IFinder<Urgency> _urgencyFinder;
        private readonly IFinder<Influence> _influenceFinder;
        private readonly IFinder<Priority> _priorityFinder;
        private readonly IFinder<CallService> _callServiceFinder;
        private readonly IServiceMapper<ObjectClass, IFindNameByGlobalID> _nameFindersMapping;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder; // TODO: Когда будет исправлена структура таблицы Call и в ней будут FK пользователей, ссылаться на PK, а не на что попало, тогда тут нужен обычный Finder, чтобы не делать запросов в БД
        private readonly IFinder<Group> _queueFinder;
        private readonly IFinder<IncidentResult> _incidentResultFinder;
        private readonly IFinder<RequestForServiceResult> _rfsResultFinder;
        private readonly IServiceMapper<UserFieldType, IUserFieldNameBLL> _userFieldNamesMapping;
        private readonly IFinder<CallBudgetUsageAggregate> _budgetUsageFinder;
        private readonly IFinder<CallBudgetUsageCauseAggregate> _budgetUsageCauseFinder;

        public CallPropertyEventParamsBuilderConfigurer(
            ILocalizeText localizer,
            ILocalizeEnum<CallReceiptType> receiptTypelLocalizer,
            IReadonlyRepository<CallType> callTypesRepository,
            IFinder<Urgency> urgencyFinder,
            IFinder<Influence> influenceFinder,
            IFinder<Priority> priorityFinder,
            IFinder<CallService> callServiceFinder,
            IServiceMapper<ObjectClass, IFindNameByGlobalID> nameFindersMapping,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IFinder<Group> queueFinder,
            IFinder<IncidentResult> incidentResultFinder,
            IFinder<RequestForServiceResult> rfsResultFinder,
            IServiceMapper<UserFieldType, IUserFieldNameBLL> userFieldNamesMapping,
            IFinder<CallBudgetUsageAggregate> budgetUsageFinder,
            IFinder<CallBudgetUsageCauseAggregate> budgetUsageCauseFinder)
        {
            _localizer = localizer;
            _receiptTypelLocalizer = receiptTypelLocalizer;
            _callTypesRepository = callTypesRepository;
            _urgencyFinder = urgencyFinder;
            _influenceFinder = influenceFinder;
            _priorityFinder = priorityFinder;
            _callServiceFinder = callServiceFinder;
            _nameFindersMapping = nameFindersMapping;
            _userFinder = userFinder;
            _queueFinder = queueFinder;
            _incidentResultFinder = incidentResultFinder;
            _rfsResultFinder = rfsResultFinder;
            _userFieldNamesMapping = userFieldNamesMapping;
            _budgetUsageFinder = budgetUsageFinder;
            _budgetUsageCauseFinder = budgetUsageCauseFinder;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<Call> collection)
        {            
            var callTypes = _callTypesRepository.With(x => x.Parent).ToDictionary(x => x.ID);

            collection.WithNumber();
            collection
                .HasProperty(x => x.ReceiptType)
                .HasName("Способ получения")
                .HasConverter(val => _receiptTypelLocalizer.Localize(val));
            collection
                .HasProperty(x => x.CallTypeID)
                .HasName("Тип заявки")
                .HasConverter(callTypeID => callTypes.ContainsKey(callTypeID) ? callTypes[callTypeID].FullName : string.Empty);
            collection
                .HasProperty(x => x.UrgencyID)
                .HasName("Срочность")
                .HasFinderLookupConverter(_urgencyFinder, x => x.Name);
            collection
                .HasProperty(x => x.InfluenceID)
                .HasName("Влияние")
                .HasFinderLookupConverter(_influenceFinder, x => x.Name);
            collection.WithPriority(_priorityFinder);
            collection.HasProperty(x => x.CallSummaryName).HasName("Краткое описание");
            collection
                .HasProperty(x => x.CallServiceID)
                .HasName("Сервис")
                .HasFinderRequiredLookupConverter(_callServiceFinder, x => x.ServiceName);
            collection
                .HasProperty(x => x.CallServiceID)
                .HasName("Элемент сервиса")
                .HasFinderRequiredLookupConverter(_callServiceFinder, x => x.ServiceItemID.HasValue ? x.ServiceItemOrAttendanceName : null);
            collection
                .HasProperty(x => x.CallServiceID)
                .HasName("Услуга сервиса")
                .HasFinderRequiredLookupConverter(_callServiceFinder, x => x.ServiceAttendanceID.HasValue ? x.ServiceItemOrAttendanceName : null);
            collection.WithDescription();
            collection.HasProperty(x => x.UtcDateRegistered).HasName("Зарегистрирована");
            collection.HasProperty(x => x.UtcDateOpened).HasName("Открыта");
            collection.HasProperty(x => x.UtcDateAccomplished).HasName("Выполнена");
            collection.HasProperty(x => x.UtcDateClosed).HasName("Закрыта");
            collection.HasProperty(x => x.UtcDatePromised).HasName("Закрыть до");
            collection.HasProperty(x => x.SLAName).HasName("SLA");
            collection
                .HasProperty(x => x.Price)
                .HasName("Стоимость")
                .HasConverter(x => x.HasValue ? x.Value.ToString("C", new CultureInfo(Global.RU)) : null);
            collection
                .HasParam(
                    "Место оказания сервиса",
                    state =>
                    {
                        var classID = (ObjectClass?)state[nameof(Call.ServicePlaceClassID)];
                        var placeID = (Guid?)state[nameof(Call.ServicePlaceID)];

                        return placeID.HasValue && classID.HasValue
                            ? _nameFindersMapping.Map(classID.Value).Find(placeID.Value) ?? string.Empty
                            : null;
                    });
            collection
                .HasProperty(x => x.InitiatorID)
                .HasName("Заявитель")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);             
            collection
                .HasProperty(x => x.ClientID)
                .HasName("Клиент")
                .HasFinderRequiredLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.OwnerID)
                .HasName("Владелец")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.ExecutorID)
                .HasName("Исполнитель")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection
                .HasProperty(x => x.AccomplisherID)
                .HasName("Выполнивший заявку")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);            
            collection
                .HasProperty(x => x.QueueID)
                .HasName("Группа")
                .HasFinderLookupConverter(_queueFinder, x => x.Name);
            collection
                .HasProperty(x => x.EscalationCount)
                .HasName("Количество эскалаций")
                .HasConverter(x => x == default ? null : x.ToString()); // default значение для истории изменений этого свойства считается пустым
            collection.HasProperty(x => x.Solution).HasName("Решение");
            collection.HasProperty(x => x.Grade).HasName("Оценка");
            collection
                .HasProperty(x => x.IncidentResultID)
                .HasName("Результат завершения инцидента")
                .HasFinderLookupConverter(_incidentResultFinder, x => x.Name);
            collection
                .HasProperty(x => x.RequestForServiceResultID)
                .HasName("Результат запроса на услугу")
                .HasFinderLookupConverter(_rfsResultFinder, x => x.Name);

            collection.WithUserFields(_userFieldNamesMapping.Map(UserFieldType.Call));

            collection.WithBudgetAggregate(_budgetUsageFinder, _budgetUsageCauseFinder);
            collection.WithEntityStateName();
            collection.WithManhours(_localizer);
        }
    }
}
