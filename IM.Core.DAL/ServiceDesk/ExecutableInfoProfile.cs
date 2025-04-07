using AutoMapper;
using InfraManager.DAL.ServiceDesk.ChangeRequests;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.DAL.ServiceDesk
{
    internal class ExecutableInfoProfile : Profile
    {
        public ExecutableInfoProfile()
        {
            CreateMap<Call, ExecutableInfo>()
                .ForMember(info => info.GroupID, mapper => mapper.MapFrom(call => call.QueueID));
            CreateMap<WorkOrder, ExecutableInfo>()
                .ForMember(info => info.GroupID, mapper => mapper.MapFrom(wo => wo.QueueID));
            CreateMap<Problem, ExecutableInfo>()
                .ForMember(info => info.GroupID, mapper => mapper.MapFrom(p => Group.NullGroupID))
                .ForMember(info => info.ExecutorID, mapper => mapper.MapFrom(p => User.NullUserGloablIdentifier));
            CreateMap<MassIncident, ExecutableInfo>()
                .ForMember(info => info.ExecutorID, mapper => mapper.MapFrom(mi => mi.ExecutedByUser.IMObjID))
                .ForMember(info => info.GroupID, mapper => mapper.MapFrom(mi => mi.ExecutedByGroupID));
            CreateMap<ChangeRequest, ExecutableInfo>()
                .ForMember(info => info.GroupID, mapper => mapper.MapFrom(rfc => rfc.QueueID))
                .ForMember(info => info.ExecutorID, mapper => mapper.MapFrom(rfc => User.NullUserGloablIdentifier));
        }
    }
}
