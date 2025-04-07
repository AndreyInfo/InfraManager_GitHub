using AutoMapper;
using InfraManager.BLL.OrganizationStructure;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.WebApi.Contracts.OrganizationStructure;

namespace InfraManager.UI.Web.Controllers.Api.OrganizationStructure
{
    //TODO перенести в BLL
    public class DeputyUserProfile : Profile
    {
        public DeputyUserProfile()
        {
            CreateMap<DeputyUserData, DeputyUser>()
                .ForMember(
                    entity => entity.UtcDataDeputyWith,
                    mapping => mapping.MapFrom(data => data.UtcDeputySince))
                .ForMember(
                    entity => entity.UtcDataDeputyBy,
                    mapping => mapping.MapFrom(data => data.UtcDeputyUntil));

            CreateMap<DeputyUserDetails, DeputyUserDetailsModel>()
                .ForMember(
                    model => model.ParentUserFullName,
                    mapping => mapping.MapFrom(details => details.ParentFullName))
                .ForMember(
                    model => model.ChildUserFullName,
                    mapping => mapping.MapFrom(details => details.ChildFullName))
                .ForMember(
                    model => model.UtcDataDeputyBySt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyBy))
                .ForMember(
                    model => model.UtcDataDeputyWithSt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyWith));

            CreateMap<DeputyUserDetails, DeputyUserListItem>()
                .ForMember(
                    model => model.UserFullName,
                    mapping => mapping.MapFrom(details => details.ChildFullName))
                .ForMember(
                    model => model.UtcDataDeputyBySt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyBy))
                .ForMember(
                    model => model.UtcDataDeputyWithSt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyWith));

            CreateMap<DeputyUserDetails, IDeputyForUserListItem>()
                .ForMember(
                    model => model.UserFullName,
                    mapping => mapping.MapFrom(details => details.ParentFullName))
                .ForMember(
                    model => model.UtcDataDeputyBySt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyBy))
                .ForMember(
                    model => model.UtcDataDeputyWithSt,
                    mapping => mapping.MapFrom(details => details.UtcDataDeputyWith));
        }
    }
}