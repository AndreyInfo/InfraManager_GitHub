using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADIMFieldConcordancesProfile : Profile
{
    public UIADIMFieldConcordancesProfile()
    {
        CreateMap<UIADIMFieldConcordance, UIADIMFieldConcordancesOutputDetails>()
            .ForMember(x=>x.ADClassID, x=>x.MapFrom(
                y=>y.ClassID))
            .ForMember(x=>x.ADConfigurationID, x=>x.MapFrom(y=>y.ConfigurationID));

        CreateMap<UIADIMFieldConcordancesDetails, UIADIMFieldConcordance>();

        CreateMap<UIADIMFieldConcordanceInsertDetails, UIADIMFieldConcordance>()
            .ForMember(x=>x.ClassID, x=>x.MapFrom(y=>y.ADClassID))
            .ForMember(x=>x.ConfigurationID, x=>x.MapFrom(y=>y.ADConfigurationID));
    }
}