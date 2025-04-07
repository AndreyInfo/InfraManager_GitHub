using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class SLAServiceReferenceData
    {
        public Guid ID { get; init; }

        public Guid ServiceID { get; init; }

        public Guid ObjectID { get; init; }

        public string ObjectName { get; init; }

        public string ObjectLocation { get; init; }

        public int ObjectClassID { get; init; }

        public string ObjectClassName { get; init; }

    }
}
