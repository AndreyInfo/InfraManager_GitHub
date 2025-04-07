using Inframanager.BLL.Events;
using InfraManager.DAL.ServiceDesk.Negotiations;
using System;

namespace InfraManager.BLL.ServiceDesk.Negotiations.Events
{
    internal class NegotiationSubjectBuilder<TNegotiation> : EventSubjectBuilderBase<TNegotiation, TNegotiation>
        where TNegotiation : Negotiation
    {
        public NegotiationSubjectBuilder() : base("Согласование")
        {
        }

        protected override Guid GetID(TNegotiation subject)
        {
            return subject.IMObjID;
        }

        protected override string GetSubjectValue(TNegotiation subject)
        {
            return subject.Name;
        }
    }
}
