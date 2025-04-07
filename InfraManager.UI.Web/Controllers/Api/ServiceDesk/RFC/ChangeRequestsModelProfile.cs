using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceDesk.ChangeRequests;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.ChangeRequest;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    public class ChangeRequestsModelProfile : Profile
    {
        public ChangeRequestsModelProfile()
        {
            CreateMap<ChangeRequestDetails, ChangeRequestDetailsModel>()
                .ForMember(
                    model => model.ManhoursString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ChangeRequestDetails, ChangeRequestDetailsModel>,
                            int>(details => details.ManhoursInMinutes))
                .ForMember(
                    model => model.ManhoursNormString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ChangeRequestDetails, ChangeRequestDetailsModel>,
                            int>(details => details.ManhoursNormInMinutes))
                .ForMember(
                    model => model.ManHours,
                    mapper =>
                        mapper.MapFrom(details => details.ManhoursInMinutes))
                .ForMember(
                    model => model.ManhoursNorm,
                    mapper =>
                        mapper.MapFrom(details => details.ManhoursNormInMinutes));

            CreateMap<ChangeRequestListItem, ChangeRequestListItemModel>()
                .ForMember(m => m.Uri, 
                    mapper => 
                        mapper.MapFrom<
                            PathResolver<ChangeRequestListItem, ChangeRequestListItemModel>,
                            InframanagerObject?>(item => new InframanagerObject(item.ID, item.ClassID)));

            CreateMap<ChangeRequestDataModel, ChangeRequestData>();
            CreateMap<ChangeRequestReferenceListItem, ChangeRequestReferenceListItemModel>();
        }
    }
}
