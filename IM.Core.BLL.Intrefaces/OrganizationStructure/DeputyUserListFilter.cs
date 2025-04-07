using Inframanager.BLL;
using InfraManager.DAL.OrganizationStructure;
using System;

namespace InfraManager.BLL.OrganizationStructure
{
    public class DeputyUserListFilter : ClientPageFilter<DeputyUser>
    {
        public bool ShowFinished { get; init; }
        public bool Active { get; init; }

        public Guid UserID { get; init; }

        public DeputyMode DeputyMode { get; init; }
    }
}