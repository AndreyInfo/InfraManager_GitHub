using InfraManager.BLL.Asset;
using InfraManager.BLL.OrganizationStructure.Groups;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure;

/// <summary>
/// Бизнес логика с группами и их членами
/// TODO разделить на две отдельные BLL
/// </summary>
public interface IGroupQueueBLL
{
    /// <summary>
    /// Получение групп с поиском и скролингом
    /// </summary>
    /// <param name="filter">фильтр для таблицы(для скролинга и поиска)</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели для таблицы описывающие группу</returns>
    Task<GroupDetails[]> GetListAsync(GroupFilter filter,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение членов группы
    /// </summary>
    /// <param name="groupID">идентификатор группы</param>
    /// <param name="isPerfomers">true- исполнителей, false - всех пользователей, кроме исполнителей</param>
    /// <param name="searchName">строка поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns>исполнителей</returns>
    Task<GroupQueueUserDetails[]> GetPerformersAsync(Guid groupID, bool isPerfomers, string searchName, CancellationToken cancellationToken);

    /// <summary>
    /// Получение исполнителя по id в группе
    /// Идет проверка на существование группы и входит ли пользователь в группу
    /// </summary>
    /// <param name="userID">идентификатор пользователя</param>
    /// <param name="groupID">идентификатор группы</param>
    /// <param name="cancellationToken"></param>
    /// <returns>иполнителя</returns>
    Task<GroupQueueUserDetails> GetPerformerByIdAsync(Guid userID, Guid groupID, CancellationToken cancellationToken);
}