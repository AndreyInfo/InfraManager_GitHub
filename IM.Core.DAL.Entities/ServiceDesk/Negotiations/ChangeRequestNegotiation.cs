using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class ChangeRequestNegotiation : Negotiation
    {
        protected ChangeRequestNegotiation()
        {
        }

        public ChangeRequestNegotiation(Guid changeRequestId)
            : base(changeRequestId, ObjectClass.ChangeRequest)
        {
        }
    }
}
