using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;

namespace InfraManager.BLL.ProductCatalogue.MaterialConsumptionRates;

public interface IMaterialConsumptionRateBLL
{
    /// <summary>
    /// Возвращает отфильтрованный массив данных расходных материалов
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные расходных материалов</returns>
    Task<MaterialConsumptionRateOutputDetails[]> GetDetailsArrayAsync(MaterialConsumptionRateFilter filter, CancellationToken token);

    /// <summary>
    /// Возвращает отфильтрованный массив данных расходных материалов
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные расходных материалов</returns>
    Task<MaterialConsumptionRateOutputDetails[]> GetDetailsPageAsync(MaterialConsumptionRateFilter filter, ClientPageFilter pageFilter,
        CancellationToken token);

    /// <summary>
    /// Получает данные о расходном материале
    /// </summary>
    /// <param name="id">Идентификатор расходного материала</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные о расходном материале</returns>
    Task<MaterialConsumptionRateOutputDetails> DetailsAsync(Guid id, CancellationToken token);

    /// <summary>
    /// Добавляет данные о расходном материале в базу
    /// </summary>
    /// <param name="data">Данные о расходном материале</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Записанные данные</returns>
    Task<MaterialConsumptionRateOutputDetails> AddAsync(MaterialConsumptionRateInputDetails data, CancellationToken token);

    /// <summary>
    /// Обновляет данные о расходном материале
    /// </summary>
    /// <param name="id">Идентификатор расходного материала</param>
    /// <param name="data">Новые данные расходного материала</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Записанае данные расходного материала</returns>
    Task<MaterialConsumptionRateOutputDetails> UpdateAsync(Guid id, MaterialConsumptionRateInputDetails data, CancellationToken token);

    /// <summary>
    /// Удаляет данные о расходном материале из базы данных
    /// </summary>
    /// <param name="id">Идентификатор расходного материала</param>
    /// <param name="token">Токен отмены</param>
    Task DeleteAsync(Guid id, CancellationToken token);
}