using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.UsersActivityType.Obsolete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfraManager.UI.Web.Controllers.BFF
{
    [Authorize]
    [ApiController]
    [Route("/api/activities/")]
    [Obsolete("Use '/api/useractivitytypes' instead.")]
    public class UserActivityTypeController : ControllerBase // todo: Удалить когда админка переедет на '/api/useractivitytypes'
    {
        private readonly IUserActivityTypeBLL _service;

        public UserActivityTypeController(IUserActivityTypeBLL service)
        {
            _service = service;
        }
        /// <summary>
        /// Метод отдает все виды деятельности из базы данных
        /// </summary>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список видов деятельности </returns>
        [HttpGet]
        [Obsolete("Use '/api/useractivitytypes' instead.")]
        public async Task<UserActivityTypeDetails[]> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _service.ListAsync(cancellationToken);
        }

        /// <summary>
        /// Метод обновляет вид деятельности в базе данных
        /// </summary>
        /// <param name="userActivityModel">модель вида деятельности</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> id вида деятельности</returns>
        [HttpPut]
        public async Task<Guid> UpdateAsync(UserActivityTypeDetails userActivityModel, CancellationToken cancellationToken = default)
        {
            return await _service.UpdateAsync(userActivityModel, cancellationToken);
        }

        /// <summary>
        /// Метод создает вид деятельности в базе данных
        /// </summary>
        /// <param name="userActivityModel">модель вида деятельности</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> id созданного вида деятельности </returns>
        [HttpPost]
        public async Task<Guid> CreateAsync(UserActivityTypeDetails userActivityModel, CancellationToken cancellationToken = default)
        {
            return await _service.CreateAsync(userActivityModel, cancellationToken);
        }

        /// <summary>
        /// Метод удаляет вид деятельности в базе данных
        /// </summary>
        /// <param name="id">идентификатор вида деятельности</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> результат удаления </returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _service.DeleteAsync(id, cancellationToken);
        }

        /// <summary>
        /// Метод ищет типы деятельности по id родителя в базе данных
        /// </summary>
        /// <param name="parentID">идентификатор родителя</param>
        /// <param name="cancellationToken">отмена задачи</param>
        /// <returns> список видов деятельности </returns>
        [HttpGet("tree")]
        [Obsolete("Use '/api/useractivitytypes' instead.")]
        public async Task<UserActivityTypeDetails[]> FindAsync(Guid? parentID, CancellationToken cancellationToken = default)
        {
            return await _service.FindByParentAsync(parentID, cancellationToken);
        }
    }
}
