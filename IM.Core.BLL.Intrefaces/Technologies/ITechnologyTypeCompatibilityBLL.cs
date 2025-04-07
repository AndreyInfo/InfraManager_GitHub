using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Technologies;
public interface ITechnologyTypeCompatibilityBLL
{
    /// <summary>
    /// Возвращает совместимые типы технологий
    /// С возможностью поиска и пагинации
    /// </summary>
    /// <param name="filter">фильтр получения совместимых типов технологий</param>
    /// <param name="cancellationToken"></param>
    /// <returns>совместимые типы технологий</returns>
    Task<TechnologyTypeDetails[]> GetListCompatibilityTechTypeByIDAsync(TechnologyTypesFilter filter, CancellationToken cancellationToken = default);
    /// <summary>
    /// Удаляет совместимые типы технологий
    /// </summary>
    /// <param name="fromId">идентификатор</param>
    /// <param name="ids">список идентификаторов</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task RemoveAsync(int fromID, IEnumerable<int> ids, CancellationToken cancellationToken = default);
    /// <summary>
    /// Добавляет совместимые типы технологий
    /// </summary>
    /// <param name="fromTechTypeID">идентификатор</param>
    /// <param name="toTechTypeID">список идентификаторов</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAsync(int fromTechTypeID, IEnumerable<int> toTechTypeIDd, CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает несовместимые типы технологий
    /// </summary>
    /// <param name="filter">Фильтр для выброки типов технологий</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TechnologyTypeDetails[]> GetListNotCompatibilityTechTypeByIDAsync(TechnologyTypesFilter filter, CancellationToken cancellationToken = default);
}
