using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет фильтр списка типов массовых инцидентов
    /// </summary>
    public class MassIncidentTypeListFilter : ClientPageFilter<MassIncidentType>
    {
        public MassIncidentTypeListFilter()
        {
            OrderByProperty = nameof(MassIncidentType.ID);
        }
    }
}
