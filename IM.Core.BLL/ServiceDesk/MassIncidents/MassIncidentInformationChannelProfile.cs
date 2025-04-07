using AutoMapper;
using InfraManager.BLL.Localization;
using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.ServiceDesk.MassIncidents
{
    internal class MassiveIncidentInformationChannelNameResolver : IMemberValueResolver<MassIncidentInformationChannel, LookupListItem<short>, string, string>
    {
        private readonly ILocalizeText _localizer;

        public MassiveIncidentInformationChannelNameResolver(ILocalizeText localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(
            MassIncidentInformationChannel source, 
            LookupListItem<short> destination, 
            string sourceMember, 
            string destMember, 
            ResolutionContext context)
        {
            return _localizer.Localize(source.ResourceKey);
        }
    }

    public class MassIncidentInformationChannelProfile : Profile
    {
        public MassIncidentInformationChannelProfile()
        {
            CreateMap<MassIncidentInformationChannel, LookupListItem<short>>()
                .ForMember(
                    item => item.Name,
                    mapper => mapper.MapFrom<MassiveIncidentInformationChannelNameResolver, string>(entity => entity.Name));              
        }
    }
}
