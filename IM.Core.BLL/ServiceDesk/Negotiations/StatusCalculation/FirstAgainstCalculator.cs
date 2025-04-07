using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations.StatusCalculation
{
    public class FirstAgainstCalculator : ICalculateNegotiationStatus
    {
        public NegotiationStatus Calculate(IEnumerable<NegotiationUser> votes)
        {
            return votes.Any(vote => vote.VotingType == VotingType.VoteAgainst)
                ? NegotiationStatus.Against
                : votes.Any(vote => vote.VotingType == VotingType.None)
                    ? NegotiationStatus.Voting
                    : NegotiationStatus.For;
        }
    }
}
