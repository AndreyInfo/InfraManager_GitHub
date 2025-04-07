using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationUserSubjectBuilder : EventSubjectBuilderBase<NegotiationUser, Negotiation>
    {
        public NegotiationUserSubjectBuilder() : base("Согласование")
        {
        }

        protected override Guid GetID(Negotiation subject)
        {
            return subject.IMObjID;
        }

        protected override string GetSubjectValue(Negotiation subject)
        {
            return subject.Name;
        }
    }
}
