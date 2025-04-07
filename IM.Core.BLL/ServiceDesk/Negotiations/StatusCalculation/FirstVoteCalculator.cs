using InfraManager.DAL.ServiceDesk.Negotiations;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk.Negotiations.StatusCalculation
{
    public class FirstVoteCalculator : ICalculateNegotiationStatus
    {
        public NegotiationStatus Calculate(IEnumerable<NegotiationUser> votes)
        {
            var firstVote = votes
                .OrderBy(vote => vote.UtcVoteDate)
                .FirstOrDefault(vote => vote.VotingType != VotingType.None)
                ?.VotingType;

            return firstVote.HasValue
                ? (firstVote.Value == VotingType.VoteAgainst ? NegotiationStatus.Against : NegotiationStatus.For)
                : NegotiationStatus.Voting;
        }
    }
}
