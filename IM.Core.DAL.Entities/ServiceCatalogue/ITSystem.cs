using InfraManager.DAL.Asset;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue
{
    public class ITSystem
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public byte[] RowVersion { get; set; }
        public Guid? InfrastructureSegmentID { get; set; }
        public Guid? CriticalityID { get; set; }
        public DateTime? DateAnnulated { get; set; }
        public DateTime? DateReceived { get; set; }
        public Guid? ClientID { get; set; }
        public int? ClientClassID { get; set; }
        public Guid? ProductCatalogTypeID { get; set; }

        public virtual InfrastructureSegment InfrastructureSegment { get; }
    }
}
