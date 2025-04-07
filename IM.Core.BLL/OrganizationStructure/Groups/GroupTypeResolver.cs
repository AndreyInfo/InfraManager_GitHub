using AutoMapper;
using InfraManager.BLL.Asset;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.OrganizationStructure.Groups;

public class GroupTypeResolver :
    IMemberValueResolver<Group, GroupDetails, GroupType, GroupTypeData>
{
    public GroupTypeData Resolve(Group source, GroupDetails destination, GroupType sourceMember, GroupTypeData destMember,
        ResolutionContext context)
    {
        var groupType = source.Type & GroupType.All;                                                                   
        var isCall = (groupType & GroupType.Call) == GroupType.Call;                                  
        var isProblem = (groupType & GroupType.Problem) == GroupType.Problem;                         
        var isWorkOrder = (groupType & GroupType.WorkOrder) == GroupType.WorkOrder;                   
        var isChangeRequest = (groupType & GroupType.ChangeRequest) == GroupType.ChangeRequest;       
        var isMassiveIncident = (groupType & GroupType.MassiveIncident) == GroupType.MassiveIncident; 
        
        return new GroupTypeData(isCall, isWorkOrder, isProblem, isChangeRequest, isMassiveIncident);
    }
}