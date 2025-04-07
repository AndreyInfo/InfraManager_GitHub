using InfraManager.BLL.Technologies;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Configuration.Technologies;

public interface ITechnologyTypeBLL
{
    /// <summary>
    /// Возвращает тип технологий
    /// </summary>
    /// <param name="id">идентификатор</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TechnologyTypeDetails> DetailsAsync(int id, CancellationToken cancellationToken = default);
    /// <summary>
    /// Возвращает типы технологий
    /// </summary>
    /// <param name="search">строка поиска</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TechnologyTypeDetails[]> GetDetailsArrayAsync(TechnologyTypeFilter filter, CancellationToken cancellationToken = default);
    /// <summary>
    /// Добавляет тип технологий
    /// </summary>
    /// <param name="technologyType">тип технологий</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TechnologyTypeDetails> AddAsync(TechnologyTypeData technologyType, CancellationToken cancellationToken = default);
    /// <summary>
    /// Обновляет тип технологий
    /// </summary>
    /// <param name="technologyType">тип технологий</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TechnologyTypeDetails> UpdateAsync(int id, TechnologyTypeData technologyType, CancellationToken cancellationToken = default);
}
