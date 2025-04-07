using Inframanager.BLL;
using InfraManager.DAL;
using InfraManager.DAL.ServiceDesk;
using InfraManager.ResourcesArea;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    internal static class Extensions
    {
        public async static Task SetCallGroupIfNeededAsync(this Call call, NullablePropertyWrapper<Guid> groupID, IFinder<Group> groupFinder, CancellationToken cancellationToken = default)
        {
            if (groupID.Value.HasValue)
            {
                var initialGroup = await groupFinder.FindAsync(groupID.Value, cancellationToken)
                    ?? throw new InvalidObjectException(nameof(Resources.Call_GroupIsMissing)); 
                call.SetGroup(initialGroup);
            }
        }
    }
}
