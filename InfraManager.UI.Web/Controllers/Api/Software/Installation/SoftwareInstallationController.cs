using IM.Core.WebApi.Contracts.Common.Models;
using InfraManager.BLL.Software;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation;
using InfraManager.CrossPlatform.WebApi.Contracts.SoftwareInstallation.Models;
using InfraManager.DAL.Software.Installation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.UI.Web.Controllers.Api.Software.Installation
{
    /// <summary>
    /// Контроллер для работы с инсталляциями
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoftwareInstallationController : ControllerBase
    {
        private readonly ISoftwareInstallationBLL _softwareInstallationBLL;

        /// <summary>
        /// Инициализация класса <s
        /// </summary>
        /// <param name="softwareLicenceSchemeDataProvider"></param>
        public SoftwareInstallationController(ISoftwareInstallationBLL softwareInstallationBLL)
        {
            _softwareInstallationBLL = softwareInstallationBLL;
        }

        /// <summary>
        /// Получение списка инсталляций по фильтру
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpPost]
        public async Task<SoftwareInstallationListItem[]> GetListAsync(SoftwareInstallationListFilter filter, CancellationToken cancellationToken = default)
            => await _softwareInstallationBLL.GetListAsync(filter, cancellationToken);
      

        /// <summary>
        /// Получение инсталляции по ИД
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список задач связи между лицензиями и инсталляциями </returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<BaseResult<SoftwareInstallationItem, BaseError>> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _softwareInstallationBLL.GetAsync(id, cancellationToken);
            return result;
        }

        /// <summary>
        /// Сохранение / создание инсталяции
        /// </summary>
        /// <param name="item">Моель инсталяции</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        [HttpPut]
        [Route("")]
        public async Task<BaseResult<Guid, SoftwareInstallationRules>> SaveAsync(SoftwareInstallationItem item, CancellationToken cancellationToken = default)
        {
            var result = await _softwareInstallationBLL.SaveAsync(item, cancellationToken);
            return result;
        }
        /// <summary>
        /// Удаление списка зависимых инсталляций
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpPost]
        [Route("{id}/delete-dependant")]
        public async Task<BaseResult<bool, BaseError>> DeleteDependantAsync([FromRoute] Guid id, GuidList list, CancellationToken cancellationToken = default)
        {
            var result = await _softwareInstallationBLL.DeleteDependantAsync(id, list, cancellationToken);
            return result;
        }

        /// <summary>
        /// добавление к списку зависимых инсталляций
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpPost]
        [Route("{id}/add-dependant")]
        public async Task<BaseResult<bool, BaseError>> AddDependantAsync([FromRoute] Guid id, GuidList list, CancellationToken cancellationToken = default)
        {
            var result = await _softwareInstallationBLL.AddDependantAsync(id, list, cancellationToken);
            return result;
        }
        /// <summary>
        /// добавление к списку зависимых инсталляций
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpGet]
        [Route("{id}/licence-reference")]
        public async Task<BaseResult<List<SoftwareLicenceUseListItem>, BaseError>> GetLicenceUseAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _softwareInstallationBLL.GetLicenceUseAsync(id, cancellationToken);
            return result;
        }
    }
}
