using Inframanager.BLL;
using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareLicenses;

public class SoftwareLicenseBLL : ISoftwareLicenseBLL, ISelfRegisteredService<ISoftwareLicenseBLL>
{
    private readonly IGetEntityArrayBLL<Guid, SoftwareLicence, SoftwareLicenseDetails, LookupListFilter> _detailsArrayBLL;

    public SoftwareLicenseBLL(
        IGetEntityArrayBLL<Guid, SoftwareLicence, SoftwareLicenseDetails, LookupListFilter> detailsArrayBLL
        )
    {
        _detailsArrayBLL = detailsArrayBLL;
    }

    public async Task<SoftwareLicenseDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default)
    {
        return await _detailsArrayBLL.ArrayAsync(filter, cancellationToken);
    }
}
