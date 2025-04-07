using Inframanager.BLL;
using System;
using InfraManager.BLL.ServiceDesk.FormDataValue;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    public class MassIncidentData : BaseMassIncidentData
    {
        public NullablePropertyWrapper<Guid> CreatedByUserID { get; init; }
        public NullablePropertyWrapper<Guid> OwnedByUserID { get; init; }
        public NullablePropertyWrapper<Guid> GroupID { get; init; }
        public NullablePropertyWrapper<int> CauseID { get; init; }
        public NullablePropertyWrapper<Guid> ExecutedByUserID { get; init; }
        public Guid? ServiceID { get; init; }
        public int? TypeID { get; init; }
        public short? InformationChannelID { get; init; }
        public string UtcDateRegistered { get; init; }
        public string UtcDateAccomplished { get; init; }
        public string UtcDateCreated { get; init; }
        public string UtcOpenedAt { get; init; }
        public string UtcCloseUntil { get; init; }
        public string UtcDateClosed { get; init; }
        public NullableString EntityStateID { get; init; }
        public NullableString EntityStateName { get; init; }
        public NullablePropertyWrapper<Guid> WorkflowSchemeID { get; init; }
        public string WorkflowSchemeVersion { get; init; }

        /// <summary>
        /// Возвращает или задает данные формы доп. параметров.
        /// </summary>
        public FormValuesData FormValuesData { get; init; }
        
        /// <summary>
        /// Признак необходимости расчета SLA
        /// </summary>
        public RefreshAgreement? RefreshAgreement { get; init; }

        public bool ServiceFieldSet => !string.IsNullOrWhiteSpace(UtcCloseUntil)
            || !string.IsNullOrWhiteSpace(UtcDateAccomplished)
            || !string.IsNullOrWhiteSpace(UtcDateRegistered)
            || !string.IsNullOrWhiteSpace(UtcOpenedAt);
    }
}
