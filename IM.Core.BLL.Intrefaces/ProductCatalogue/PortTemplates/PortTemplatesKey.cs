using System;

namespace InfraManager.BLL.ProductCatalogue.PortTemplates;

public class PortTemplatesKey
{
    public PortTemplatesKey(Guid id, int portNumber)
    {
        ObjectID = id;
        PortNumber = portNumber;
    }
    
    public Guid ObjectID { get; }

    public int PortNumber { get; }
}