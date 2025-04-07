using Inframanager.BLL.Events;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings.UserFields;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Problems.Events
{
    internal class ProblemEventParamsCollectionConfigurer : IConfigureDefaultEventParamsBuilderCollection<Problem>
    {
        private readonly IReadonlyRepository<ProblemType> _typesRepository;
        private readonly IFinder<Priority> _priorityFinder;
        private readonly IFinder<Urgency> _urgencyFinder;
        private readonly IFinder<Influence> _influenceFinder;
        private readonly IFinder<ProblemCause> _problemCauseFinder;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;
        private readonly IServiceMapper<UserFieldType, IUserFieldNameBLL> _userFieldsMapping;
        private readonly ILocalizeText _localizer;

        public ProblemEventParamsCollectionConfigurer(
            IReadonlyRepository<ProblemType> typesRepository,
            IFinder<Priority> priorityFinder,
            IFinder<Urgency> urgencyFinder,
            IFinder<Influence> influenceFinder,
            IFinder<ProblemCause> problemCauseFinder,
            IFindEntityByGlobalIdentifier<User> userFinder,
            IServiceMapper<UserFieldType, IUserFieldNameBLL> userFieldsMapping,
            ILocalizeText localizer)
        {
            _typesRepository = typesRepository;
            _priorityFinder = priorityFinder;
            _urgencyFinder = urgencyFinder;
            _influenceFinder = influenceFinder;
            _problemCauseFinder = problemCauseFinder;
            _userFinder = userFinder;
            _userFieldsMapping = userFieldsMapping;
            _localizer = localizer;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<Problem> collection)
        {
            var allProblemTypes = _typesRepository.With(x => x.Parent).ToDictionary(x => x.ID);

            collection.WithNumber();
            collection
                .HasProperty(x => x.TypeID)
                .HasName("Тип проблемы")
                .HasConverter(typeID => allProblemTypes.ContainsKey(typeID) ? allProblemTypes[typeID].FullName : string.Empty);            
            collection
                .HasProperty(x => x.UrgencyId)
                .HasName("Срочность")
                .HasFinderLookupConverter(_urgencyFinder, x => x.Name);
            collection
                .HasProperty(x => x.InfluenceId)
                .HasName("Влияние")
                .HasFinderLookupConverter(_influenceFinder, x => x.Name);
            collection.WithPriority(_priorityFinder);
            collection
                .HasProperty(x => x.ProblemCauseId)
                .HasName("Краткое описание причины")
                .HasFinderLookupConverter(_problemCauseFinder, x => x.Name);
            collection.WithDescription();
            collection.HasProperty(x => x.Summary).HasName("Краткое описание");
            collection.HasProperty(x => x.Solution).HasName("Решение");
            collection.HasProperty(x => x.Cause).HasName("Причина");
            collection.HasProperty(x => x.Fix).HasName("Временное решение");
            collection.HasProperty(x => x.UtcDateDetected).HasName("Обнаружена");
            collection.HasProperty(x => x.UtcDatePromised).HasName("Закрыть до");
            collection.HasProperty(x => x.UtcDateSolved).HasName("Решена");
            collection.HasProperty(x => x.UtcDateClosed).HasName("Закрыта");
            collection
                .HasProperty(x => x.OwnerID)
                .HasName("Владелец")
                .HasFinderLookupConverter(_userFinder, x => x.FullName);
            collection.WithUserFields(_userFieldsMapping.Map(UserFieldType.Problem));
            // TODO: Добавить бюджет и обоснование
            collection.WithEntityStateName();
            collection.WithManhours(_localizer);
        }
    }
}
