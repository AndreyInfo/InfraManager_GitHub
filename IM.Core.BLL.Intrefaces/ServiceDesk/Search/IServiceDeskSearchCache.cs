using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Search;
/// <summary>
/// Кэш результатов поиска объектов Service Desk
/// </summary>
public interface IServiceDeskSearchCache
{
    /// <summary>
    /// Записывает результаты поиска с указанным ключом
    /// </summary>
    /// <param name="key">Ключ поиска</param>
    /// <param name="searchResult">Результаты поиска</param>
    void Cache(string key, IReadOnlyList<FoundObject> searchResult);

    /// <summary>
    /// Получает указанное кол-во результатов поиска по указанному ключу, если они есть в кэше, и сдвигает стартовый индекс
    /// </summary>
    /// <param name="key">Ключ поиска</param>
    /// <param name="amount">Кол-во результатов</param>
    /// <param name="result">Результаты поиска</param>
    /// <returns>Есть ли результаты в кэше</returns>
    bool TryTakeNext(string key, int amount, out IEnumerable<FoundObject> result);

    /// <summary>
    /// Получает результаты поиска по указанному ключу, если они есть в кэше
    /// </summary>
    /// <param name="key">Ключ поиска</param>
    /// <param name="result">Результаты поиска</param>
    /// <returns>Есть ли результаты в кэше</returns>
    bool TryGet(string key, out IEnumerable<FoundObject> result);
}