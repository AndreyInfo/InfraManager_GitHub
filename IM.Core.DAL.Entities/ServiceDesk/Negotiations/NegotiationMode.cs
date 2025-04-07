namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public enum NegotiationMode : byte
    {
        /// <summary>
        /// Голосование идет пока все не проголосуют
        /// </summary>
        VoteAll = 0,

        /// <summary>
        /// Голосование идет до первого против
        /// </summary>
        FirstVoteAgainst = 1,

        /// <summary>
        /// Голосование идет до первого за
        /// </summary>
        FirstVoteFor = 2,

        /// <summary>
        /// Голосование идет до любого первого голоса
        /// </summary>
        FirstVoteAny = 3
    }
}
