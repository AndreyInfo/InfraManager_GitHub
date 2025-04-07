using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using InfraManager.DAL.Asset;

namespace InfraManager.BLL.ProductCatalogue.Manufactures;

public interface IManufacturersBLL
{
    /// <summary>
    /// Возвращает отфильтрованный список производителей
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Список производителей</returns>
    Task<ManufacturerDetails[]> GetDetailsArrayAsync(ManufacturersFilter filter, CancellationToken token);

    /// <summary>
    /// Возвращает отсортированный отфильтрованный список производителей
    /// с разбиением на страницы
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="pageFilter">Сортировка, начало страницы и ее размер</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Список производителей</returns>
    Task<ManufacturerDetails[]> GetDetailsPageAsync(ManufacturersFilter filter,
        ClientPageFilter<Manufacturer> pageFilter,
        CancellationToken token);

    /// <summary>
    /// Возвращает данные производителя
    /// </summary>
    /// <param name="id">Идентификатор производителя</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные производителя</returns>
    Task<ManufacturerDetails> DetailsAsync(int id, CancellationToken token);

    /// <summary>
    /// Добавляет данные производителя в базу
    /// </summary>
    /// <param name="data">Данные производителя</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные производителя, записанные в базу</returns>
    Task<ManufacturerDetails> AddAsync(ManufacturerData data, CancellationToken token);

    /// <summary>
    /// Изменение данных о производителе
    /// </summary>
    /// <param name="id">Идентификатор производителя</param>
    /// <param name="data">Новые данные производителя</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Новые данные производителя</returns>
    Task<ManufacturerDetails> UpdateAsync(int id, ManufacturerData data, CancellationToken token);

    /// <summary>
    /// Удаляет данные производителя из базы
    /// </summary>
    /// <param name="id">Идентификатор производителя</param>
    /// <param name="token">Токен отмены</param>
    Task DeleteAsync(int id, CancellationToken token);

    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой по viewName
    /// </summary>
    /// <param name="filter">фильтр</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Массив моделей производителей</returns>
    Task<ManufacturerDetails[]> GetPaggingAsync(ManufacturersFilter filter, CancellationToken cancellationToken);
}