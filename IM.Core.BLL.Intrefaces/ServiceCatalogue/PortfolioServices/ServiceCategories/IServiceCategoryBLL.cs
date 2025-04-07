using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices.ServiceCategories;

[Obsolete("Выпилить")]
public interface IServiceCategoryBLL
{    
    Task<ServiceCategoryDetailsModel[]> ListAsync(
        ServiceCategoryFilter filter,
        CancellationToken cancellationToken = default);
}
