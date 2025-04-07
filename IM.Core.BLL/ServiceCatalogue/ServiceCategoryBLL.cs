using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue
{
    internal class ServiceCategoryBLL : 
        StandardBLL<Guid, ServiceCategory, ServiceCategoryData, ServiceCategoryDetails, ServiceCategoryListFilter>,
        IServiceCategoryBLL,
        ISelfRegisteredService<IServiceCategoryBLL>
    {
        public ServiceCategoryBLL(
            IRepository<ServiceCategory> repository,
            ILogger<ServiceCategoryBLL> logger,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            IBuildObject<ServiceCategoryDetails, ServiceCategory> detailsBuilder,
            IInsertEntityBLL<ServiceCategory, ServiceCategoryData> insertEntityBLL,
            IModifyEntityBLL<Guid, ServiceCategory, ServiceCategoryData, ServiceCategoryDetails> modifyEntityBLL,
            IRemoveEntityBLL<Guid, ServiceCategory> removeEntityBLL,
            IGetEntityBLL<Guid, ServiceCategory, ServiceCategoryDetails> detailsBLL,
            IGetEntityArrayBLL<Guid, ServiceCategory, ServiceCategoryDetails, ServiceCategoryListFilter> detailsArrayBLL) 
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

        public async Task<ServiceCategoryDetails[]> GetDetailsPageAsync(ServiceCategoryListFilter filterBy, CancellationToken cancellationToken = default) =>        
            await GetDetailsPageAsync(filterBy, filterBy, cancellationToken);
        
    }
}
