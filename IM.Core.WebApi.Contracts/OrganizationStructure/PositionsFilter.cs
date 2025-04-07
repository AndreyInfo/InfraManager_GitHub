using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.WebApi.Contracts.OrganizationStructure;

public class PositionsFilter : BaseFilter
{
    public string OrderBy { get; set; }

}