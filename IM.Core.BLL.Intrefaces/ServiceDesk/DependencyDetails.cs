using InfraManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk
{
    public class DependencyDetails
    {
        public Guid ObjectID { get; init; }
        public int ObjectClassID { get; init; }
        public string ObjectName { get; init; }
        public string ObjectLocation { get; init; }
        public string Note { get; init; }
        public DependencyType Type { get; init; }
        public bool IsLocked { get; init; }
    }
}
