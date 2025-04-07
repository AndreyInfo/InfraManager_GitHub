using Inframanager;

namespace InfraManager.DAL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет сущность Канал приема массовых инцидентов
    /// </summary>
    [OperationIdMapping(ObjectAction.ViewDetails, OperationID.MassIncident_Properties)]
    [OperationIdMapping(ObjectAction.ViewDetailsArray, OperationID.MassIncident_Add)]
    public class MassIncidentInformationChannel
    {
        /// <summary>
        /// Возвращает идентификатор
        /// </summary>
        public short ID { get; }
        /// <summary>
        /// Возвращает наименование (требуется локализация)
        /// </summary>
        public string Name { get; }

        public string ResourceKey => $"{nameof(MassIncidentInformationChannel)}_{Name}";
    }
}
