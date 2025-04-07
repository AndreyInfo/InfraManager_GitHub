using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Highlighting;

public interface IHighlightingBLL
{
    /// <summary>
    /// Возвращает список правил выделения строк в списке
    /// </summary>
    /// <param name="filter">Фильтр для получения данных</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Список правил выделения строк в списке</returns>
    Task<HighlightingDetails[]> GetAllDetailsArrayAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет Правило выделения строк в списке
    /// </summary>
    /// <param name="data">Данные правил выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Данные правил выделения строк в списке</returns>
    Task<HighlightingDetails> AddAsync(HighlightingData data, CancellationToken cancellationToken = default);


    /// <summary>
    /// Обновляет Правило выделения строк в списке
    /// </summary>
    /// <param name="highlightingID">Идентификатор правила выделения строк в списке</param>
    /// <param name="data">Данные правил выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task<HighlightingDetails> UpdateAsync(Guid highlightingID, HighlightingData data, CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет правило выделения строк в списке
    /// </summary>
    /// <param name="id">Идентификатор правила выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значения правила выделения строк в списке
    /// </summary>
    /// <param name="id">Идентификатор правила выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Значения правила выделения строк в списке</returns>
    Task<HighlightingConditionDetails[]> ListValueAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавляет значения правила выделения строк в списке
    /// </summary>
    /// <param name="value">Данные правил выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task AddValueAsync(HighlightingConditionData value,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Изменяет значения правила выделения строк в списке
    /// </summary>
    /// <param name="valueID">Идентификатор значения правила выделения строк в списке</param>
    /// <param name="value">Данные правил выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task UpdateValueAsync(Guid valueID, HighlightingConditionData value,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет значения правила выделения строк в списке
    /// </summary>
    /// <param name="valueID">Идентификатор правила выделения строк в списке</param>
    /// <param name="cancellationToken">токен отмены</param>
    Task DeleteValueAsync(Guid valueID, CancellationToken cancellationToken = default);
}
