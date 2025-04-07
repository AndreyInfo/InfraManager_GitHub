using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.Documents;

namespace InfraManager.BLL.Mapping
{
    public class DocumentDataProfile : Profile
    {
        public DocumentDataProfile()
        {
            CreateMap<Document, DocumentDataDetails>()
                .ForMember(dst => dst.Id, m => m.MapFrom(src => src.ID))
                .ForMember(dst => dst.Name, m => m.MapFrom(src => src.Name))
                .ForMember(dst => dst.Extension, m => m.MapFrom(src => src.Extension))
                .ForMember(dst => dst.Data, m => m.MapFrom(src => src.Data));
            CreateMap<Document, DocumentInfoDetails>();
        }
    }
}
