using InfraManager.CrossPlatform.WebApi.Contracts.Common;
using System;

namespace InfraManager.BLL.KnowledgeBase
{
    public class KBArticleAccessListFilter : BaseFilter
    {
        public Guid? KbArticleID { get; init; }
    }
}
