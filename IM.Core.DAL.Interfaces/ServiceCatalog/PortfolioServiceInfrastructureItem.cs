using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.DAL.ProductCatalogue
{
    public class PortfolioServiceInfrastructureItem
    {
        public Guid ID { get; init; }
        public Guid ServiceID { get; init; }
        public ObjectClass ClassID { get; init; }
        public Guid ObjectID { get; init; }
        public string Name { get; init; }
        public string Location { get; init; }
        public string Category { get; init; }
    }
}
