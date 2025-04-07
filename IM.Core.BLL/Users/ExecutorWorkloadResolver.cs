using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.DAL.Users;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Users;

public class ExecutorWorkloadResolver : IValueResolver<EmployeeWorkloadQueryResultItem, EmployeeWorkloadListItem, string>
{
    private const string ResourceName = nameof(Resources.ExecutorWorkloadTemplate);

    private readonly ILocalizeText _localizer;

    public ExecutorWorkloadResolver(ILocalizeText localizer)
    {
        _localizer = localizer;
    }

    public string Resolve(EmployeeWorkloadQueryResultItem source, EmployeeWorkloadListItem destination, string destMember, ResolutionContext context)
    {
        return string.Format(_localizer.Localize(ResourceName), source.CallCount, source.WorkOrderCount);
    }
}