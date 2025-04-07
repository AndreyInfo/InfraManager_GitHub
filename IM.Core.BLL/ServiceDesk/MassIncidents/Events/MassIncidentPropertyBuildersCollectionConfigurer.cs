using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceCatalogue.OperationalLevelAgreements;
using InfraManager.DAL.ServiceDesk;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents.Events
{
    internal class MassIncidentPropertyBuildersCollectionConfigurer : IConfigureDefaultEventParamsBuilderCollection<MassIncident>
    {
        private readonly IFinder<MassIncidentCause> _causeFinder;
        private readonly IFinder<User> _userFinder;
        private readonly IFinder<Criticality> _criticalityFinder;
        private readonly IFinder<Group> _groupFinder;
        private readonly IFinder<Priority> _priorityFinder;
        private readonly IFinder<Service> _serviceFinder;
        private readonly IFinder<MassIncidentType> _typeFinder;
        private readonly IMassIncidentInformationChannelBLL _informationChannelsBll;
        private readonly IFinder<WorkFlowScheme> _workflowSchemeFinder;
        private readonly IFinder<TechnicalFailureCategory> _techFailureCategoryFinder;
        private readonly IFinder<OperationalLevelAgreement> _agreementFinder;

        public MassIncidentPropertyBuildersCollectionConfigurer(
            IFinder<MassIncidentCause> causeFinder,
            IFinder<User> userFinder,
            IFinder<Criticality> criticalityFinder,
            IFinder<Group> groupFinder,
            IFinder<Priority> priorityFinder,
            IFinder<Service> serviceFinder,
            IFinder<MassIncidentType> typeFinder,
            IMassIncidentInformationChannelBLL informationChannelsBll,
            IFinder<WorkFlowScheme> workflowSchemeFinder,
            IFinder<TechnicalFailureCategory> techFailureCategoryFinder,
            IFinder<OperationalLevelAgreement> agreementFinder)
        {
            _causeFinder = causeFinder;
            _userFinder = userFinder;
            _criticalityFinder = criticalityFinder;
            _groupFinder = groupFinder;
            _priorityFinder = priorityFinder;
            _serviceFinder = serviceFinder;
            _typeFinder = typeFinder;
            _informationChannelsBll = informationChannelsBll;
            _workflowSchemeFinder = workflowSchemeFinder;
            _techFailureCategoryFinder = techFailureCategoryFinder;
            _agreementFinder = agreementFinder;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<MassIncident> collection)
        {
            collection.HasProperty(x => x.ID).HasName("Номер");
            collection.HasProperty(x => x.Name).HasName("Краткое описание");

            collection
                .HasProperty(x => x.InformationChannelID)
                .HasName("Канал приема информации")
                .HasConverter(id => _informationChannelsBll.Find(id).Name);
            collection
                .HasProperty(x => x.TypeID)
                .HasName("Тип")
                .HasConverter(typeID => _typeFinder.Find(typeID)?.Name);
            collection
                .HasProperty(x => x.TechnicalFailureCategoryID)
                .HasName("Категория технических сбоев")
                .HasConverter(catID => catID.HasValue ? _techFailureCategoryFinder.Find(catID.Value)?.Name : null);
            collection
                .HasProperty(x => x.ServiceID)
                .HasName("Сервис")
                .HasConverter(serviceID => _serviceFinder.Find(serviceID)?.Name);
            collection
                .HasProperty(x => x.OperationalLevelAgreementID)
                .HasName("Соглашение")
                .HasConverter(agreementID => agreementID.HasValue ? _agreementFinder.Find(agreementID.Value)?.Name : null);
            collection
                .HasProperty(x => x.PriorityID)
                .HasName("Приоритет")
                .HasConverter(priorityID => _priorityFinder.Find(priorityID)?.Name);
            collection
                .HasProperty(x => x.CriticalityID)
                .HasName("Критичность")
                .HasConverter(criticalityID => criticalityID.HasValue ? _criticalityFinder.Find(criticalityID)?.Name : null);
            collection
                 .HasProperty(x => x.CauseID)
                 .HasName("Причина")
                 .HasConverter(causeID => causeID.HasValue ? _causeFinder.Find(causeID)?.Name : null);

            collection
                .HasProperty(x => x.OwnedByUserID)
                .HasName("Владелец")
                .HasConverter(ownerID => _userFinder.Find(ownerID)?.FullName);
            collection
                .HasProperty(x => x.ExecutedByUserID)
                .HasName("Исполнитель")
                .HasConverter(ownerID => _userFinder.Find(ownerID)?.FullName);
            collection
                .HasProperty(x => x.CreatedByUserID)
                .HasName("Инициатор")
                .HasConverter(userID => _userFinder.Find(userID).FullName);

            collection
                .HasProperty(x => x.ExecutedByGroupID)
                .HasName("Группа")
                .HasConverter(groupID => _groupFinder.Find(groupID)?.Name);  

            collection.HasProperty(x => x.UtcCreatedAt).HasName("Создан");
            collection.HasProperty(x => x.UtcCloseUntil).HasName("Закрыть до");
            collection.HasProperty(x => x.UtcDateAccomplished).HasName("Выполнен");
            collection.HasProperty(x => x.UtcDateClosed).HasName("Закрыт");
            collection.HasProperty(x => x.UtcOpenedAt).HasName("Открыт");
            collection.HasProperty(x => x.UtcRegisteredAt).HasName("Зарегистрирован");

            collection.WithDescription("Описание", x => x.Description);
            collection.WithDescription("Причина", x => x.Cause);
            collection.WithDescription("Решение", x => x.Solution);

            collection
                .HasProperty(x => x.WorkflowSchemeID)
                .HasName("Схема рабочей процедуры")
                .HasConverter(schemeID => schemeID.HasValue ? _workflowSchemeFinder.Find(schemeID)?.Name : null);
            collection.HasProperty(x => x.EntityStateName).HasName("Состояние");                    
        }
    }
}
