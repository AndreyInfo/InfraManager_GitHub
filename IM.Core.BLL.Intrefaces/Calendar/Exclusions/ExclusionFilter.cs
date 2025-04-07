using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using InfraManager.DAL.Calendar;

namespace InfraManager.BLL.Calendar.Exclusions;

public class ExclusionFilter : BaseFilter
{
    public ExclusionType? ExclusionType { get; init; }
}
