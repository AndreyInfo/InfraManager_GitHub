using Inframanager.BLL.Events;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk.Manhours;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Manhours.Events
{
    internal class WorkEventParamsConfigurer : IConfigureDefaultEventParamsBuilderCollection<ManhoursWork>
    {
        private readonly IReadonlyRepository<UserActivityType> _activityTypesRepository;
        private readonly IFindEntityByGlobalIdentifier<User> _userFinder;

        public WorkEventParamsConfigurer(
            IReadonlyRepository<UserActivityType> activityTypesRepository, 
            IFindEntityByGlobalIdentifier<User> userFinder)
        {
            _activityTypesRepository = activityTypesRepository;
            _userFinder = userFinder;
        }

        public void Configure(IDefaultEventParamsBuildersCollection<ManhoursWork> collection)
        {
            var allActivityTypes = _activityTypesRepository.With(x => x.Parent).ToDictionary(x => x.ID);

            collection
                .HasProperty(x => x.Number)
                .HasName("Название")
                .HasConverter(n => n == default ? string.Empty : $"Новая работа"); // TODO: старый механизм не позволял писать в историю значения, генерируемые на стороне БД, отсюда и такое странное значение
            collection.HasProperty(x => x.Description).HasName("Описание");
            collection
                .HasProperty(x => x.UserActivityTypeID)
                .HasName("Вид деятельности")
                .HasConverter(activityTypeID => activityTypeID.HasValue ? allActivityTypes[activityTypeID.Value].FullName : null);
            collection
                .HasProperty(x => x.InitiatorID)
                .HasName("Инициатор")
                .HasFinderLookupConverter(_userFinder, user => user.FullName);
            collection
                .HasProperty(x => x.ExecutorID)
                .HasName("Исполнитель")
                .HasFinderLookupConverter(_userFinder, user => user.FullName);
        }
    }
}
