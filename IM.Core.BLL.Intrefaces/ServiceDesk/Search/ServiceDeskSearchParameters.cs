using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Search
{
    public abstract class ServiceDeskSearchParameters
    {
        public IReadOnlyList<ObjectClass> Classes { get; }

        protected ServiceDeskSearchParameters(IReadOnlyList<ObjectClass> classes)
        {
            Classes = classes;
        }
    }
}