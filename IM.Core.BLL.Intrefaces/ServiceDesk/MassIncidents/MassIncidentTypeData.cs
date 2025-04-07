using System;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    /// <summary>
    /// Этот класс представляет входной контракт данных типа массового инцидента
    /// </summary>
    public class MassIncidentTypeData
    {
        public string Name { get; init; }
        public byte[] RowVersion { get; init; }
        public string WorkflowSchemeIdentifier { get; init; }
        public Guid? FormID { get; init; }

        [Obsolete]
        public string IconName { get; set; }
    }
}
