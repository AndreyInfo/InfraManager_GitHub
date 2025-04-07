using InfraManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    internal class NegotiationQuery : INegotiationQuery, ISelfRegisteredService<INegotiationQuery>
    {
        private readonly CrossPlatformDbContext _dbContext;

        public NegotiationQuery(CrossPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool NegotiationExistsByObjectQuery(Guid objectID, Guid userID)
        {
            var query = from nu in _dbContext.Set<NegotiationUser>().AsNoTracking()
                        join ng in _dbContext.Set<Negotiation>().AsNoTracking()
                        on nu.NegotiationID equals ng.IMObjID
                        where nu.UserID == userID && ng.ObjectID == objectID
                        select nu.UserID;
            return query.Any();
        }
    }
}
