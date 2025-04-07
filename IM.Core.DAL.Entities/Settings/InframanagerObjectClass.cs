using System;
using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.MaintenanceWork;
using System.Collections.Generic;

namespace InfraManager.DAL.Settings
{
    [Obsolete("Не использовать")]
    public class InframanagerObjectClass
    {
        public ObjectClass ID { get; init; }
        
        public string Name { get; set; }

        public virtual ICollection<Operation> Operations { get; init; }
        public virtual ICollection<MaintenanceDependency> MaintenanceDependencies { get; init; }
    }
}
