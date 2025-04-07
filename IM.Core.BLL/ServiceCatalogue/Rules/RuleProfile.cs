using AutoMapper;
using InfraManager.DAL;
using InfraManager.DAL.ServiceCatalogue;

namespace InfraManager.BLL.ServiceCatalogue.Rules
{
    internal class RuleProfile : Profile
    {
        public RuleProfile()
        {
            CreateMap<Rule, RuleDetails>()
                .ForMember(x => x.SLAID, x => x.MapFrom(x => x.ServiceLevelAgreementID));

            CreateMap<RuleValue, RuleValueDetails>().ReverseMap();

            CreateMap<RuleData, Rule>()
                .ForMember(x => x.ServiceLevelAgreementID, x => x.MapFrom(x => x.SLAID));

            //TODO подправить работу с правилами
            CreateMap<ServiceDetails, Service>();
            
            CreateMap<ServiceDetails, RuleValue>();
        }
    }
}
