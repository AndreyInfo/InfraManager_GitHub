using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Asset;

/// <summary>
/// Бизнес-логика для работы с имуществом (Asset).
/// </summary>
public interface IAssetBLL
{
    /// <summary>
    /// Добавление имущества.
    /// </summary>
    /// <param name="id">Идентификатор оборудования.</param>
    /// <param name="data">Данные имущества.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные нового имущества типа <see cref="AssetDetails"/>.</returns>
    Task<AssetDetails> AddAsync(AssetData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление имущества.
    /// </summary>
    /// <param name="id">Идентификатор имущества.</param>
    /// <param name="data">Данные имущества.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    /// <returns>Данные обновленного имущества типа <see cref="AssetDetails"/>.</returns>
    Task<AssetDetails> UpdateAsync(Guid id, AssetData data, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление имущества.
    /// </summary>
    /// <param name="id">Идентификатор имущества.</param>
    /// <param name="cancellationToken">Токен отмены.</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
