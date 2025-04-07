using Inframanager.BLL;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallTypeListFilter : ClientPageFilter<CallType>
    {
        public bool? VisibleInWeb { get; init; }
    }
}
