using Inframanager.BLL;
using InfraManager.BLL.Software.SoftwareModels.CommonDetails;
using InfraManager.DAL;
using InfraManager.DAL.Software;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.Software.SoftwareLicenseSchemes;

public class SoftwareLicenseSchemeBLL : ISoftwareLicenseSchemeBLL, ISelfRegisteredService<ISoftwareLicenseSchemeBLL>
{
    private readonly IGetEntityArrayBLL<Guid, SoftwareLicenceScheme, SoftwareLicenseSchemeDetails, LookupListFilter> _detailsArrayBLL;

    public SoftwareLicenseSchemeBLL(IGetEntityArrayBLL<Guid, SoftwareLicenceScheme, SoftwareLicenseSchemeDetails, LookupListFilter> detailsArrayBLL)
    {
        _detailsArrayBLL = detailsArrayBLL;
    }

    public async Task<SoftwareLicenseSchemeDetails[]> GetListAsync(LookupListFilter filter, CancellationToken cancellationToken = default)
    {
        return await _detailsArrayBLL.ArrayAsync(filter, cancellationToken);
    }
}
