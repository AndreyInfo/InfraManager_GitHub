using InfraManager.CrossPlatform.WebApi.Contracts.ELP;
using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.ELP;

namespace InfraManager.CrossPlatform.BLL.Intrefaces.ELP
{
    public interface IELPSettingBLL
    {
        /// <summary>
        /// Получает список связей между инсталляциями и лицензиями
        /// </summary>
        /// <param name="listFilter">Фильтрация</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Список связей между инсталляциями и лицензиями</returns>
        Task<ELPListItem[]> GetListAsync(ELPListFilter listFilter, CancellationToken cancellationToken);
        /// <summary>
        /// Получает данные cвязи между инсталляциями и лицензиями<
        /// </summary>
        /// <param name="id">Идентификатор связи</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные cвязи между инсталляциями и лицензиями</returns>
        Task<ELPSettingDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Добавляет cвязь между инсталляциями и лицензиями
        /// </summary>
        /// <param name="data">Данные связи</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные новой cвязи между инсталляциями и лицензиями</returns>
        Task<ELPSettingDetails> AddAsync(ELPItem data, CancellationToken cancellationToken);
        /// <summary>
        /// Удаляет cвязь между инсталляциями и лицензиями
        /// </summary>
        /// <param name="id">Идентификатор связи связи</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        /// <summary>
        /// Изменяет cвязь между инсталляциями и лицензиями
        /// </summary>
        /// <param name="id">Идентификатор связи</param>
        /// <param name="data">Данные, которые надо изменить</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Данные измененной cвязи между инсталляциями и лицензиями</returns>
        Task<ELPSettingDetails> UpdateAsync(Guid id, ELPItem data, CancellationToken cancellationToken);
    }
}