using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations.StatusCalculation
{
    public class VoteAllCalculator : ICalculateNegotiationStatus
    {
        public NegotiationStatus Calculate(IEnumerable<NegotiationUser> votes)
        {
            return votes.Any(vote => vote.VotingType == VotingType.None)
                ? NegotiationStatus.Voting
                : votes.Count(vote => vote.VotingType == VotingType.VoteFor)
                        > votes.Count(vote => vote.VotingType == VotingType.VoteAgainst)
                    ? NegotiationStatus.For
                    : NegotiationStatus.Against;
        }
    }
}
