using System;
using System.Collections.Generic;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.DAL
{
    public class ServiceTemplate
    {
        public Guid ID { get; set; }

        public string Name { get; set; }
        public string Note { get; set; }

        public Guid ServiceID { get; set; }

        public byte[] RowVersion { get; set; }

        public virtual ICollection<Rule> Rules { get; set; }
    }
}
