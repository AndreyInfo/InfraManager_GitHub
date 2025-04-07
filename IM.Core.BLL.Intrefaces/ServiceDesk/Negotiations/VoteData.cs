using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class VoteData
    {
        public VotingType? Vote { get; init; }
        public string Comment { get; init; }
        public Guid? UserID { get; init; }
    }
}
