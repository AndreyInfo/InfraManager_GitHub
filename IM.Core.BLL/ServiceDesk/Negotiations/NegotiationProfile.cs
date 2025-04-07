using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationProfile : Profile
    {
        public NegotiationProfile()
        {
            CreateMap<NegotiationData, Negotiation>()
                .ForMember(
                    entity => entity.Mode,
                    mapper => mapper.MapFrom((patch, state) => patch.Mode ?? state.Mode))
                .IgnoreOtherNulls();
            CreateMap<Negotiation, NegotiationDetails>()
                .ForMember(
                    details => details.ID, 
                    mapper => mapper.MapFrom(entity => entity.IMObjID))
                .ForMember(
                    details => details.ModeName,
                    mapper => mapper.MapFrom<
                        LocalizedEnumResolver<Negotiation, NegotiationDetails, NegotiationMode>,
                        NegotiationMode>(
                            entity => entity.Mode))
                .ForMember(
                    details => details.StatusName,
                    mapper => mapper.MapFrom<
                        LocalizedEnumResolver<Negotiation, NegotiationDetails, NegotiationStatus>,
                        NegotiationStatus>(
                            entity => entity.Status))
                .ForMember(
                    details => details.SettingCommentPlacet,
                    mapper => mapper.MapFrom<
                        SettingResolver<Negotiation, NegotiationDetails, bool>,
                        SystemSettings>(_ => SystemSettings.CommentPlacet))
                .ForMember(
                    details => details.SettingCommentNonPlacet,
                    mapper => mapper.MapFrom<
                        SettingResolver<Negotiation, NegotiationDetails, bool>,
                        SystemSettings>(_ => SystemSettings.CommentNonPlacet));
        }
    }
}
