using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Negotiations;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Negotiations;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Negotiations
{
    public class NegotiationObjectPathResolver : IValueResolver<NegotiationDetails, NegotiationDetailsModel, string>
    {
        private readonly IServiceMapper<ObjectClass, IBuildResourcePath> _pathBuilder;

        public NegotiationObjectPathResolver(
            IServiceMapper<ObjectClass, IBuildResourcePath> pathBuilder)
        {
            _pathBuilder = pathBuilder;
        }

        public string Resolve(
            NegotiationDetails source, 
            NegotiationDetailsModel destination, 
            string destMember, 
            ResolutionContext context)
        {
            return _pathBuilder
                .Map(source.ObjectClassID)
                .GetPathToSingle(source.ObjectID.ToString());
        }
    }

    public class NegotiationModelProfile : Profile
    {
        public NegotiationModelProfile()
        {
            CreateMap<NegotiationDetails, NegotiationDetailsModel>()
                .ForMember(
                    x => x.Object,
                    mapper => mapper.MapFrom<NegotiationObjectPathResolver>());
            CreateMap<NegotiationListItem, NegotiationReportItemModel>()
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                            PathResolver<NegotiationListItem, NegotiationReportItemModel>,
                            InframanagerObject?>(
                                item => new InframanagerObject(item.ObjectID, item.ClassID)))
               .ForMember(
                    model => model.IMObjID,
                    mapper => mapper.MapFrom(item => item.ObjectID))
               .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());
        }
    }
}
