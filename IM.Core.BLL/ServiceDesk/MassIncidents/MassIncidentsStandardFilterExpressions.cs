using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassIncidentsStandardFilterExpressions :
        StandardPredicatesDictionary<MassIncident>,
        ISelfRegisteredService<IStandardPredicatesProvider<MassIncident>>
    {
        public MassIncidentsStandardFilterExpressions()
        {
            //TODO: Когда придумают стандартные фильтры к списку массовых инцидентов, тогда их нужно будет сюда добавить
        }
    }
}
