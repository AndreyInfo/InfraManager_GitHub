using InfraManager.BLL.Settings.TableFilters;
using InfraManager.DAL;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    internal class StandardNegotiationFilterExpressions : StandardPredicatesDictionary<Negotiation>, ISelfRegisteredService<IStandardPredicatesProvider<Negotiation>>
    {
        public StandardNegotiationFilterExpressions(IReadonlyRepository<DeputyUser> deputyUsers)
        {            
            //!Начатые согласования, требующие моего участия
            Add(
                StandardFilters.NegotiationNegStartedNeeded,
                    userID =>
                    {
                        var replacedByUser = deputyUsers.Query(DeputyUser.UserIsDeputy(userID));
                        return n => n.Status == NegotiationStatus.Voting 
                            && n.NegotiationUsers.Any(
                                nu => nu.VotingType == VotingType.None 
                                    && (nu.UserID == userID || replacedByUser.Any(x => x.ParentUserId == nu.UserID)));
                    });
            //Незавершенные согласования, уже не требующие моего участия
            Add(
                StandardFilters.NegotiationNegStartedNotNeeded,
                    userID =>
                    {
                        var replacedByUser = deputyUsers.Query(DeputyUser.UserIsDeputy(userID));
                        return n => n.Status == NegotiationStatus.Voting 
                            && n.NegotiationUsers.Any(
                                nu => (nu.UserID == userID || replacedByUser.Any(x => x.ParentUserId == nu.UserID)) 
                                    && nu.VotingType != VotingType.None);
                    });
            //Завершенные согласования
            Add(
                StandardFilters.NegotiationNegFinished,
                userId =>
                    n => n.Status == NegotiationStatus.For || n.Status == NegotiationStatus.Against);
            //Неначатые согласования
            Add(
                StandardFilters.NegotiationNegNotStarted,
                userId =>
                    n => n.Status == NegotiationStatus.Created);
        }

    }
}
