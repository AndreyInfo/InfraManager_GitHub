using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс описывает выходной контракт данных типа массового инцидента
    /// </summary>
    public class MassIncidentTypeDetails : MassIncidentTypeData
    {
        public int ID { get; init; }
        public Guid IMObjID { get; init; }
    }
}
