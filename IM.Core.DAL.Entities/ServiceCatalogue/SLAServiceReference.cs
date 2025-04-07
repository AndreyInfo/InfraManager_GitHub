using System;

namespace InfraManager.DAL.ServiceCatalogue;

public class SLAServiceReference
{
    protected SLAServiceReference()
    {

    }

    public SLAServiceReference(Guid slaID, Guid serviceReferenceID)
    {
        SLAID = slaID;
        ServiceReferenceID = serviceReferenceID;
    }

    public Guid SLAID { get; set; }
    public Guid ServiceReferenceID { get; init; }
}