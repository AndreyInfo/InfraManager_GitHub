using AutoMapper;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Negotiations
{
    public class NegotiationUserPathResolver : IValueResolver<NegotiationUserDetails, NegotiationUserDetailsModel, string>
    {
        private readonly IBuildResourcePath _userPathBuilder;

        public NegotiationUserPathResolver(
            IServiceMapper<ObjectClass, IBuildResourcePath> pathBuilder)
        {
            _userPathBuilder = pathBuilder.Map(ObjectClass.User);
        }

        public string Resolve(
            NegotiationUserDetails source,
            NegotiationUserDetailsModel destination,
            string destMember,
            ResolutionContext context)
        {
            return _userPathBuilder
                .GetPathToSingle(source.UserID.ToString());
        }
    }

    public class NegotiationUserModelProfile : Profile
    {
        public NegotiationUserModelProfile()
        {

            CreateMap<NegotiationUserDetails, NegotiationUserDetailsModel>()
                .ForMember(
                    model => model.User,
                    mapper => mapper.MapFrom<NegotiationUserPathResolver>())
                .ForMember(
                    model => model.UtcDateVote,
                    mapper => mapper.MapFrom(details => details.UtcVoteDate))
                .ForMember(
                    model => model.VotingTypeString,
                    mapper => mapper.MapFrom(details => details.VotingTypeName))
                .ForMember(
                    model => model.Details,
                    mapper => mapper.MapFrom(details => details.UserDetails))
                .ForMember(
                    model => model.Email,
                    mapper => mapper.MapFrom(details => details.UserEmail))
                .ForMember(
                    model => model.PositionName,
                    mapper => mapper.MapFrom(details => details.UserPositionName))
                .ForMember(
                    model => model.SubdivisionName,
                    mapper => mapper.MapFrom(details => details.UserSubdivisionName));
        }
    }
}
