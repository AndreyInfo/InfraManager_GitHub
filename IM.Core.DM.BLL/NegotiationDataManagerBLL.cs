using IM.Core.DM.BLL.Interfaces;
using InfraManager;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace IM.Core.DM.BLL
{
    internal class NegotiationDataManagerBLL : INegotiationDataManagerBLL, ISelfRegisteredService<INegotiationDataManagerBLL>
    {
        private readonly INegotiationQuery _negotiationQuery;

        public NegotiationDataManagerBLL(
                    INegotiationQuery negotiationQuery)
        {
            _negotiationQuery = negotiationQuery;
        }

        public bool NegotiationExistsByObject(Guid objectID, Guid userID)
        {
            return _negotiationQuery.NegotiationExistsByObjectQuery(objectID, userID);
        }
    }
}
