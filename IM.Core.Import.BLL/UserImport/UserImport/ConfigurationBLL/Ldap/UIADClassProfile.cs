using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADClassProfile : Profile
{
    public UIADClassProfile()
    {
        CreateMap<UIADClass, UIADClassOutputDetails>();

        CreateMap<UIADClassDetails, UIADClass>();
    }
}