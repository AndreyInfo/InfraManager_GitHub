using System;

namespace InfraManager.BLL.ProductCatalogue.PortTemplates;

public class PortTemplatesData
{
    public Guid ObjectID { get; init; }
    
    public int ClassID { get; init; }

    public int JackTypeID { get; init; }
    
    public int PortNumber { get; init; }

    public int TechnologyID { get; init; }
}