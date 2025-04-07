using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.Asset;
using Microsoft.Extensions.Logging;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

internal class MaterialConsumptionRateBLL : 
    StandardBLL<Guid, MaterialConsumptionRate, MaterialConsumptionRateInputDetails, MaterialConsumptionRateOutputDetails, MaterialConsumptionRateFilter>
    , IMaterialConsumptionRateBLL
    , ISelfRegisteredService<IMaterialConsumptionRateBLL>
{
    public MaterialConsumptionRateBLL(IRepository<MaterialConsumptionRate> repository
        , ILogger<MaterialConsumptionRateBLL> logger
        , IUnitOfWork unitOfWork
        , ICurrentUser currentUser
        , IBuildObject<MaterialConsumptionRateOutputDetails, MaterialConsumptionRate> detailsBuilder
        , IInsertEntityBLL<MaterialConsumptionRate, MaterialConsumptionRateInputDetails> insertEntityBLL
        , IModifyEntityBLL<Guid, MaterialConsumptionRate, MaterialConsumptionRateInputDetails, MaterialConsumptionRateOutputDetails> modifyEntityBLL
        , IRemoveEntityBLL<Guid, MaterialConsumptionRate> removeEntityBLL
        , IGetEntityBLL<Guid, MaterialConsumptionRate, MaterialConsumptionRateOutputDetails> detailsBLL
        , IGetEntityArrayBLL<Guid, MaterialConsumptionRate, MaterialConsumptionRateOutputDetails, MaterialConsumptionRateFilter> detailsArrayBLL) 
        : base(repository
            , logger
            , unitOfWork
            , currentUser
            , detailsBuilder
            , insertEntityBLL
            , modifyEntityBLL
            , removeEntityBLL
            , detailsBLL
            , detailsArrayBLL)
    {
    }

    

    public Task<MaterialConsumptionRateOutputDetails[]> GetDetailsPageAsync(MaterialConsumptionRateFilter filter, ClientPageFilter pageFilter, CancellationToken token)
    {
        var pageFilterWithOrder = new ClientPageFilter()
        {
            Ascending = pageFilter.Ascending,
            OrderByProperty = pageFilter.OrderByProperty ?? nameof(MaterialConsumptionRate.DeviceCategoryID),
            Skip = pageFilter.Skip,
            Take = pageFilter.Take
        };
        
        return base.GetDetailsPageAsync(filter, pageFilterWithOrder, token);
    }

   
}