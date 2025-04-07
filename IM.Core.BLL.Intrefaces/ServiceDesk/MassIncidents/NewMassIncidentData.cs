using System;
using InfraManager.BLL.ServiceDesk.FormDataValue;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс описывает входной контракт для создания массовых инцидентов.
    /// </summary>
    public class NewMassIncidentData : BaseMassIncidentData
    {
        public Guid ServiceID { get; init; }
        public Guid[] AffectedServiceIDs { get; init; }
        public int TypeID { get; init; }
        public int? CauseID { get; init; }
        public short InformationChannelID { get; init; }        
        public Guid[] DocumentIds { get; init; }
        public Guid? CreatedByUserID { get; init; }
        public Guid? OwnedByUserID { get; init; }
        public Guid? ExecutedByGroupID { get; init; }
        public Guid? ExecutedByUserID { get; init; }
        public Guid[] Calls { get; init; }

        /// <summary>
        /// Возвращает или задает данные формы доп. параметров.
        /// </summary>
        public FormValuesData FormValuesData { get; init; }
    }
}
