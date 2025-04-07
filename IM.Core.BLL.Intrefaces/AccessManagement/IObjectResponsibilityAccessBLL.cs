using InfraManager.BLL.OrganizationStructure.Groups;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.AccessManagement;

public interface IObjectResponsibilityAccessBLL
{
    /// <summary>
    /// Удаляются все права доступа для владельца
    /// </summary>
    /// <param name="ownerID">владелец доступа к объектам</param>
    /// <param name="cancellationToken"></param>
    Task DeleteByOwnerAsync(Guid ownerID, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранение доступа ответственности
    /// Желательно не использовать, сделано из-за ограничений фронта
    /// TODO выпилить, как только на фронте появится возможность, т.к. нарушает ат
    /// </summary>
    /// <param name="models">модель для сохранения</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAccess(AccessElementsDetails[] models, CancellationToken cancellationToken);

    /// <summary>
    /// Получение всех объектов доступа для собственника(пользователь, группа и т.д.)
    /// </summary>
    /// <param name="ownerID">идентификатор собственника</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модель с доступами</returns>
    Task<ItemResponsibilityTrees> GetAccessAsync(Guid ownerID, CancellationToken cancellationToken);

}
