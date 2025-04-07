using System.Threading.Tasks;
using System.Threading;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;

namespace InfraManager.BLL.Software.SoftwareLicenseSchemes;

public interface ISoftwareLicenseSchemeBLL
{
    /// <summary>
    /// Получение списка схем лицензирования
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>Массив данных схем лицензирования типа <see cref="SoftwareLicenseSchemeDetails"/>.</returns>
    Task<SoftwareLicenseSchemeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default);
}
