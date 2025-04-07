using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.Documents;
using InfraManager.WebApi.Contracts.Models.Documents;

namespace InfraManager.UI.Web.Controllers.Api.Documents
{
    //TODO перенести в BLL
    public class DocumentInfoDetailsProfile : Profile
    {
        public DocumentInfoDetailsProfile()
        {
            CreateMap<DocumentInfoDetails, DocumentInfoDetailsModel>();
            CreateMap<Document, DocumentInfoDetails>()
                .ForMember(dst => dst.Size, m => m.MapFrom(src => src.Data.Length));
        }
    }
}
