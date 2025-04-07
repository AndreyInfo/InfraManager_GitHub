using InfraManager.DAL.Settings;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.DAL.Software.Installation
{
    public interface ICustomFilterSoftwareInstallationListQuery
    {
        IQueryable<ViewSoftwareInstallation> Query(IEnumerable<FilterElementData> filter);
    }
}
