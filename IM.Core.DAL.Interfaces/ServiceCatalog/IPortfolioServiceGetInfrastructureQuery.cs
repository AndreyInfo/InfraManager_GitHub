using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceDesk
{
    
    public interface IPortfolioServiceGetInfrastructureQuery
    {
        Task<PortfolioServiceInfrastructureItem[]> ExecuteAsync(Guid serviceID,
            CancellationToken cancellationToken = default);
    }
}
