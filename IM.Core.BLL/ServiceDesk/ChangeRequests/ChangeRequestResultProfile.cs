using AutoMapper;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestResultProfile : Profile
    {
        public ChangeRequestResultProfile()
        {
            CreateMap<RequestForServiceResult, ChangeRequestResultListItem>();
            CreateMap<ChangeRequestResultModel, RequestForServiceResult>()
                .ConstructUsing(x => new RequestForServiceResult(x.Name));
            CreateMap<RequestForServiceResult, ChangeRequestResultDetailsModel>();
        }
    }
}
