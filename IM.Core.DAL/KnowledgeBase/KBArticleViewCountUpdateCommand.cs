using InfraManager;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.KnowledgeBase
{
    internal class KBArticleViewCountUpdateCommand :
                        IKBArticleViewCountUpdateCommand,
                        ISelfRegisteredService<IKBArticleViewCountUpdateCommand>
    {
        private readonly CrossPlatformDbContext _dbContext;

        public KBArticleViewCountUpdateCommand(CrossPlatformDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task ExecuteAsync(Guid articleId, CancellationToken cancellationToken)
        {
            var kbArticleSet = _dbContext.Set<KBArticle>();
            var viewCount = await kbArticleSet
                                .Where(x => x.ID == articleId)
                                .Select(x => x.ViewsCount)
                                .FirstAsync(cancellationToken);
            var article = new KBArticle()
            {
                ID = articleId,
                ViewsCount = viewCount + 1
            };
            kbArticleSet.Attach(article);
            _dbContext.Entry(article).Property(x => x.ViewsCount).IsModified = true;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
