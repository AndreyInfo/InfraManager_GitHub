using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADPathsProfile : Profile
{
    public UIADPathsProfile()
    {
        CreateMap<UIADPath, UIADPathsOutputDetails>();

        CreateMap<UIADPathsDetails, UIADPath>();
    }
}