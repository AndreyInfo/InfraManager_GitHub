using AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestCategoryProfile : Profile
    {
        public ChangeRequestCategoryProfile()
        {
            CreateMap<ChangeRequestCategory, ChangeRequestCategoryDetails>();
        }
    }
}
