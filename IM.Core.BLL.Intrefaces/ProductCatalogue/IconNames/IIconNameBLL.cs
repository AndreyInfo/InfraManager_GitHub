using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ProductCatalogue.IconNames;

/// <summary>
/// Бизнес логика иконок
/// </summary>
public interface IIconNameBLL
{
    /// <summary>
    /// Получение двнных иконки по идентификатору
    /// </summary>
    /// <param name="iconID">Идентификатор иконки</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные иконки</returns>
    Task<IconNameDetails> GetIconAsync(Guid iconID, CancellationToken token = default);

    /// <summary>
    /// Получение списка свойств иконок с идентификаторами
    /// </summary>
    /// <param name="token">Токен отмены</param>
    /// <returns>Данные иконки</returns>
    Task<IconNameDetails[]> GetIconsAsync(CancellationToken token);
}