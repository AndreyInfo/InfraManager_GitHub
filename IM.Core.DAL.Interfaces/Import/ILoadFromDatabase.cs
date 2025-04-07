using System.Collections.Generic;
using System.Threading;

namespace InfraManager.DAL.Import;

/// <summary>
/// Работа с подключаемой базой данных
/// </summary>
public interface ILoadFromDatabase
{
    /// <summary>
    /// Получает содержимое таблицы из указанной в параметрах базы данных
    /// </summary>
    /// <param name="fieldNames">Список полей</param>
    /// <param name="tableName">Название таблицы</param>
    /// <param name="databaseName">Название базы данных</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Содержимое таблицы для выбранных полей</returns>
    IAsyncEnumerable<DBRowData> ImportModelsAsync(IEnumerable<string> fieldNames,
        string tableName,
        string databaseName,
        CancellationToken cancellationToken);
}