using InfraManager.BLL.ProductCatalogue.Manufactures;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset.ConnectorTypes;

/// <summary>
/// Бизнес-логика для работы с типами разъемов.
/// </summary>
public interface IConnectorTypeBLL
{
    /// <summary>
    /// Получение таблицы с фильтрацией, пагинацией, поиском и сортировкой.
    /// </summary>
    /// <param name="filter">Базовый фильтр <see cref="BaseFilter"/>.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Массив данных типа <see cref="ConnectorTypeDetails"/>.</returns>
    Task<ConnectorTypeDetails[]> GetListAsync(BaseFilter filter, CancellationToken cancellationToken);

    /// <summary>
    /// Создание нового типа разъема порта
    /// </summary>
    /// <param name="data">Параметры типа разъема порта</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Записанные параметры типа разъема порта</returns>
    Task<ConnectorTypeDetails> AddAsync(ConnectorTypeData data,
        CancellationToken cancellationToken);

    /// <summary>
    /// Обновление типа разъема.
    /// </summary>
    /// <param name="id">Идентификатор типа разъема порта.</param>
    /// <param name="data">Параметры типа разъема порта.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Обновленные параметры типа разъема порта.</returns>
    Task<ConnectorTypeDetails> UpdateAsync(int id, ConnectorTypeData data, CancellationToken cancellationToken);

    /// <summary>
    /// Получение параметров типа разъема порта по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор типа разъема порта</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденные параметры типа разъема порта</returns>
    Task<ConnectorTypeDetails> DetailsAsync(int id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление типа разъемов портов по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого типа разъема</param>
    /// <param name="cancellationToken">Токен отмены</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
