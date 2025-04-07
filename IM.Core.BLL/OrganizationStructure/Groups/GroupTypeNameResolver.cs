using System.Collections.Generic;
using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.BLL.Localization;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.OrganizationStructure.Groups;

internal class GroupTypeNameResolver : IMemberValueResolver<Group, GroupDetails, GroupType, string>
{
    private readonly ILocalizeText _localizer;

    public GroupTypeNameResolver(ILocalizeText localizer)
    {
        _localizer = localizer;
    }

    public string Resolve(Group source, GroupDetails destination, GroupType sourceMember, string destMember, ResolutionContext context)
    {
        if (sourceMember == GroupType.None)
        {
            return "-";
        }
        if ((sourceMember & GroupType.All) == GroupType.All)
        {
            return _localizer.Localize(nameof(Resources.GroupType_All));
        }
        
        var typeNames = new List<string>();
        if ((sourceMember & GroupType.Call) == GroupType.Call)
        {
            typeNames.Add(_localizer.Localize(nameof(Resources.Calls)));
        }
        if ((sourceMember & GroupType.WorkOrder) == GroupType.WorkOrder)
        {
            typeNames.Add(_localizer.Localize(nameof(Resources.WorkOrders)));
        }
        if ((sourceMember & GroupType.MassiveIncident) == GroupType.MassiveIncident)
        {
            typeNames.Add(_localizer.Localize(nameof(Resources.MassIncident)));
        }
        if ((sourceMember & GroupType.Problem) == GroupType.Problem)
        {
            typeNames.Add(_localizer.Localize(nameof(Resources.Problems)));
        }
        if ((sourceMember & GroupType.ChangeRequest) == GroupType.ChangeRequest)
        {
            typeNames.Add(_localizer.Localize(nameof(Resources.ChangeRequests)));
        }

        return typeNames.Count > 0 ? string.Join(", ", typeNames) : string.Empty;
    }
}