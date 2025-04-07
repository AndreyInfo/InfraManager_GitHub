using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using System.Threading.Tasks;
using System.Threading;

namespace InfraManager.BLL.Software.SoftwareTypes;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareType
/// </summary>
public interface ISoftwareTypeBLL
{
    /// <summary>
    /// Получение списка типов ПО.
    /// </summary>
    /// <param name="filter">Фильтр по имени.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных типов ПО типа <see cref="SoftwareTypeDetails"/>.</returns>
    Task<SoftwareTypeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default);
}
