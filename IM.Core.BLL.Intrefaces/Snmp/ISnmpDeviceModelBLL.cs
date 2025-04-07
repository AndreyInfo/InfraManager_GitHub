using Inframanager.BLL;
using InfraManager.DAL.Snmp;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Snmp
{
    /// <summary>
    /// Бизнес логика для работы с объектами «Правило распознавания»
    /// </summary>
    public interface ISnmpDeviceModelBLL
    {
        /// <summary>
        /// Добавление нового объекта «Правило распознавания»
        /// </summary>
        /// <param name="dataModel">Модель данных</param>
        /// <param name="cancellationToken">токен отмены</param>
        /// <returns>Модель объекта «Правило распознавания»</returns>
        Task<SnmpDeviceModelDetails> AddAsync(SnmpDeviceModelData dataModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохранение изменений объекта «Правило распознавания»
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="dataModel">Модель данных</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Модель объекта «Правило распознавания»</returns>
        Task<SnmpDeviceModelDetails> UpdateAsync(Guid id, SnmpDeviceModelData dataModel, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удавление конкрентного объекта «Правило распознавания»
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение конкрентного объекта «Правило распознавания»
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Модель объекта «Правило распознавания»</returns>
        Task<SnmpDeviceModelDetails> DetailsAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение коллекции объектов «Правило распознавания»
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="pageFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SnmpDeviceModelDetails[]> GetDetailsPageAsync(SnmpDeviceModelFilter filterBy, ClientPageFilter<SnmpDeviceModel> pageFilter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение коллекции объектов «Правило распознавания»
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="filterBy"></param>
        /// <param name="pageFilter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SnmpDeviceModelDetails[]> GetDetailsPageAsync<TProperty>(SnmpDeviceModelFilter filterBy, BackendPageFilter<SnmpDeviceModel, TProperty> pageFilter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение коллекции объектов «Правило распознавания»
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SnmpDeviceModelDetails[]> GetDetailsArrayAsync(SnmpDeviceModelFilter filterBy, CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение коллекции объектов «Правило распознавания»
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SnmpDeviceModelDetails[]> GetAllDetailsArrayAsync(CancellationToken cancellationToken = default);
    }
}
