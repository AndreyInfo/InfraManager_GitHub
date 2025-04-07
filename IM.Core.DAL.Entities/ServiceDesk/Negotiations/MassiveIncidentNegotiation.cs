using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public class MassiveIncidentNegotiation : Negotiation
    {
        protected MassiveIncidentNegotiation() : base()
        {
        }

        public MassiveIncidentNegotiation(Guid massiveIncidentID) 
            : base(massiveIncidentID, ObjectClass.MassIncident)
        {
        }
    }
}
