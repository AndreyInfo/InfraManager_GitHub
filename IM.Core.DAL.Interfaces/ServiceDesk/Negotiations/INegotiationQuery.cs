using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public interface INegotiationQuery
    {
        bool NegotiationExistsByObjectQuery(Guid objectID, Guid userID);
    }
}
