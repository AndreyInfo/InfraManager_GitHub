using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.Users;
using InfraManager.ResourcesArea;

namespace InfraManager.BLL.Users;

public class EmployeeWorkloadLIstItemMappingProfile : Profile
{
    public EmployeeWorkloadLIstItemMappingProfile()
    {
        CreateMap<EmployeeWorkloadQueryResultItem, EmployeeWorkloadListItem>()
            .ForMember(dst => dst.IsOnWorkplace,
                mapper => mapper.MapFrom<LocalizedTextResolver<EmployeeWorkloadQueryResultItem, EmployeeWorkloadListItem>, string>(
                    src => src.IsOnWorkplace ? nameof(Resources.True) : null))
            .ForMember(dst => dst.ExecutorWorkload,
                mapper => mapper.MapFrom<ExecutorWorkloadResolver>());
    }
}