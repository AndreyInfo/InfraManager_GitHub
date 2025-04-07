using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.UI.Web.AutoMapper;
using InfraManager.Web.BLL.Helpers;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.MassIncidents;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;
using System.Linq;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    public class ProblemModelProfile : Profile
    {
        public ProblemModelProfile()
        {
            CreateMap<ProblemDetails, ProblemDetailsModel>()
                .ForMember(
                    model => model.PriorityColor,
                    mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(
                    model => model.ManhoursString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ProblemDetails, ProblemDetailsModel>,
                            int>(details => details.ManhoursInMinutes))
                .ForMember(
                    model => model.ManhoursNormString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<ProblemDetails, ProblemDetailsModel>,
                            int>(details => details.ManhoursNormInMinutes))
                .ForMember(
                    model => model.Calls,
                    mapper => mapper.MapFrom<
                        PathsArrayResolver<ProblemDetails, ProblemDetailsModel>,
                        InframanagerObject[]>(
                            details => details.CallIds
                                .Select(
                                    callId => 
                                        new InframanagerObject(
                                            callId, 
                                            ObjectClass.Call))
                                .ToArray()));
            CreateMap<ProblemListItem, ProblemListItemModel>()
                .ForMember(
                    model => model.Uri,
                    mapper => mapper.MapFrom<
                            PathResolver<ProblemListItem, ProblemListItemModel>,
                            InframanagerObject?>(
                                item => new InframanagerObject(item.ID, item.ClassID)))
                .ForMember(
                    model => model.TypeImage,
                    mapper => mapper.MapFrom(item => $"api/problemTypes/{item.TypeID}/image"))
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>());

            CreateMap<ProblemReferenceListItem, ProblemReferenceListItemModel>();
        }
    }
}
