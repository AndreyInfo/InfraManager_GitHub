using AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using System;

namespace InfraManager.BLL.ServiceDesk.ChangeRequests
{
    public class ChangeRequestTypeProfile : Profile
    {
        public ChangeRequestTypeProfile()
        {
            CreateMap<ChangeRequestType, RfcTypeDetailsModel>();

            CreateMap<ChangeRequestType, RfcTypeModel>().ReverseMap();

            CreateMap<ChangeRequestType, RfcTypeListItemModel>();
        }
    }
}
