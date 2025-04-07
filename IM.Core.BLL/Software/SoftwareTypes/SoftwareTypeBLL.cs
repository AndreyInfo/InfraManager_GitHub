using Inframanager.BLL;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using System;
using System.Threading;
using System.Threading.Tasks;
using SoftwareTypeEntity = InfraManager.DAL.Software.SoftwareType;

namespace InfraManager.BLL.Software.SoftwareTypes;

public class SoftwareTypeBLL : ISoftwareTypeBLL, ISelfRegisteredService<ISoftwareTypeBLL>
{
    private readonly IGetEntityArrayBLL<Guid, SoftwareTypeEntity, SoftwareTypeDetails, LookupListFilter> _detailsArrayBLL;

    public SoftwareTypeBLL(IGetEntityArrayBLL<Guid, SoftwareTypeEntity, SoftwareTypeDetails, LookupListFilter> detailsArrayBLL)
    {
        _detailsArrayBLL = detailsArrayBLL;
    }

    public async Task<SoftwareTypeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default)
    {
        return await _detailsArrayBLL.ArrayAsync(filter, cancellationToken);
    }
}
