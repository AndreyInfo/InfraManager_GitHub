using InfraManager.BLL.ProductCatalogue.Slots;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.SlotTemplates;

/// <summary>
/// Бизнес-логика для работы с шаблонами слотов.
/// </summary>
public interface ISlotTemplateBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Фильтр типа <see cref="SlotTemplateFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="SlotTemplateDetails"/>.</returns>
    Task<SlotTemplateDetails[]> GetListAsync(SlotTemplateFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Получение шаблона слота.
    /// </summary>
    /// <param name="key">Идентификатор шаблона слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные шаблона слота типа <see cref="SlotTemplateDetails"/>.</returns>
    Task<SlotTemplateDetails> DetailsAsync(SlotBaseKey key, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового шаблона слота.
    /// </summary>
    /// <param name="data">Данные шаблона слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового шаблона слота типа <see cref="SlotTemplateDetails"/>.</returns>
    Task<SlotTemplateDetails> AddAsync(SlotTemplateData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление шаблона слота.
    /// </summary>
    /// <param name="key">Идентификатор шаблона слота.</param>
    /// <param name="data">Данные шаблона слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного шаблона слота типа <see cref="SlotTemplateDetails"/>.</returns>
    Task<SlotTemplateDetails> UpdateAsync(SlotBaseKey key, SlotTemplateData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление шаблона слота.
    /// </summary>
    /// <param name="key">Идентификатор шаблона слота.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(SlotBaseKey key, CancellationToken cancellationToken);
}
