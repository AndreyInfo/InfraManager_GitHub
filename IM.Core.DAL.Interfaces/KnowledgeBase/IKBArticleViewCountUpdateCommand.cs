using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.KnowledgeBase
{
    public interface IKBArticleViewCountUpdateCommand
    {
        Task ExecuteAsync(Guid articleId, CancellationToken cancellationToken);
    }
}
