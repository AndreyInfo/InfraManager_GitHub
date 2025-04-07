using InfraManager.DAL.ServiceDesk.Negotiations;
using System;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationData
    {
        public string Name { get; init; }
        public NegotiationMode? Mode { get; init; }
        public bool IsStarted { get; init; }
        public Guid[] UserIDs { get; init; }
        public bool ModifyNegotiation => (UserIDs != null && UserIDs.Any())
            || Mode.HasValue
            || !string.IsNullOrWhiteSpace(Name);
    }
}
