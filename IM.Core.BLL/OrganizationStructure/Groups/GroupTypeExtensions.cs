using System.Collections.Generic;
using System.Linq;
using InfraManager.BLL.Asset;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure.Groups;

public static class GroupTypeExtensions
{
    public static GroupType ConvertToGroupType(GroupTypeData data)
    {
        GroupType groupType = 0;

        var groupTypeArray = ConvertFromData(data);
        foreach (var el in groupTypeArray)
        {
            groupType |= el;
        }
        
        return groupType;
    }

    public static byte[] GetPossibleValues(GroupTypeData groupTypeData)
    {
        var convertedType = ConvertFromData(groupTypeData);
        
        var storage = new Dictionary<byte, GroupType>();
        for (byte val = 0; val <= (byte)GroupType.All; val++)
        {
            storage.Add(val, (GroupType)val);
        }

        List<byte> result = new List<byte>();

        foreach (var el in convertedType)
        {
            result.AddRange(storage.Where(x => (x.Value & el) == el).Select(x => x.Key).ToList());
        }

        return result.Distinct().ToArray();
    }
    
    private static GroupType[] ConvertFromData(GroupTypeData groupTypeData)
    {
        List<GroupType> convertedType = new List<GroupType>();
        
        if (groupTypeData.IsCall)
        {
            convertedType.Add(GroupType.Call);
        }
        if (groupTypeData.IsProblem)
        {
            convertedType.Add(GroupType.Problem);
        }
        if (groupTypeData.IsChangeRequest)
        {
            convertedType.Add(GroupType.ChangeRequest);
        }
        if (groupTypeData.IsMassiveIncident)
        {
            convertedType.Add(GroupType.MassiveIncident);
        }
        if (groupTypeData.IsWorkOrder)
        {
            convertedType.Add(GroupType.WorkOrder);
        }

        return convertedType.ToArray();
    }
}