using System.Data.Common;
using System.Threading.Tasks;

namespace InfraManager.DAL.Import;

/// <summary>
/// Получает объекты для работы с базой данных 
/// </summary>
public interface IDBRequestService
{
    /// <summary>
    /// Возвращает объект соединения с базой данных
    /// </summary>
    /// <param name="connectionString">Строка подключения</param>
    /// <returns>Соединение с базой данных</returns>
    DbConnection GetDbConnection(string connectionString);

    /// <summary>
    /// Возвращает объект запроса к базе данных 
    /// </summary>
    /// <param name="connection">Объект соединения с базой данных</param>
    /// <param name="command">запрос</param>
    /// <returns>Объект запроса к базе данных</returns>
    DbCommand GetDbCommand(DbConnection connection, string command);
}