using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk
{
    public class MyTasksReportItemProfile : Profile
    {
        public MyTasksReportItemProfile()
        {
            CreateMap<MyTasksListQueryResultItem, MyTasksReportItem>()
                .ForMember(
                    reportItem => reportItem.CategoryName,
                    mapper => mapper.MapFrom<LocalizedEnumResolver<MyTasksListQueryResultItem, MyTasksReportItem, Issues>, Issues>(
                        queryItem => queryItem.CategorySortColumn));
        }
    }
}
