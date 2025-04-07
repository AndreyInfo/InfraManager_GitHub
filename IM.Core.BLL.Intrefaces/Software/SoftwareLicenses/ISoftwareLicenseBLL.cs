using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareLicenses;

/// <summary>
/// Бизнес логика для работы с сущностью SoftwareLicence
/// </summary>
public interface ISoftwareLicenseBLL
{
    /// <summary>
    /// Получение списка лицензий
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SoftwareLicenseDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default);
}
