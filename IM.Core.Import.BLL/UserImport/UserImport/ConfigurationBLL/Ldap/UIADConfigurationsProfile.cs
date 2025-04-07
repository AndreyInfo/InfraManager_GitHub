using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADConfigurationsProfile : Profile
{
    public UIADConfigurationsProfile()
    {
        CreateMap<UIADConfiguration, UIADConfigurationsOutputDetails>()
            ;

        CreateMap<UIADConfigurationsDetails, UIADConfiguration>();
    }
}