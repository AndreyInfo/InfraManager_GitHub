using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.ProductCatalogue.PortTemplates;


public class PortTemplatesFilter : BaseFilter
{
    public int? JackTypeID { get; init; }

    public int? TechnologyID { get; init; }

    public string TechnologyName { get; init; }

    public string JackTypeName { get; init; }

    public int? ClassID { get; init; }

    public int? PortNumber { get; init; }

    public Guid? ObjectID { get; init; }

}