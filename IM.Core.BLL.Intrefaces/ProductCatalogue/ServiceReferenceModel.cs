using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue
{
    public class ServiceReferenceModel
    {
        public Guid ID { get; set; }
        public ObjectClass ClassID { get; set; }
        public Guid ServiceID { get; set; }
        public Guid ObjectID { get; set; }
    }
}
