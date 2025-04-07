using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;

namespace InfraManager.BLL.Software.SoftwareModelUsingTypes;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareModelUsingType
/// </summary>
public interface ISoftwareModelUsingTypeBLL
{
    /// <summary>
    /// Получение списка типов использования ПО.
    /// </summary>
    /// <param name="filter">Фильтр по имени.</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных типов использования ПО типа <see cref="SoftwareModelUsingTypeDetails"/>.</returns>
    Task<SoftwareModelUsingTypeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default);
}
