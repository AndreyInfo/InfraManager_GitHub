using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareManufacturers;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareManufacturer
/// </summary>
public interface ISoftwareManufacturerBLL
{
    /// <summary>
    /// Получение списка производителей ПО
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SoftwareManufacturerDetails[]> GetListAsync(CancellationToken cancellationToken = default);
}
