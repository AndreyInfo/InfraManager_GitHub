using System;
using System.Linq;
using AutoMapper;
using InfraManager.DAL.ProductCatalogue.LifeCycles;

namespace InfraManager.BLL.AccessManagement.Operations;

internal class RoleOperationNodeProfile : Profile
{
    public RoleOperationNodeProfile()
    {
        CreateMap<LifeCycle, RoleOperationNode<Guid>>()
               .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.LifeCycleStates.Any()))
               .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => ObjectClass.LifeCycle));

        CreateMap<LifeCycleState, RoleOperationNode<Guid>>()
               .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.LifeCycleStateOperations.Any()))
               .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.LifeCycleID))
               .ForMember(dst => dst.ClassID, m => m.MapFrom(scr => ObjectClass.LifeCycleState));

        CreateMap<LifeCycleStateOperation, RoleOperationNode<Guid>>()
           .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.LifeCycleStateID));
    }
}
