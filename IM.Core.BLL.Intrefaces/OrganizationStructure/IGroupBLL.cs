using InfraManager.BLL.Asset;
using InfraManager.BLL.OrganizationStructure.Groups;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.OrganizationStructure;

public interface IGroupBLL
{
    /// <summary>
    /// Получение группы по идентификатору
    /// </summary>
    /// <param name="queueID">идентификатор групп</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель группы</returns>
    Task<GroupDetails> DetailsAsync(Guid queueID, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение списка групп, с возможностью поиска
    /// </summary>
    /// <param name="searchName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>список моделей групп</returns>
    Task<GroupDetails[]> GetListAsync(string searchName, CancellationToken cancellationToken);

    /// <summary>
    /// Добавление группы с участниками и элементами доступа
    /// </summary>
    /// <param name="data">добавляемая модель</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор добавленной сущности</returns>
    Task<Guid> AddAsync(GroupData data, CancellationToken cancellationToken);

    /// <summary>
    /// Обновление группы с участниками и элементами доступа
    /// </summary>
    /// <param name="details">обновляемая сущность</param>
    /// <param name="id">идентификатор обновляемой сущности</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Идентификатор обновленной сущности</returns>
    Task<Guid> UpdateAsync(GroupDetails details, Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Удаление со всеми связанными сущностями
    /// </summary>
    /// <param name="id">идентификатор удаляемой сущности</param>
    /// <param name="cancellationToken"></param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

}