using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADSettingsProfile : Profile
{
    public UIADSettingsProfile()
    {
        CreateMap<UIADSetting, UIADSettingsOutputDetails>();

        CreateMap<UIADSettingsDetails, UIADSetting>();
    }
}