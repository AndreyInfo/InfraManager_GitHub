using AutoMapper;
using InfraManager.BLL.ServiceDesk.Problems;
using InfraManager.WebApi.Contracts.Models.ServiceDesk.Problems;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.Problems
{
    public class ProblemTypeModelProfile : Profile
    {
        public ProblemTypeModelProfile()
        {
            CreateMap<ProblemTypeDetails, ProblemTypeDetailsModel>();
        }
    }
}