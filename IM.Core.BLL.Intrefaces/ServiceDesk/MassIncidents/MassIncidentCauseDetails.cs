using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk.MassIncidents;
using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет фильтр списка причин массовых инцидентов
    /// </summary>
    public class MassIncidentCauseDetails : LookupDetails<int>
    {
        public Guid IMObjID { get; init; }
    }
}
