using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.DAL.ServiceDesk.Negotiations;

namespace InfraManager.BLL.ServiceDesk.Negotiations
{
    public class NegotiationUserProfile : Profile
    {
        public NegotiationUserProfile()
        {
            CreateMap<NegotiationUser, NegotiationUserDetails>()
                .ForMember(
                    details => details.UserFullName,
                    mapper => mapper.MapFrom(entity => entity.User.FullName))
                .ForMember(
                    details => details.VotingTypeName,
                    mapper => mapper.MapFrom<
                        LocalizedEnumResolver<NegotiationUser, NegotiationUserDetails, VotingType>,
                        VotingType>(
                            entity => entity.VotingType));
        }
    }
}
