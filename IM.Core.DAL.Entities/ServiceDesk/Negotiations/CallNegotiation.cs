using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class CallNegotiation : Negotiation
    {
        protected CallNegotiation()
        {
        }

        internal CallNegotiation(Guid callId) : base(callId, ObjectClass.Call)
        {
        }
    }
}
