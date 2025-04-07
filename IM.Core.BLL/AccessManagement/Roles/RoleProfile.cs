using System;
using AutoMapper;
using InfraManager.BLL.Roles;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement.Roles;

internal class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleData, Role>();
        
        CreateMap<Role, RoleListItemDetails>();
        CreateMap<Role, LookupListItem<Guid>>();

        CreateMap<Role, RoleDetails>()
            .ReverseMap();

        CreateMap<RoleInsertDetails, Role>()
            .ConstructUsing(c => new(c.Name));

        CreateMap<RoleOperation, OperationModelForRole<int>>()
            .ForMember(c => c.Name, m => m.MapFrom(scr => scr.Operation.OperationName))
            .ReverseMap()
            .ForMember(c => c.Operation, m => m.Ignore());

        CreateMap<RoleLifeCycleStateOperation, OperationModelForRole<Guid>>()
            .ForMember(c => c.OperationID, m => m.MapFrom(scr => scr.LifeCycleStateOperationID))
            .ForMember(c => c.Name, m => m.MapFrom(scr => scr.LifeCycleStateOperation.Name))
            .ReverseMap()
            .ForMember(c => c.LifeCycleStateOperationID, m => m.MapFrom(scr => scr.OperationID))
            .ForMember(c => c.LifeCycleStateOperation, m => m.Ignore());


        CreateMap<Role, UserRolesWithSelectedDetails>();
    }
}
