using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Technologies;

public class TechnologyTypesFilter : BaseFilter
{
    public int? FromID { get; init; }
}