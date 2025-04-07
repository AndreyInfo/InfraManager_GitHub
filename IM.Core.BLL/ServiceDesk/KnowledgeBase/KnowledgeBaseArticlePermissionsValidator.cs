using InfraManager.DAL.ServiceDesk;
using System;
using System.Threading;
using System.Threading.Tasks;
using Inframanager.BLL;
using System.Linq.Expressions;
using InfraManager.DAL.KnowledgeBase;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InfraManager.BLL.ServiceDesk
{
    internal class KnowledgeBaseArticlePermissionsValidator : 
        IValidateObjectPermissions<Guid, KBArticle>,
        ISelfRegisteredService<IValidateObjectPermissions<Guid, KBArticle>>
    {
        private readonly IKnowledgeBaseQuery _knowledgeBaseQuery;

        public KnowledgeBaseArticlePermissionsValidator(IKnowledgeBaseQuery knowledgeBaseQuery)
        {   
            _knowledgeBaseQuery = knowledgeBaseQuery;
        }

        public IEnumerable<Expression<Func<KBArticle, bool>>> ObjectIsAvailable(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ObjectIsAvailableAsync(Guid userId, Guid articleID, CancellationToken cancellationToken = default)
        {
            return (await _knowledgeBaseQuery.CheckArticlesAccessAsync(new[] { articleID }, userId, cancellationToken)).Any();
        }
    }
}
