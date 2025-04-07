using AutoMapper;
using InfraManager.BLL.OrganizationStructure.Groups;
using InfraManager.DAL.OrganizationStructure;

namespace InfraManager.BLL.Users;

public class GroupWorkloadListItemMappingProfile : Profile
{
    public GroupWorkloadListItemMappingProfile()
    {
        CreateMap<GroupWorkloadListQueryResultItem, GroupWorkloadListItem>();
    }
}