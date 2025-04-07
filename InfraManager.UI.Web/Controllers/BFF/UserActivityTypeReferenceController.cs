using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.UsersActivityType.Obsolete;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Authorize]
    [ApiController]
    [Route("/api/activity_reference/")]
    [Obsolete("Use '/api/useractivitytypes' instead.")]
    public class UserActivityTypeReferenceController : ControllerBase //TODO refactor Controller Paths // todo: Удалить когда админка переедет на '/api/useractivitytypes'
    {
        private readonly IUserActivityTypeReferenceBLL _service;

        public UserActivityTypeReferenceController(IUserActivityTypeReferenceBLL service)
        {
            _service = service;
        }

        /// <summary>
        /// Метод отдает все привязанные виды деятельности из базы данных в виде идентификаторов
        /// </summary>
        /// <param name="id">идентификатор пользователя или группы</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список видов деятельности без вложенных элементов </returns>
        [HttpGet("raw/{id}")]
        [Obsolete("Use '/api/useractivitytypes' instead.")]
        public async Task<UserActivityTypeReferenceDetails[]> GetListByIDAsync(Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _service.GetListByIdAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод отдает все привязанные виды деятельности из базы данных в виде идентификаторов
        /// </summary>
        /// <param name="id">идентификатор пользователя или группы</param>
        /// <param name="filter">Стандарьный фильтр сортировки</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список видов деятельности </returns>
        [HttpGet("{id}")]
        [Obsolete("Use '/api/useractivitytypes' instead.")]
        public async Task<UserActivityTypePathDetails[]> GetListUserActivityTypeByIDAsync(Guid id,
            [FromQuery] BaseFilter filter, CancellationToken cancellationToken = default)
        {
            return await _service.GetListUserActivityTypeByIdAsync(id, filter, cancellationToken);
        }


        /// <summary>
        /// Метод добавляет связи видов деятельности с пользователем или группой
        /// </summary>
        /// <param name="models">модели связей видов деятельности</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> id созданных связей видов деятельности </returns>
        [HttpPost]
        public async Task<Guid[]> InsertAsync([FromBody] UserActivityTypeReferenceDetails[] models,
            CancellationToken cancellationToken = default)
        {
            return await _service.InsertAsync(models, cancellationToken);
        }

        /// <summary>
        /// Метод удаляет связи видов деятельности с пользователем или группой
        /// </summary>
        /// <param name="ids">идентификаторы связей</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результаты успешности операции </returns>
        [HttpDelete]
        public async Task DeleteAsync([FromBody] Guid[] ids, CancellationToken cancellationToken = default)
        {
            await _service.DeleteAsync(ids, cancellationToken);
        }
    }
}
