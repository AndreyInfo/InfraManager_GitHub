using Inframanager.BLL;
using InfraManager.BLL.ServiceCatalogue.PortfolioServices.Services;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class ServiceBLL :
        StandardBLL<Guid, Service, ServiceData, ServiceDetails, ServiceListFilter>,
        IServiceBLL,
        ISelfRegisteredService<IServiceBLL>
    {
        public ServiceBLL(
            IRepository<Service> repository,
            ILogger<ServiceBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ServiceDetails, Service> detailsBuilder,
            IInsertEntityBLL<Service, ServiceData> insertEntityBLL,
            IModifyEntityBLL<Guid, Service, ServiceData, ServiceDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, Service> removeEntityBLL,
            IGetEntityBLL<Guid, Service, ServiceDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, Service, ServiceDetails, ServiceListFilter> detailsArrayBLL) 
            : base(
                  repository,
                  logger,
                  unitOfWork,
                  currentUser,
                  detailsBuilder,
                  insertEntityBLL,
                  modifyEntityBLL,
                  removeEntityBLL,
                  detailsBLL,
                  detailsArrayBLL)
        {
        }

        public async Task<ServiceDetails[]> GetDetailsPageAsync(ServiceListFilter filterBy, CancellationToken cancellationToken = default) =>
            await GetDetailsPageAsync(filterBy, filterBy, cancellationToken);
    }
}
