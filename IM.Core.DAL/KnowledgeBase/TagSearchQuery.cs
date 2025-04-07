using System.Linq;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;

namespace InfraManager.DAL.KnowledgeBase
{
    public class TagSearchQuery : ITagSearchQuery,
        ISelfRegisteredService<ITagSearchQuery>
    {
        private readonly DbSet<KBTag> _knowledgeBaseTags;

        public TagSearchQuery(DbSet<KBTag> knowledgeBaseTags)
        {
            _knowledgeBaseTags = knowledgeBaseTags;
        }

        public IQueryable<ObjectSearchResult> Query(SearchCriteria searchCriteria)
        {
            var query = _knowledgeBaseTags.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchCriteria.Text))
            {
                var searchPattern = searchCriteria.Text.ToLower().ToContainsPattern();
                query = query.Where(x =>
                    EF.Functions.Like(x.Name.ToLower(), searchPattern));
            }

            return query.Select(
                x => new ObjectSearchResult
                {
                    ID = x.Id,
                    ClassID = ObjectClass.KBArticleTag,
                    FullName = x.Name
                });
        }
    }
}