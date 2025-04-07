using InfraManager.BLL.Assets;
using InfraManager.CrossPlatform.WebApi.Contracts.Assets;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Controllers;
using InfraManager.Web.BLL.Tables;

namespace InfraManager.CrossPlatform.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с инсталляциями
    /// </summary>
    [Route("licence-scheme/processors-for-select")]
    [Authorize]
    public class ProcessorsController : ControllerBase
    {
        private readonly IProcessorBLL _assetsDataProvider;

        /// <summary>
        /// Инициализация класса 
        /// </summary>
        /// <param name="assetsDataProvider"></param>
        public ProcessorsController(IProcessorBLL assetsDataProvider)
        {
            _assetsDataProvider = assetsDataProvider?? throw new ArgumentNullException(nameof(assetsDataProvider));
        }

        /// <summary>
        /// Получение списка инсталляций по фильтру
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpPost]
        public async Task<ResultData<List<BaseForTable>>> GetListAsync([FromQuery] ProcessorsListFilter filter, CancellationToken cancellationToken = default)
        {
            var getListResponce = await _assetsDataProvider.GetProcessorsListAsync(filter, cancellationToken);
            if (!getListResponce.Success)
                return new ResultData<List<BaseForTable>>(RequestResponceType.GlobalError, null);

            var result = new List<BaseForTable>();
            foreach (var item in getListResponce.Result)
            {
                result.Add(new ProcessorModelForTable(item));
            }
            return new ResultData<List<BaseForTable>>(RequestResponceType.Success, result);
        }

        public class ProcessorModelForTable : BaseForTable
        {
            private CrossPlatform.WebApi.Contracts.Assets.ProcessorModelModel _item;

            public ProcessorModelForTable(CrossPlatform.WebApi.Contracts.Assets.ProcessorModelModel item)
            {
                _item = item;
            }

            public override Guid ID => _item.ID;
            public string Name => _item.ModelName;
            public String TypeName => _item.TypeName;
            public String ManufacturerName => _item.ManufactorName;
            public string Cores => _item.Cores;
            public int CoefficientValue => 1;

        }


    }
}
