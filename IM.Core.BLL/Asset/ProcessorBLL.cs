using InfraManager.CrossPlatform.WebApi.Contracts.Assets;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Asset;
using InfraManager.DAL.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Assets
{
    internal class ProcessorBLL : IProcessorBLL, ISelfRegisteredService<IProcessorBLL>
    {    
        /// <summary>
        /// Запрос списка процессоров
        /// </summary>
        private readonly IProcessorsListQuery _query;

        public ProcessorBLL(IProcessorsListQuery listQuery)
        {           
            _query = listQuery ?? throw new ArgumentNullException(nameof(listQuery));
        }

        public async Task<BaseResult<List<ProcessorModelModel>, BaseError>> GetProcessorsListAsync(
            ProcessorsListFilter filter, 
            CancellationToken cancellationToken)
        {
            Guid? catalogID = filter.ProductCatalogID == Guid.Empty || filter.ProductCatalogClassID != (int)ObjectClass.ProductCatalogType ? null : filter.ProductCatalogID;
            Guid? categoryID = filter.ProductCatalogID == Guid.Empty || filter.ProductCatalogClassID != (int)ObjectClass.ProductCatalogCategory ? null : filter.ProductCatalogID;

            var result = await _query.QueryAsync(
                new ProcessorsQueryRequest(catalogID, categoryID, (ProductTemplate)AssetConstants.ProductCatalogTemplate_Processor, filter.SearchText), 
                cancellationToken);

            return new BaseResult<List<ProcessorModelModel>, BaseError>(
                result
                    .Select(x => new ProcessorModelModel()  // TODO: аватомаппер нужно использовтаь
                    { 
                        ID = x.AdapterType.IMObjID,
                        TypeName = x.AdapterType.ProductCatalogType.Name,
                        ModelName = x.AdapterType.Name,
                        ManufactorName = x.Manufacturer?.Name,
                        Cores = x.Processor?.NumberOfCores
                    })
                    .ToList(), 
                null);
        }
    }
}
