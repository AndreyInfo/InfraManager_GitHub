using AutoMapper;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.ServiceDesk.KB.Statuses;

public class KnowledgeBaseStatusProfile : Profile
{
    public KnowledgeBaseStatusProfile()
    {
        CreateMap<KnowledgeBaseArticleStatus, KBArticleStatusDetails>();
        CreateMap<LookupData, KnowledgeBaseArticleStatus>();
    }
}