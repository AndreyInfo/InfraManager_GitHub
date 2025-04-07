using System;

namespace InfraManager.BLL.ProductCatalogue.PortTemplates;

public class PortTemplatesDetails
{
    public Guid ObjectID { get; init; }
    public int ClassID { get; init; }
    public int PortNumber { get; set; }
    public int JackTypeID { get; init; }
    public int TechnologyID { get; init; }
    public string JackTypeName { get; init; }
    public string TechnologyTypeName { get; init; }
}