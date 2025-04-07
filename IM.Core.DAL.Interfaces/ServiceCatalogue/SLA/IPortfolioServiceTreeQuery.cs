using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.ServiceCatalogue.SLA;

public interface IPortfolioServiceTreeQuery
{
    Task<PortfolioServicesItem[]>
        ExecuteAsync(Guid? slaID, Guid parentID, CancellationToken cancellationToken = default);
}