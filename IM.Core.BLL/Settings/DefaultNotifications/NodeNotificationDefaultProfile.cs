using AutoMapper;
using InfraManager.DAL.Notification;
using System.Linq;
using System;
using Model = InfraManager.DAL.Notification;

namespace InfraManager.BLL.Settings.DefaultNotifications;

internal class NodeNotificationDefaultProfile : Profile
{
    public NodeNotificationDefaultProfile()
    {
        CreateMap<(ObjectClass classID, string className, NodeNotificationDefaultDetails<BusinessRole>[] children)
          , NodeNotificationDefaultDetails<ObjectClass>>()
          .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.classID))
          .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.className))
          .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.children.Any()))
          .ForMember(dst => dst.IsSelectFull, m => m.MapFrom(scr => scr.children.All(c => c.IsSelectFull)))
          .ForMember(dst => dst.IsSelectPart, m => m.MapFrom(scr => scr.children.Any(c => c.IsSelectPart || c.IsSelectFull)
                                                                    && !scr.children.All(c => c.IsSelectFull)));
                                                                    

        CreateMap <(BusinessRole role, ObjectClass classID, Model.Notification[] notifiations, string name)
            , NodeNotificationDefaultDetails<BusinessRole>>()
            .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.name))
            .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.role))
            .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.classID))
            .ForMember(dst => dst.HasChild, m => m.MapFrom(scr => scr.notifiations.Any()))
            .ForMember(dst => dst.IsSelectFull, m => m.MapFrom(scr => scr.notifiations.All(v => v.DefaultBusinessRole.HasFlag(scr.role))))
            .ForMember(dst => dst.IsSelectPart, m => m.MapFrom(scr => scr.notifiations.Any(v => v.DefaultBusinessRole.HasFlag(scr.role))
                                                                      && !scr.notifiations.All(v => v.DefaultBusinessRole.HasFlag(scr.role))));

        CreateMap<(Model.Notification notifiation, BusinessRole role)
           , NodeNotificationDefaultDetails<Guid>>()
           .ForMember(dst => dst.ID, m => m.MapFrom(scr => scr.notifiation.ID))
           .ForMember(dst => dst.Name, m => m.MapFrom(scr => scr.notifiation.Name))
           .ForMember(dst => dst.ParentID, m => m.MapFrom(scr => scr.role))
           .ForMember(dst => dst.IsSelectFull, m => m.MapFrom(scr => scr.notifiation.DefaultBusinessRole != BusinessRole.None
                                                                     && scr.notifiation.DefaultBusinessRole.HasFlag(scr.role)));
    }
}
