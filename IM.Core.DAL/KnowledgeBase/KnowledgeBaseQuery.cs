using InfraManager.DAL.AccessManagement;
using InfraManager.DAL.Documents;
using InfraManager.DAL.OrganizationStructure;
using InfraManager.DAL.ProductCatalogue.LifeCycles;
using InfraManager.DAL.ServiceCatalogue;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.DAL.KnowledgeBase
{
    internal class KnowledgeBaseQuery : IKnowledgeBaseQuery, ISelfRegisteredService<IKnowledgeBaseQuery>
    {
        private const string DeletedEntryDescription = "[УДАЛЕН]";
        private readonly DbContext dbContext;

        public KnowledgeBaseQuery(CrossPlatformDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private async Task<IQueryable<KBArticle>> SetAccessFilterAsync(IQueryable<KBArticle> queryable, Guid userID, CancellationToken cancellationToken)
        {
            var onlyItAccess = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var byClassAccess = Guid.Parse("00000000-0000-0000-0000-000000000002");
            var isAdmin = await dbContext.Set<UserRole>()
                                    .AnyAsync(x => x.UserID == userID && x.RoleID == Role.AdminRoleId, cancellationToken);

            if (isAdmin) return queryable.Include(i => i.AccessList);

            var user = await dbContext.Set<User>()
                .Include(i => i.Subdivision)
                .SingleOrDefaultAsync(s => s.IMObjID == userID, cancellationToken);

            var isIt = await CheckItUserAsync(user, cancellationToken);

            var userGroupsQuery = dbContext.Set<GroupUser>()
                .Where(w => w.UserID == userID);

            var userOrganizationID = user?.Subdivision?.OrganizationID;
            var userSubDivisionID = user?.SubdivisionID;
            IEnumerable<Guid> userSubDivisionHierarchy = null;
            if (userSubDivisionID.HasValue)
            {
                //структура сущности подразделений имеет родителя в виде ссылки на саму себя, нужно найти всех родителей, т.к. права выдаются с учетом вложенных подразделений
                var allSubdivisions = await dbContext.Set<Subdivision>()
                    .ToArrayAsync(cancellationToken);
                userSubDivisionHierarchy = user.Subdivision.GetBranch(allSubdivisions).Select(h => h.ID);
            }

            return queryable.Include(i => i.AccessList)
                .Where(x => x.ArticleAccessID == Guid.Empty
                            || (x.ArticleAccessID == onlyItAccess && isIt)
                            || (x.ArticleAccessID == byClassAccess &&
                                (
                                    x.AccessList.AsQueryable().Any(KBArticleAccessList.IsAccessedByClass(ObjectClass.User, userID))
                                    //todo: тот же фильтр, что и в KBArticleAccessList.IsAccessedByClass, но не придумал, как прикрутить в подзапрос
                                    || userGroupsQuery.Any(a => x.AccessList.AsQueryable().Any(access => access.ObjectClass == ObjectClass.Group && access.ObjectID == a.GroupID))
                                    || (userOrganizationID.HasValue && x.AccessList.AsQueryable().Any(KBArticleAccessList.IsAccessedByClass(ObjectClass.Organizaton, userOrganizationID.Value)))
                                    || (userSubDivisionID.HasValue &&
                                        (
                                            (userSubDivisionHierarchy == null && x.AccessList.AsQueryable().Any(KBArticleAccessList.IsAccessedByClass(ObjectClass.Division, userSubDivisionID.Value))) ||
                                            (userSubDivisionHierarchy != null && x.AccessList.AsQueryable().Any(access => access.ObjectClass == ObjectClass.Division &&
                                                (access.WithSub ? userSubDivisionHierarchy.Contains(access.ObjectID) : access.ObjectID == userSubDivisionID.Value ))
                                            )
                                        )
                                    )
                            ))
                );
        }

        public async Task<KBArticleFolderItem[]> GetAccessFoldersAsync(Guid userID, CancellationToken cancellationToken)
        {
            var queryArticle = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .AsQueryable();
            var queryArticleAccessFilter = await SetAccessFilterAsync(queryArticle, userID, cancellationToken);

            var items = await (from kbArticle in queryArticleAccessFilter
                     join accessFolders in dbContext.Set<KBArticleReference>().AsNoTracking()
                        on kbArticle.ID equals accessFolders.ArticleId
                    join folders in dbContext.Set<KBArticleFolder>().AsNoTracking()
                        on accessFolders.ObjectId equals folders.ID
                            where accessFolders.ObjectClassID == ObjectClass.KBArticleFolder
                     select new KBArticleFolderItem()
                     {
                         ID = folders.ID,
                         Name = folders.Name,
                         Note = folders.Note,
                         Visible = folders.Visible,
                         HasChilds = dbContext.Set<KBArticleFolder>().Any(y => y.ParentID == folders.ID),
                         ParentId = folders.ParentID,
                         FullName = folders.Name
                     }).ToListAsync(cancellationToken);

            items.Where(x => x.ParentId.HasValue)
                 .ForEach(x => CreateFullName(x, items));

            var needFolderWithoutCategory = await queryArticleAccessFilter.AnyAsync(x => !dbContext.Set<KBArticleReference>()
                                            .Any(y => y.ArticleId == x.ID && y.ObjectClassID == ObjectClass.KBArticleFolder && y.ObjectId != Guid.Empty)
                                         || dbContext.Set<KBArticleReference>().Any(y => y.ObjectId == Guid.Empty && y.ArticleId == x.ID), cancellationToken);
            if (needFolderWithoutCategory)
            {
                items.Add(new KBArticleFolderItem
                {
                    ID = Guid.Empty,
                    FullName = "Без категории",
                    Name = "Без категории",
                    Note = "",
                    Visible = true,
                    HasChilds = false
                });
            }

            return items.ToArray();
        }

        public async Task<KBArticleItem> GetArticleAsync(Guid articleId, Guid userId, CancellationToken cancellationToken)
        {
            var qryArticle = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .AsQueryable()
                .Where(x => x.ID == articleId);
            var query = await SetAccessFilterAsync(qryArticle, userId, cancellationToken);
            var joinquery = from article in query
                            join articleStatus in dbContext.Set<KnowledgeBaseArticleStatus>().AsNoTracking()
                            on article.ArticleStatusID equals articleStatus.ID
                            into statusJoin
                            from articleStatus in statusJoin.DefaultIfEmpty()

                            join articleType in dbContext.Set<KBArticleType>().AsNoTracking()
                            on article.ArticleTypeID equals articleType.ID
                            into typeJoin
                            from articleType in typeJoin.DefaultIfEmpty()

                            join articleParams in dbContext.Set<KBArticleParameter>().AsNoTracking()
                            on article.ID equals articleParams.ID
                            into paramJoin
                            from articleParams in paramJoin.DefaultIfEmpty()

                            join articleAccess in dbContext.Set<KBArticleAccess>().AsNoTracking()
                            on article.ArticleAccessID equals articleAccess.ArticleAccessId
                            into accessJoin
                            from articleAccess in accessJoin.DefaultIfEmpty()

                            join articleLifeCycle in dbContext.Set<LifeCycleState>().AsNoTracking()
                            on article.ArticleAccessID equals articleLifeCycle.ID
                            into lifeCycleJoin
                            from articleLifeCycle in lifeCycleJoin.DefaultIfEmpty()

                            let appCount = dbContext.Set<KBArticleReference>()
                                .AsNoTracking()
                                .Count(x => x.ArticleId == article.ID &&
                                       (x.ObjectClassID == ObjectClass.Call || x.ObjectClassID == ObjectClass.Problem))

                            select new KBArticleItem
                            {
                                ID = article.ID,
                                Name = article.Name,
                                Number = article.Number,
                                UtcDateCreated = article.UtcDateCreated,
                                UtcDateModified = article.UtcDateModified,
                                AuthorID = article.AuthorID,
                                Description = article.Description,
                                Solution = article.Solution,
                                HTMLDescription = article.HTMLDescription,
                                HTMLSolution = article.HTMLSolution,
                                Visible = article.Visible,
                                HTMLAlternativeSolution = article.HTMLAlternativeSolution,
                                AlternativeSolution = article.AlternativeSolution,
                                StatusID = articleStatus.ID,
                                StatusName = articleStatus.Name,
                                TypeID = articleType.ID,
                                TypeName = articleType.Name,
                                Readed = articleParams != null ? articleParams.ReadCount : 0,
                                Used = articleParams != null ? articleParams.UseCount : 0,
                                Rated = articleParams != null ? articleParams.Rating : 0,
                                ViewsCount = article.ViewsCount,
                                ExpertID = article.ExpertID,
                                ModifierID = article.ModifierID,
                                UtcDateValidUntil = article.UtcDateValidUntil,
                                AccessID = article.ArticleAccessID,
                                AccessName = articleAccess.ArticleAccessName,
                                ApplicationCount = appCount,
                                LifeCycleStateID = article.LifeCycleStateID,
                                LifeCycleStateName = articleLifeCycle.Name,
                                VisibleForClient = article.Visible,
                                AuthorFullName = dbContext.Set<User>()
                                        .AsNoTracking()
                                        .Where(y => y.IMObjID == article.AuthorID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName)
                                        .FirstOrDefault(),
                                ExpertFullName = dbContext.Set<User>()
                                        .AsNoTracking()
                                        .Where(y => y.IMObjID == article.ExpertID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName)
                                        .FirstOrDefault(),
                                ModifierFullName = dbContext.Set<User>()
                                        .AsNoTracking()
                                        .Where(y => y.IMObjID == article.ModifierID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName)
                                        .FirstOrDefault(),
                            };
            var item = await joinquery.FirstOrDefaultAsync(cancellationToken);
            if (item != null)
            {
                item.Tags = await dbContext.Set<KBArticleReference>()
                                     .AsNoTracking()
                                     .Where(x => x.ArticleId == item.ID && x.ObjectClassID == ObjectClass.KBArticleTag)
                                     .Select(x => dbContext.Set<KBTag>().AsNoTracking()
                                                        .Where(y => y.Id == x.ObjectId)
                                                        .Select(k => k.Name)
                                                        .First())
                                     .ToArrayAsync(cancellationToken);
            }
            return item;
        }

        public async Task<KBArticleTypeItem[]> GetArticleTypesAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Set<KBArticleType>().AsNoTracking()
                        .Select(x => new KBArticleTypeItem()
                        {
                            ID = x.ID,
                            Name = x.Name
                        })
                        .ToArrayAsync(cancellationToken);
        }

        public async Task<KBArticleStatusItem[]> GetArticleStatusesAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Set<KnowledgeBaseArticleStatus>().AsNoTracking()
                        .Select(x => new KBArticleStatusItem()
                        {
                            ID = x.ID,
                            Name = x.Name
                        })
                        .ToArrayAsync(cancellationToken);
        }

        public async Task<KBArticleAccessItem[]> GetArticleAccessAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Set<KBArticleAccess>().AsNoTracking()
                        .Select(x => new KBArticleAccessItem()
                        {
                            ID = x.ArticleAccessId,
                            Name = x.ArticleAccessName
                        })
                        .ToArrayAsync(cancellationToken);
        }

        public async Task<KBArticleInfoItem[]> GetArticleInfoByIdsAsync(Guid[] articleIds, Guid? serviceItemAttendanceID, CancellationToken cancellationToken)
        {
            Guid? serviceId = null;
            if (serviceItemAttendanceID.HasValue)
            {
                serviceId = await dbContext.Set<ServiceItem>().AsNoTracking()
                                .Where(x => x.ID == serviceItemAttendanceID)
                                .Select(x => x.Service.ID)
                                .FirstOrDefaultAsync(cancellationToken);
                if (!serviceId.HasValue)
                {
                    serviceId = await dbContext.Set<ServiceAttendance>().AsNoTracking()
                                .Where(x => x.ID == serviceItemAttendanceID)
                                .Select(x => x.ServiceID)
                                .FirstOrDefaultAsync(cancellationToken);
                }
            }

            var query = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .Where(x => x.Visible == true && articleIds.Contains(x.ID));

            var qryArticleTag = from sqKbTag in dbContext.Set<KBTag>().AsNoTracking()
                                join sqKbRef in dbContext.Set<KBArticleReference>().AsNoTracking()
                                on sqKbTag.Id equals sqKbRef.ObjectId
                                where sqKbRef.ObjectClassID == ObjectClass.KBArticleTag
                                select new { articleId = sqKbRef.ArticleId, tagName = sqKbTag.Name };

            if (serviceId.HasValue)
            {
                var tagSubQuery = dbContext.Set<ServiceTag>().AsNoTracking()
                                    .Where(x => x.ObjectId == serviceId || x.ObjectId == serviceItemAttendanceID);

                query = from kbArticle in query

                        let checkTag = (from srvTag in tagSubQuery
                                       where !qryArticleTag.Any(x => x.articleId == kbArticle.ID && EF.Functions.Like(x.tagName, '%' + srvTag.Tag))
                                       select new { value = 1 }).ToArray()

                        where !checkTag.Any()
                        select kbArticle;
            }
            return (await
                    (from kbArticle in query
                     join sqTags in qryArticleTag
                     on kbArticle.ID equals sqTags.articleId
                     into jQry
                     from jTag in jQry.DefaultIfEmpty()
                     orderby kbArticle.UtcDateModified descending
                     select new
                     {
                         kbArticle.ID,
                         kbArticle.Name,
                         kbArticle.Description,
                         kbArticle.HTMLDescription,
                         kbArticle.HTMLSolution,
                         kbArticle.UtcDateModified,
                         jTag.tagName
                     })
                    .ToArrayAsync(cancellationToken)) // Без предварительной выборки EF падает и жалуется, что не поддерживает такое.
                    .GroupBy(x => new { x.ID, x.Name, x.Description, x.HTMLDescription, x.HTMLSolution, x.UtcDateModified },
                             x => x.tagName,
                             (article, tags) => new KBArticleInfoItem()
                             {
                                 ID = article.ID,
                                 Name = article.Name,
                                 Description = article.Description,
                                 HTMLDescription = article.HTMLDescription,
                                 HTMLSolution = article.HTMLSolution,
                                 UtcDateModified = article.UtcDateModified,
                                 Tags = string.Join(", ", tags)
                             })
                    .ToArray();
        }

        public async Task<KBArticleShortItem[]> GetArticlesByIdsAsync(Guid[] ids, Guid userId, CancellationToken cancellationToken)
        {
            var query = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .AsQueryable();

            var folders = await GetAllFoldersAsync(cancellationToken);

            query = await SetAccessFilterAsync(query, userId, cancellationToken);
            query = query.Where(x => ids.Contains(x.ID))
                         .OrderBy(x => x.Solution.Substring(0, 50));
            var items = await query
                .Select(x => new
                {
                    x.ID,
                    x.Name,
                    x.Number,
                    x.Description,
                    x.Solution,
                    x.UtcDateCreated,
                    x.UtcDateModified,
                    x.Visible,
                    DocumentsCount = dbContext.Set<DocumentReference>().Count(y => y.ObjectID == x.ID),
                    AuthorFullName = dbContext.Set<User>()
                                        .AsNoTracking()
                                        .Where(y => y.IMObjID == x.AuthorID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName)
                                        .First(),
                    FolderId = dbContext.Set<KBArticleReference>().AsNoTracking()
                                    .Where(w => w.ArticleId == x.ID && w.ObjectClassID == ObjectClass.KBArticleFolder)
                                    .Select(t => t.ObjectId)
                                    .FirstOrDefault()
                })
                .ToArrayAsync(cancellationToken);

            return items.Select(x => new KBArticleShortItem()
            {
                ID = x.ID,
                Name = x.Name,
                Number = x.Number,
                Description = x.Description,
                Solution = x.Solution,
                UtcDateCreated = x.UtcDateCreated,
                UtcDateModified = x.UtcDateModified,
                Visible = x.Visible,
                DocumentsCount = x.DocumentsCount,
                AuthorFullName = x.AuthorFullName,
                Section = folders.FirstOrDefault(k => k.ID == x.FolderId)?.FullName
            })
            .ToArray();
        }

        public async Task<KBArticleShortItem[]> GetArticlesAsync(Guid[] folderIds, Guid userId, CancellationToken cancellationToken)
        {
            var query = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .AsQueryable();

            query = await SetAccessFilterAsync(query, userId, cancellationToken);
            query = query.Where(x => dbContext.Set<KBArticleReference>().Any(y => folderIds.Contains(y.ObjectId) && y.ArticleId == x.ID));
            var items = await query
                .Select(x => new KBArticleShortItem()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Number = x.Number,
                    Description = x.Description,
                    Solution = x.Solution,
                    UtcDateCreated = x.UtcDateCreated,
                    UtcDateModified = x.UtcDateModified,
                    Visible = x.Visible,
                    DocumentsCount = dbContext.Set<DocumentReference>().Count(y => y.ObjectID == x.ID),
                    AuthorFullName = dbContext.Set<User>()
                                        .AsNoTracking()
                                        .Where(y => y.IMObjID == x.AuthorID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName)
                                        .First()
                })
                .ToArrayAsync(cancellationToken);
            return items.GroupBy(x => x.ID)
                        .Select(x => x.First())
                        .ToArray();
        }

        public async Task<KBArticleShortItem[]> GetObjectArticlesAsync(Guid objectID, ObjectClass objectClassID, bool seeInvisible, CancellationToken cancellationToken)
        {
            var qrArticles = dbContext.Set<KBArticle>()
                                .AsNoTracking()
                                .AsQueryable();
            var qrReferences = dbContext.Set<KBArticleReference>()
                                    .AsNoTracking()
                                    .Where(x => x.ObjectClassID == objectClassID && x.ObjectId == objectID);
            var query = from articles in qrArticles
                        join refs in qrReferences
                        on articles.ID equals refs.ArticleId
                        where articles.Visible == seeInvisible
                        select new KBArticleShortItem()
                        {
                            ID = articles.ID,
                            Name = articles.Name,
                            Number = articles.Number,
                            Description = articles.Description,
                            Solution = articles.Solution,
                            UtcDateCreated = articles.UtcDateCreated,
                            UtcDateModified = articles.UtcDateModified,
                            Visible = articles.Visible,
                            DocumentsCount = dbContext.Set<DocumentReference>()
                                                      .Count(y => y.ObjectID == articles.ID),
                            AuthorFullName = dbContext.Set<User>().Where(y => y.IMObjID == articles.AuthorID)
                                                      .Select(y => y.Removed ? DeletedEntryDescription : y.FullName).First()
                        };
            return await query.ToArrayAsync(cancellationToken);
        }

        public async Task<KBArticleShortItem[]> GetArticlesAsync(Guid folderId, Guid userId, CancellationToken cancellationToken)
        {

            var query = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .AsQueryable();

            query = await SetAccessFilterAsync(query, userId, cancellationToken);
            if (folderId == Guid.Empty)
                query = query.Where(x => !dbContext.Set<KBArticleReference>()
                                            .Any(y => y.ArticleId == x.ID && y.ObjectClassID == ObjectClass.KBArticleFolder && y.ObjectId != Guid.Empty)
                                         || dbContext.Set<KBArticleReference>().Any(y => y.ObjectId == Guid.Empty && y.ArticleId == x.ID));
            else
                query = query.Where(x => dbContext.Set<KBArticleReference>().Any(y => y.ObjectId == folderId && y.ArticleId == x.ID));


            return await query
                .Select(x => new KBArticleShortItem()
                {
                    ID = x.ID,
                    Name = x.Name,
                    Number = x.Number,
                    Description = x.Description,
                    Solution = x.Solution,
                    UtcDateCreated = x.UtcDateCreated,
                    UtcDateModified = x.UtcDateModified,
                    Visible = x.Visible,
                    DocumentsCount = dbContext.Set<DocumentReference>().Count(y => y.ObjectID == x.ID),
                    AuthorFullName = dbContext.Set<User>().Where(y => y.IMObjID == x.AuthorID)
                                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName).First()
                })
                .ToArrayAsync(cancellationToken);
        }

        public async Task<KBArticleFolderItem[]> GetAllFoldersAsync(CancellationToken cancellationToken)
        {
            var items = await dbContext
                   .Set<KBArticleFolder>()
                   .AsNoTracking()
                   .Select(x => new KBArticleFolderItem()
                   {
                       ID = x.ID,
                       Name = x.Name,
                       Note = x.Note,
                       Visible = x.Visible,
                       HasChilds = dbContext.Set<KBArticleFolder>().Any(y => y.ParentID == x.ID),
                       ParentId = x.ParentID,
                       FullName = x.Name
                   })
                   .ToListAsync(cancellationToken);
            items.Where(x => x.ParentId.HasValue)
                 .ForEach(x => CreateFullName(x, items));

            items.Add(new KBArticleFolderItem
            {
                ID = Guid.Empty,
                FullName = "Без категории",
                Name = "Без категории",
                Note = "",
                Visible = true,
                HasChilds = false
            });

            return items.ToArray();
        }

        public async Task<KBArticleShortItem> GetArticleByNumberAsync(int number, CancellationToken cancellationToken)
        {
            var query = dbContext.Set<KBArticle>()
                .AsNoTracking()
                .Where(x => x.Visible && x.Number == number);

            return await query
                .Select(x => new KBArticleShortItem
                {
                    ID = x.ID,
                    Name = x.Name,
                    Number = x.Number,
                    Description = x.Description,
                    Solution = x.Solution,
                    UtcDateCreated = x.UtcDateCreated,
                    UtcDateModified = x.UtcDateModified,
                    Visible = x.Visible,
                    DocumentsCount = dbContext.Set<DocumentReference>().Count(y => y.ObjectID == x.ID),
                    AuthorFullName = dbContext.Set<User>().Where(y => y.IMObjID == x.AuthorID)
                        .Select(y => y.Removed ? DeletedEntryDescription : y.FullName).First()
                }).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Guid[]> CheckArticlesAccessAsync(Guid[] articleIDs, Guid userID, CancellationToken cancellationToken)
        {
            var query = dbContext.Set<KBArticle>()
                .AsQueryable()
                .Where(w => articleIDs.Contains(w.ID));

            query = await SetAccessFilterAsync(query, userID, cancellationToken);

            return await query.Select(s => s.ID)
                .ToArrayAsync(cancellationToken);
        }

        private static void CreateFullName(KBArticleFolderItem item, List<KBArticleFolderItem> items)
        {
            var textList = new List<string>() { item.FullName };
            var parentItem = items.First(x => x.ID == item.ParentId.Value);
            do
            {
                textList.Add(parentItem.Name);
                parentItem = parentItem.ParentId.HasValue ? items.First(x => x.ID == parentItem.ParentId.Value) : null;
            } while (parentItem != null);
            item.FullName = string.Join(" \\ ", textList.Reverse<string>());
        }

        public async Task<KBArticleFolderItem[]> GetFoldersAsync(Guid? parentId, bool visible, CancellationToken cancellationToken)
        {
            var items = await dbContext
                    .Set<KBArticleFolder>()
                    .AsNoTracking()
                    .Where(x => x.ParentID == parentId && x.Visible == visible)
                    .Select(x => Tuple.Create(new KBArticleFolderItem()
                    {
                        ID = x.ID,
                        Name = x.Name,
                        Note = x.Note,
                        Visible = x.Visible,
                        HasChilds = dbContext.Set<KBArticleFolder>().Any(y => y.ParentID == x.ID)
                    }, x.ParentID))
                    .ToArrayAsync(cancellationToken);
            return items.Select(x => x.Item1).ToArray();
        }

        private async Task<bool> CheckItUserAsync(User user, CancellationToken cancellationToken)
        {
            if (user == null)
            {
                return false;
            }
            var hasRoles = await dbContext.Set<UserRole>()
                .AnyAsync(a => a.UserID == user.IMObjID, cancellationToken);

            if (hasRoles)
                return true;

            var allSubdivisions = await dbContext.Set<Subdivision>()
                .ToArrayAsync(cancellationToken);

            return user.Subdivision?.IsInItSubdivision(allSubdivisions) ?? false;
        }
    }
}
