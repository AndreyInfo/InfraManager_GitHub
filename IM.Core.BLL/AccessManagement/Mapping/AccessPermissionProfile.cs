using AutoMapper;
using InfraManager.BLL.AccessManagement.AccessPermissions;
using InfraManager.DAL.AccessManagement;

namespace InfraManager.BLL.AccessManagement.Mapping;

internal class AccessPermissionProfile : Profile
{
    public AccessPermissionProfile()
    {
        CreateMap<AccessPermission, AccessPermissionDetails>()
            .ForMember(dst => dst.Rights, m => m.MapFrom(scr => 
                    new AccessPermissionRightsDetails(scr.Properties, scr.Add, scr.Delete, scr.Update, scr.AccessManage)))
                .ReverseMap()
            .ForMember(dst => dst.Properties, m => m.MapFrom(scr => scr.Rights.HasPropertiesPermissions))
            .ForMember(dst => dst.Add, m => m.MapFrom(scr => scr.Rights.HasAddPermissions))
            .ForMember(dst => dst.Delete, m => m.MapFrom(scr => scr.Rights.HasDeletePermissions))
            .ForMember(dst => dst.Update, m => m.MapFrom(scr => scr.Rights.HasUpdatePermissions))
            .ForMember(dst => dst.AccessManage, m => m.MapFrom(scr => scr.Rights.HasAccessManagePermissions));

        CreateMap<AccessPermissionModelItem, AccessPermissionDetails>()
            .ForMember(dst => dst.Rights, m => m.MapFrom(scr => 
                    new AccessPermissionRightsDetails(scr.Properties, scr.Add, scr.Delete, scr.Update, scr.AccessManage)));
    }
}
