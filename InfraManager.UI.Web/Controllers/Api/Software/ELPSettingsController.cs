using InfraManager.CrossPlatform.BLL.Intrefaces.ELP;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ELP;
using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using ELPListItem = InfraManager.BLL.ELP.ELPListItem;

namespace InfraManager.UI.Web.Controllers.Api.Software
{
    /// <summary>
    /// Контроллер для работы с задачами связи между лицензиями и инсталляциями
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    public class ELPSettingsController : ControllerBase
    {
        private readonly IELPSettingBLL _elpSettingBLL;

        /// <summary>
        /// Инициализация класса 
        /// </summary>
        /// <param name="elpSettingBLL"></param>
        public ELPSettingsController(IELPSettingBLL elpSettingBLL)
        {
            _elpSettingBLL = elpSettingBLL ?? throw new ArgumentNullException(nameof(elpSettingBLL));
        }

        /// <summary>
        /// Получение списка задач связи по фильтру
        /// </summary>
        /// <param name="filter">фильтр</param>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список инсталляций </returns>
        [HttpGet]
        [Route("")]
        public async Task<ELPListItem[]> GetListAsync([FromQuery] ELPListFilter filter, CancellationToken cancellationToken)
        {
            return await _elpSettingBLL.GetListAsync(filter, cancellationToken);
        }

        /// <summary>
        /// Получение задачи связи между лицензиями и инсталляциями по ИД
        /// </summary>
        /// <param name="cancellationToken"> токен отмены</param>
        /// <returns> список задач связи между лицензиями и инсталляциями </returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ELPSettingDetails> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _elpSettingBLL.DetailsAsync(id, cancellationToken);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ELPSettingDetails> UpdateAsync(Guid id, ELPItem item, CancellationToken cancellationToken = default)
        {
            return await _elpSettingBLL.UpdateAsync(id, item, cancellationToken);
        }

        [HttpPost]
        public async Task<ELPSettingDetails> AddAsync(ELPItem item, CancellationToken cancellationToken = default)
        {
            return await _elpSettingBLL.AddAsync(item, cancellationToken);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _elpSettingBLL.DeleteAsync(id, cancellationToken);
        }
    }
}
