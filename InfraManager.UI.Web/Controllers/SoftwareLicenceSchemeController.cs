using InfraManager.BLL.Software;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareLicenceScheme.Models;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.CrossPlatform.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы со схемами лицензирования
    /// </summary>
    [Route("CoreApi/software-licence-scheme")]
    [Route("licence-scheme")]
    [Authorize]
    [ApiController]
    public class SoftwareLicenceSchemeController : ControllerBase
    {
        private readonly ISoftwareLicenceSchemeBLL _softwareLicenceSchemeDataProvider;

        /// <summary>
        /// Инициализирует экземпляр <see cref="SoftwareLicenceSchemeController"/>.
        /// </summary>
        /// <param name="softwareLicenceSchemeDataProvider">дата провайдер для работы со схемами лицензирования</param>
        public SoftwareLicenceSchemeController(ISoftwareLicenceSchemeBLL softwareLicenceSchemeDataProvider) =>
            _softwareLicenceSchemeDataProvider = softwareLicenceSchemeDataProvider ?? throw new ArgumentNullException(nameof(softwareLicenceSchemeDataProvider));

        /// <summary>
        /// Получить схему лицензирования по id
        /// </summary>
        /// <param name="id">идентификатор схемы лицензирования</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> схема лицензирования + результат операции </returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<BaseResult<SoftwareLicenceScheme, BaseError>> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _softwareLicenceSchemeDataProvider.GetAsync(id, cancellationToken);
        }

        /// <summary>
        /// Получить схему лицензирования по фильтру
        /// </summary>
        /// <param name="filter">фильтр схем лицензирования <see cref="SoftwareLicenceSchemeListFilter"/>.</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат получения схем лицензирования по фильтру + результат операции</returns>
        [HttpGet]
        [Route("")]
        public async Task<BaseResult<List<SoftwareLicenceSchemeListItem>, BaseError>> GetListAsync ([FromQuery] SoftwareLicenceSchemeListFilter request, CancellationToken cancellationToken)
        {
            return await _softwareLicenceSchemeDataProvider.GetListAsync(request.SearchText, request.ShowDeleted, null, cancellationToken);
        }

        [HttpGet("list-for-select")]
        public async Task<BaseResult<List<SoftwareLicenceSchemeListItem>, BaseError>> GetListAsyncListForSelect([FromForm] InfraManager.Web.DTL.Tables.AssetModelSearchTableLoadRequestInfo requestInfo)
        {
            SoftwareLicenceSchemeListFilter filter = new SoftwareLicenceSchemeListFilter()
            {
                SearchText = requestInfo.SearchRequest,
            };
            CancellationToken cancellationToken = default;

            return await _softwareLicenceSchemeDataProvider.GetListAsync(filter.SearchText, filter.ShowDeleted, null, cancellationToken);
        }

        /// <summary>
        /// Сохранение схемы лицензирования
        /// </summary>
        /// <param name="scheme">  модель схемы лицензирования <see cref="SoftwareLicenceScheme"/> </param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> идентификатор схемы лицензирования + результат операции </returns>
        [HttpPut]
        [Route("")]
        public async Task<BaseResult<Guid, SoftwareLicenceSchemeRules>> SaveAsync([FromBody]SoftwareLicenceScheme scheme, CancellationToken cancellationToken = default)
        {
            var result = await _softwareLicenceSchemeDataProvider.SaveAsync(scheme, cancellationToken);
            return result;
        }

        /// <summary>
        /// Сохранение схемы лицензирования
        /// </summary>
        /// <param name="scheme">  модель схемы лицензирования <see cref="SoftwareLicenceScheme"/> </param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> идентификатор схемы лицензирования + результат операции </returns>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync([FromBody] SoftwareLicenceScheme model, CancellationToken cancellationToken = default)
        {
            var result = await _softwareLicenceSchemeDataProvider.SaveAsync(model, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Пометрка схем лицензирования как удаленные
        /// </summary>
        /// <param name="request">запрос на пометку схемы лицензирования удаленной  <see cref="SoftwareLicenceSchemeDeleteRequest"/> </param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPost]
        [Route("mark-as-deleted")]
        public async Task<BaseResult<SoftwareLicenceSchemeDeleteResponse, SoftwareLicenceSchemeRules>> DeleteAsync([FromBody] SoftwareLicenceSchemeDeleteRequest request, CancellationToken cancellationToken = default)
        {
             return await _softwareLicenceSchemeDataProvider.DeleteRestoreAsync(request, false, cancellationToken);
        }

        /// <summary>
        /// Снятие пометки со схем лицензирования  удаленные
        /// </summary>
        /// <param name="request">запрос на снятие пометки удаленная со схемы лицензирования <see cref="SoftwareLicenceSchemeDeleteRequest"/> </param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат операции </returns>
        [HttpPost]
        [Route("unmark-as-deleted")]
        public async Task<BaseResult<SoftwareLicenceSchemeDeleteResponse, SoftwareLicenceSchemeRules>> ResotreAsync([FromBody] SoftwareLicenceSchemeDeleteRequest request, CancellationToken cancellationToken = default)
        {
            return await _softwareLicenceSchemeDataProvider.DeleteRestoreAsync(request, true, cancellationToken);
        }

        /// <summary>
        /// Получение списка типов схем лицензирвования
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns>список типов схем лицензирования + результат операции</returns>
        [HttpGet]
        [Route("~/licence-object")]
        public async Task<List<ListItem>> GetLicenseObjectTypesAsync(CancellationToken cancellationToken = default)
        {
            var respose = await _softwareLicenceSchemeDataProvider.GetLicenseObjectTypesAsync(cancellationToken);
            return respose.Result.Select(item => new ListItem() { Id = item.Id, Name = item.Name }).ToList();
        }

        [HttpPost]
        [Route("{field}/validate")]
        public async Task<ExpressionValidationResponse> ValidateFieldValue(string field, [FromBodyOrForm] LicenceCountUpdateRequest request, CancellationToken cancellationToken = default)
        {
            var model = new ValidateFieldValueModel()
            {
                Value = request.Expression
            };
            return await _softwareLicenceSchemeDataProvider.Validate(field, model.Value, cancellationToken);
        }

        [HttpGet]
        [Route("statements")]
        public async Task<ResultData<ExpressionStatementsModel>> GetLicenceExpressionStatements()
        {
            CancellationToken cancellationToken = default;

            var response = await _softwareLicenceSchemeDataProvider.GetLicenceExpressionStatements(cancellationToken);
            if (!response.Success)
            {
                return new ResultData<ExpressionStatementsModel>(RequestResponceType.GlobalError, null);
            }

            return new ResultData<ExpressionStatementsModel>(RequestResponceType.Success, response.Result);
        }
    }
}
