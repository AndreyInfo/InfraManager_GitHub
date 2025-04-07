using Inframanager.BLL;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareModelUsingTypes;

public class SoftwareModelUsingTypeBLL : ISoftwareModelUsingTypeBLL, ISelfRegisteredService<ISoftwareModelUsingTypeBLL>
{
    private readonly IGetEntityArrayBLL<Guid, SoftwareModelUsingType, SoftwareModelUsingTypeDetails, LookupListFilter> _detailsArray;

    public SoftwareModelUsingTypeBLL(IGetEntityArrayBLL<Guid, SoftwareModelUsingType, SoftwareModelUsingTypeDetails, LookupListFilter> detailsArray)
    {
        _detailsArray = detailsArray;
    }

    public async Task<SoftwareModelUsingTypeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default)
    {
        return await _detailsArray.ArrayAsync(filter, cancellationToken);
    }
}
