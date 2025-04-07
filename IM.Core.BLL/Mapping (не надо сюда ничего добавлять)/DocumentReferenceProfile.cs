using AutoMapper;
using InfraManager.BLL.ServiceDesk;
using InfraManager.DAL.Documents;

namespace InfraManager.BLL.Mapping
{
    public class DocumentReferenceProfile : Profile
    {
        public DocumentReferenceProfile()
        {
            CreateMap<DocumentReference, DocumentReferenceDetails>();
        }
    }
}
