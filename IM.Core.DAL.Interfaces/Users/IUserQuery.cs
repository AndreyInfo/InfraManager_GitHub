using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.Users;

public interface IUserQuery
{
    /// <summary>
    /// Возвращает список пользователей с определенной фильтрацией
    /// </summary>
    /// <param name="objectID">ID организации или подразделения</param>
    /// <param name="operationIDs">Получение пользователей с данными операциями</param>
    /// <param name="objectClass">Выбор, фильтрация по организации или по подразделению</param>
    /// <param name="searchString">Строка поиска</param>
    /// <param name="sortColumn">По какой колонке сортировать</param>
    /// <param name="take">Сколько записей получить</param>
    /// <param name="skip">Сколько записей пропустить</param>
    /// <param name="onlyWithEmails">Флаг для возврата тех пользоватеоей, у которы есть Email</param>
    /// <param name="kbExpert">Только пользователи с правом быть экспертом базы знаний</param>
    /// <param name="cancellationToken">токен отмены</param>
    /// <returns>Список пользователей</returns>
    Task<UserModelItem[]> ExecuteAsync(Guid objectID
        , OperationID[] operationIDs
        , ObjectClass objectClass
        , string searchString
        , Sort sortColumn
        , int take
        , int skip
        , bool? onlyWithEmails
        , bool kbExpert
        , CancellationToken cancellationToken);
}
