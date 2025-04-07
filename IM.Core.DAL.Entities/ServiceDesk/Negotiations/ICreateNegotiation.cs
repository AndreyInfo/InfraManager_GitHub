using System;

namespace InfraManager.DAL.ServiceDesk.Negotiations
{
    public interface ICreateNegotiation : IGloballyIdentifiedEntity
    {
        Negotiation CreateNegotiation();
        DateTime UtcDateModified { set; }
    }
}
