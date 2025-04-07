using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Events;
using InfraManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Parameters
{
    public interface IParameterEnumBLL
    {
        /// <summary>
        /// Возвращает параметры
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ParameterEnumDetails[]> GetParameterEnumsAsync(ParameterEnumFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает параметр
        /// </summary>
        /// <param name="id">идентификатор парамета</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ParameterEnumDetails> GetParameterEnumAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет параметр
        /// </summary>
        /// <param name="parameterData">параметр</param>
        /// <param name="cancellationToken"></param>
        /// <returns>добавленная модель</returns>
        Task<ParameterEnumDetails> AddParameterEnumAsync(ParameterEnumDetails parameterData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет параметр
        /// </summary>
        /// <param name="parameterData">параметр</param>
        /// <param name="cancellationToken"></param>
        /// <returns>обновленная модель</returns>
        Task<ParameterEnumDetails> UpdateParameterEnumAsync(ParameterEnumDetails parameterData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет параметр
        /// </summary>
        /// <param name="id">идентификатор параметра</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteParameterEnumAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает значения параметра
        /// </summary>
        /// <param name="parentId">идентификатор родительсткого элемента</param>
        /// <param name="parameterEnumId">идентификатор параметра</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ParameterEnumValuesData[]> GetParameterEnumValuesAsync(Guid parameterEnumID, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает значение параметра
        /// </summary>
        /// <param name="id">идентификатор значения параметра</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ParameterEnumValueData> GetParameterEnumValueAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Сохраняет значение параметра
        /// </summary>
        /// <param name="parameterData">значения параметра</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateParameterEnumValuesAsync(List<ParameterEnumValuesData> parameterValuesData, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет значение параметра
        /// </summary>
        /// <param name="id">идентификатор значения параметра</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteParameterEnumValueAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает историю
        /// </summary>
        /// <param name="id">Идентификатор оповещения</param>
        /// <param name="dateFrom">Начальная дата</param>
        /// <param name="dateTill">Конечноая дата</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Event[]> GetHistoryAsync(Guid id, DateTime? dateFrom, DateTime? dateTill, CancellationToken cancellationToken = default);

    }
}
