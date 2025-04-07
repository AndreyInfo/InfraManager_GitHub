using InfraManager.BLL.Software;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Controllers;

namespace InfraManager.CrossPlatform.WebApi.Controllers.Api.Software.Licence
{
    /// <summary>
    /// Этот класс реализует контроллер REST Api ресурса SoftwareLicenceScheme
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SoftwareLicenceSchemesController : BaseApiController   // ControllerBase
    {
        private readonly ISoftwareLicenceSchemeBLL _softwareLicenceSchemeDataProvider;
        
        /// <summary>
        /// Инициализирует экземпляр <see cref="SoftwareLicenceSchemesController"/>.
        /// </summary>
        /// <param name="softwareLicenceSchemeDataProvider">дата провайдер для работы со схемами лицензирования</param>
        public SoftwareLicenceSchemesController(ISoftwareLicenceSchemeBLL softwareLicenceSchemeDataProvider)
        {
            _softwareLicenceSchemeDataProvider = softwareLicenceSchemeDataProvider ?? throw new ArgumentNullException(nameof(softwareLicenceSchemeDataProvider));
        }

        #region TODO: move to another controller methods

        /// <summary>
        /// Получение списка видов схем лицензирования
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns>список видов схем лицензирования + результат операции</returns>
        [HttpGet]
        [Route("scheme-types")]
        public async Task<BaseResult<List<ListItem>, BaseError>> GetSchemeTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _softwareLicenceSchemeDataProvider.GetSchemeTypesAsync(cancellationToken);
        }

        #endregion
    }
}
