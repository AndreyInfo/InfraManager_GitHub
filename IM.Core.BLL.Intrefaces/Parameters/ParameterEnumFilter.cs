using InfraManager.CrossPlatform.WebApi.Contracts.Common;

namespace InfraManager.BLL.Parameters;

public class ParameterEnumFilter : BaseFilter
{
    public bool? IsTree { get; init; }
}