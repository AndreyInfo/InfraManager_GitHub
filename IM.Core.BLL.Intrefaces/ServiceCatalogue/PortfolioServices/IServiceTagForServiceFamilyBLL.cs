using InfraManager.DAL.ServiceCatalogue;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceCatalogue.PortfolioServices;
public interface IServiceTagForServiceFamilyBLL
{
    /// <summary>
    /// Получение тегов
    /// </summary>
    /// <param name="id">идентификатор обекта</param>
    /// <param name="classID">тип объекта</param>
    /// <param name="cancellationToken"></param>
    /// <returns>модели тегов</returns>
    Task<ServiceTag[]> GetByIDAndClassIdAsync(Guid id, ObjectClass classID, CancellationToken cancellationToken);

    /// <summary>
    /// Сохранение тегов
    /// </summary>
    /// <param name="saveModels">сохроняемые теги</param>
    /// <param name="id">идентификатор объекта</param>
    /// <param name="classID">тип объекта</param>
    /// <param name="cancellationToken"></param>
    Task SaveAsync(IEnumerable<ServiceTag> saveModels, Guid id, ObjectClass classID, CancellationToken cancellationToken = default);
}