using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Collections.Generic;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public interface ICalculateNegotiationStatus
    {
        NegotiationStatus Calculate(IEnumerable<NegotiationUser> votes);
    }
}
