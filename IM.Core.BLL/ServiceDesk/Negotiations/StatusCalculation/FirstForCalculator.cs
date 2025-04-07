using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations.StatusCalculation
{
    public class FirstForCalculator : ICalculateNegotiationStatus
    {
        public NegotiationStatus Calculate(IEnumerable<NegotiationUser> votes)
        {
            return votes.Any(vote => vote.VotingType == VotingType.VoteFor)
                ? NegotiationStatus.For
                : votes.Any(vote => vote.VotingType == VotingType.None)
                    ? NegotiationStatus.Voting
                    : NegotiationStatus.Against;
        }
    }
}
