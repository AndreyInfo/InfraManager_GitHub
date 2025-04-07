using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Asset
{
    public interface IELPSettingDataProvider
    {
        /// <summary>
        /// Возвращает список всех свзяей с опуциональной фильтрацией по имени
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IList<ElpSetting>> GetListAsync(string searchText, CancellationToken cancellationToken);
        /// <summary>
        /// Возвращает связь по идентификатор
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ElpSetting> GetAsync(Guid ID, CancellationToken cancellationToken);
        /// <summary>
        /// Добавляет новый элемент связи
        /// </summary>
        /// <param name="elpsetting"></param>
        void Add(ElpSetting elpsetting);
        /// <summary>
        /// Возвращет связь по имени
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ElpSetting> GetByNameAsync(string name, CancellationToken cancellationToken);
        void Remove(ElpSetting elpsetting);
    }
}
