using System;

namespace InfraManager.BLL.ServiceCatalogue
{
    public class SLAReferenceDetails : SLAReferenceData
    {
        public Guid SLAID { get; init; }
        
        public Guid ObjectID { get; init; }
        
        public ObjectClass ClassID { get; init; }
    }
}
