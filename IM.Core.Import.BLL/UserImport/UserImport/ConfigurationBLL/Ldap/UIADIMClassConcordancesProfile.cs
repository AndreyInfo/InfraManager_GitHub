using AutoMapper;
using IM.Core.Import.BLL.Interface.Ldap.Import;
using Inframanager.DAL.ActiveDirectory.Import;
using InfraManager.ServiceBase.ImportService.LdapModels;

namespace IM.Core.Import.BLL.Ldap.Import;

internal class UIADIMClassConcordancesProfile : Profile
{
    public UIADIMClassConcordancesProfile()
    {
        CreateMap<UIADIMClassConcordance, UIADIMClassConcordancesOutputDetails>();
        
        CreateMap<UIADIMClassConcordancesDetails, UIADIMClassConcordance>();
        
         CreateMap<UIADIMFieldConcordanceInsertDetails, UIADIMFieldConcordance>()
             .ForMember(x=>x.ClassID, x=>x.MapFrom(y=>y.ADClassID))
             .ForMember(x=>x.ConfigurationID, x=>x.MapFrom(y=>y.ADConfigurationID));


    }
}