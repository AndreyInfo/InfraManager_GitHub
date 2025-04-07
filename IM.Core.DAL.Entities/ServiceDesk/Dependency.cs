using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    public abstract class Dependency
    {
        protected Dependency() { }
        protected Dependency(Guid ownerObjectID, InframanagerObject inframanagerObject)
        {
            OwnerObjectID = ownerObjectID;
            ObjectID = inframanagerObject.Id;
            ObjectClassID = inframanagerObject.ClassId;
        }
        public Guid OwnerObjectID { get; init; }
        public Guid ObjectID { get; init; }
        public ObjectClass ObjectClassID { get; init; }
        public string ObjectName { get; set; }
        public string ObjectLocation { get; set; }
        public string Note { get; set; }
        public DependencyType Type { get; set; }
        public bool Locked { get; set; }

    }
}
