using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices;

namespace IM.Core.HttpClient.ServiceCatalog;

public interface ISupportLineResponsibleClient
{
    Task<SupportLineResponsibleDetails[]> GetResponsibleAsync(Guid guid, Guid? userId = null,
        CancellationToken cancellationToken = default(CancellationToken));
}