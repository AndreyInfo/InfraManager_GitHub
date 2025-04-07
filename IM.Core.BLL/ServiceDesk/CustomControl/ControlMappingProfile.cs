using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.CustomControl
{
    public class ControlMappingProfile : Profile
    {
        public ControlMappingProfile()
        {
            CreateMap<ObjectUnderControlQueryResultItem, ObjectUnderControl>()
                .ForMember(
                    reportItem => reportItem.CategoryName,
                    mapper => mapper.MapFrom<LocalizedEnumResolver<ObjectUnderControlQueryResultItem, ObjectUnderControl, Issues>, Issues>(
                        queryItem => queryItem.CategorySortColumn));
        }
    }
}
