using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.Synonyms;

public interface ISynonymBLL
{
    /// <summary>
    /// Получение nаблицы сининимов.
    /// </summary>
    /// <param name="filter">Фильтр таблицы</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Таблица сининимов</returns>
    Task<SynonymOutputDetails[]> GetDetailsArrayAsync(SynonymFilter filter,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение данных для синонима каталога продуктов 
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Детализация синонима</returns>
    Task<SynonymOutputDetails> DetailsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Вставляет синоним в указанную категорию 
    /// </summary>
    /// <param name="data">Данные для встаки</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<SynonymOutputDetails> AddAsync(SynonymDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление синонима ссответствующего идентификатору
    /// </summary>
    /// <param name="id">Идентификатор синонима</param>
    /// <param name="data">Новые данные для синонима с тем же идентификатором</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task<SynonymOutputDetails> UpdateAsync(Guid id,
        SynonymDetails data,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление синонима
    /// </summary>
    /// <param name="id">Идентификатор синонима</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns></returns>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}