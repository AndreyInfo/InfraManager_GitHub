using AutoMapper;
using InfraManager.BLL.KnowledgeBase.KnowledgeBaseClassifiers;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KnowledgeBase.KnowledgeBaseClassifiers;

public class KnowledgeBaseClassifierProfile : Profile
{
    public KnowledgeBaseClassifierProfile()
    {
        CreateMap<KBArticleFolder, KnowledgeBaseClassifierDetails>()
            .ForMember(x => x.ExpertID, x => x.MapFrom(x => x.Expert.IMObjID))
            .ForMember(x => x.ExpertName, x => x.MapFrom(x => x.Expert.Name))
            .ForMember(x => x.ParentName, x => x.MapFrom(x => x.Parent.Name));
        
        CreateMap<KnowledgeBaseClassifierData, KBArticleFolder>()
            .ForMember(x => x.ExpertID, x => x.Ignore());
    }
}