using AutoMapper;
using InfraManager.BLL.AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.BLL.ServiceDesk.Calls;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Calls;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Calls
{
    public class CallModelProfile : Profile
    {
        public CallModelProfile()
        {
            CreateMap<CallDetails, CallDetailsModel>()
                .ForMember(model => model.PriorityColor, mapper => mapper.MapFrom<PriorityColorResolver>())
                .ForMember(model => model.Grade, mapper => mapper.MapFrom(data => string.IsNullOrEmpty(data.Grade) ? null : (byte?)byte.Parse(data.Grade)))
                .ForMember(
                    model => model.ManhoursString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<CallDetails, CallDetailsModel>,
                            int>(details => details.ManhoursInMinutes))
                .ForMember(
                    model => model.ManhoursNormString,
                    mapper =>
                        mapper.MapFrom<
                            ManhoursResolver<CallDetails, CallDetailsModel>,
                            int>(details => details.ManhoursNormInMinutes));
        }
    }
}
