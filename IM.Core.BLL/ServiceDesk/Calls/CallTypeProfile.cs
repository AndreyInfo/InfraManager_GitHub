using AutoMapper;
using InfraManager.DAL.ServiceDesk;
using System;

namespace InfraManager.BLL.ServiceDesk.Calls
{
    public class CallTypeProfile : Profile
    {
        public CallTypeProfile()
        {
            CreateMap<CallType, CallTypeDetails>()
                .ForMember(x => x.IsRFC, m => m.MapFrom(x => x.IsChangeRequest))
                .ForMember(x => x.Name, m => m.MapFrom(x => x.EventHandlerName))
                .ForMember(x => x.HasNoImage, m => m.MapFrom(x => x.Icon == null))
                .ForMember(x => x.ParentCallTypeID, m => m.MapFrom(x => x.Parent == null ? (Guid?)null : x.Parent.ID))
                .ForMember(x => x.WorkflowSchemeIdentifier, m => m.MapFrom(x => x.WorkflowSchemeIdentifier));

            CreateMap<CallType, BLL.Calls.DTO.CallTypeDetails>().ReverseMap(); //revers map for sla rules (TODO: убрать реверс мап, не использовать выходной контракт как входной)

            CreateMap<CallTypeData, CallType>().ConstructUsing(x => new(x.Name));
        }
    }
}
