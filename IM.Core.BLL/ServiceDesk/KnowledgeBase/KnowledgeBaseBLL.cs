using AutoMapper;
using Inframanager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.KnowledgeBase;
using InfraManager.DAL;
using InfraManager.DAL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ServiceDesk
{
    // TODO: Переписать
    internal class KnowledgeBaseBLL : IKnowledgeBaseBLL, ISelfRegisteredService<IKnowledgeBaseBLL> 
    {
        private readonly IMapper _mapper;
        private readonly IKnowledgeBaseQuery _knowledgeBaseQuery;
        private readonly IUserAccessBLL _userAccess;
        private readonly ICurrentUser _currentUser;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<KBArticleFolder> _folderRepository;
        private readonly IRepository<KBArticleAccessList> _accessListRepository;
        private readonly IRepository<KBArticle> _articleRepository;
        private readonly IRepository<KBTag> _tagsRepository;
        private readonly IUnitOfWork _saveChanges;
        private readonly IRepository<KBArticleReference> _articleRefRepository;
        private readonly IKBArticleViewCountUpdateCommand _articleViewCountUpdateCommand;
        private readonly IValidateObjectPermissions<Guid, KBArticle> _validateKbArticle;
        private readonly IKnowledgeBaseAccessBLL _knowledgeBaseAccessBLL;
        private readonly ILoadEntity<Guid, KBArticle, KBArticleDetails> _loader;

        public KnowledgeBaseBLL(
                    IMapper mapper,
                    ICurrentUser currentUser,
                    IUserAccessBLL userAccess,
                    IUnitOfWork saveChanges,
                    IRepository<User> userRepository,
                    IRepository<KBTag> tagsRepository,
                    IKnowledgeBaseQuery knowledgeBaseQuery,
                    IRepository<KBArticleFolder> folderRepository,
                    IRepository<KBArticleAccessList> accessListRepository,
                    IRepository<KBArticleReference> articleRefRepository,
                    IRepository<KBArticle> articleRepository,
                    IValidateObjectPermissions<Guid, KBArticle> validateKbArticle,
                    IKBArticleViewCountUpdateCommand articleViewCountUpdateCommand,
                    IKnowledgeBaseAccessBLL knowledgeBaseAccessBLL,
                    ILoadEntity<Guid, KBArticle, KBArticleDetails> loader)
        {
            _mapper = mapper;
            _knowledgeBaseQuery = knowledgeBaseQuery;
            _userAccess = userAccess;
            _currentUser = currentUser;
            _articleRepository = articleRepository;
            _saveChanges = saveChanges;
            _tagsRepository = tagsRepository;
            _articleRefRepository = articleRefRepository;
            _folderRepository = folderRepository;
            _accessListRepository = accessListRepository;
            _userRepository = userRepository;
            _validateKbArticle = validateKbArticle;
            _articleViewCountUpdateCommand = articleViewCountUpdateCommand;
            _knowledgeBaseAccessBLL = knowledgeBaseAccessBLL;
            _loader = loader;
    }        

        public async Task<bool> CheckSearchAccessAsync(bool seeInvisible, CancellationToken cancellationToken)
        {
            if (seeInvisible && !(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return false;
            }
            return true;
        }

        public async Task<KBArticleInfoDetails[]> GetArticleInfoByIdsAsync(Guid[] foundArticleIds, Guid? serviceItemAttendanceID, CancellationToken cancellationToken)
        {
            var articles = await _knowledgeBaseQuery.GetArticleInfoByIdsAsync(foundArticleIds, serviceItemAttendanceID, cancellationToken);
            return _mapper.Map<KBArticleInfoDetails[]>(articles);
        }

        public async Task<KBArticleShortDetails[]> GetObjectArticlesAsync(Guid objectID, ObjectClass objectClassID, bool seeInvisible, CancellationToken cancellationToken)
        {
            var items = await _knowledgeBaseQuery.GetObjectArticlesAsync(objectID, objectClassID, seeInvisible, cancellationToken);
            return _mapper.Map<KBArticleShortDetails[]>(items);
        }

        public async Task<KBArticleDetails> GetArticleAsync(Guid articleID, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            if (!await _validateKbArticle.ObjectIsAvailableAsync(userId, articleID, cancellationToken))
                throw new AccessDeniedException("Access denied");
            var item = await _knowledgeBaseQuery.GetArticleAsync(articleID, userId, cancellationToken);
            if (item != null)
            {
                await _articleViewCountUpdateCommand.ExecuteAsync(articleID, cancellationToken);
                item.ViewsCount++;
            }

            if (item != null)
            {
                var article = _mapper.Map<KBArticleDetails>(item);

                article.KBArticleDependencyList = await GetArticleDependciesAsync(articleID, cancellationToken);
                
                return article;
            }
            else
                return null;
        }

        private async Task<KBArticleDependency[]> GetArticleDependciesAsync(Guid articleID, CancellationToken cancellationToken)
        {
            var dependcies = await _knowledgeBaseQuery.GetObjectArticlesAsync(articleID, ObjectClass.KBArticle, true, cancellationToken);

            return _mapper.Map<KBArticleDependency[]>(dependcies);
        }

        public async Task<Guid> GetArticleIDByNumberAsync(int number, CancellationToken cancellationToken)
        {
            var userID = _currentUser.UserId;

            var article = await _knowledgeBaseQuery.GetArticleByNumberAsync(number, cancellationToken);

            if (article == null)
                throw new ObjectNotFoundException($"Can't find article by {number}");

            if (!await _validateKbArticle.ObjectIsAvailableAsync(userID, article.ID, cancellationToken))
                throw new AccessDeniedException("Access denied");

            return article.ID;
        }

        public async Task<KBArticleShortDetails[]> GetArticlesByIdsAsync(Guid[] ids, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;
            var items = await _knowledgeBaseQuery.GetArticlesByIdsAsync(ids, userId, cancellationToken);
            return _mapper.Map<KBArticleShortDetails[]>(items);
        }

        public async Task<KBArticleShortDetails[]> GetArticlesAsync(Guid folderId, bool seeInvisible, CancellationToken cancellationToken)
        {
            if (seeInvisible && !(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return null;
            }
            var userId = _currentUser.UserId;
            var folderItems = await _knowledgeBaseQuery.GetAllFoldersAsync(cancellationToken);
            var kbaFolderHierarchy = GetFolderHierarchy(folderItems, folderId);
            if (kbaFolderHierarchy != null && folderId != Guid.Empty)
            {
                var folderHierarchy = kbaFolderHierarchy as KBArticleFolderItem[] ?? kbaFolderHierarchy.ToArray();

                if (folderHierarchy.Length != 0)
                {
                    var foldersId = folderHierarchy.Select(p => p.ID).ToList();
                    foldersId.Add(folderId);

                    var items = await _knowledgeBaseQuery.GetArticlesAsync(foldersId.ToArray(), userId, cancellationToken);
                    return _mapper.Map<KBArticleShortDetails[]>(items);
                }
            }
            var articles = await _knowledgeBaseQuery.GetArticlesAsync(folderId, userId, cancellationToken);
            return _mapper.Map<KBArticleShortDetails[]>(articles);
        }

        public async Task<KBArticleFolderDetails[]> GetFolderHierarchyAsync(Guid? parentId, bool visible, CancellationToken cancellationToken)
        {
            if (visible && !(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return null;
            }
            var folderItems = await _knowledgeBaseQuery.GetAllFoldersAsync(cancellationToken);
            var kbaFolderHierarchy = GetFolderHierarchy(folderItems, parentId);
            if (kbaFolderHierarchy != null)
            {
                var folderHierarchy = kbaFolderHierarchy as KBArticleFolderItem[] ?? kbaFolderHierarchy.ToArray();

                if (!visible)
                {
                    folderHierarchy = folderHierarchy.Where(f => f.Visible).ToArray();
                }

                return folderHierarchy.Select(x => new KBArticleFolderDetails
                {
                    ID = x.ID,
                    Name = x.Name,
                    FullName = x.FullName,
                    Note = x.Note,
                    Visible = x.Visible,
                    HasChilds = x.HasChilds,
                    ParentID = x.ParentId
                }).ToArray();
            }
            return null;
        }

        public async Task<KBArticleStatusDetails[]> GetArticleStatusesAsync(CancellationToken cancellationToken)
        {
            if (!(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return null;
            }
            var statuses = await _knowledgeBaseQuery.GetArticleStatusesAsync(cancellationToken);
            return statuses.Select(x => new KBArticleStatusDetails
            {
                ID = x.ID,
                Name = x.Name
            })
            .ToArray();
        }

        public async Task<KBArticleAccessModel[]> GetArticleAccessAsync(CancellationToken cancellationToken)
        {
            if (!(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return null;
            }
            var accesses = await _knowledgeBaseQuery.GetArticleAccessAsync(cancellationToken);
            return accesses.Select(x => new KBArticleAccessModel
            {
                ID = x.ID,
                Name = x.Name
            })
            .ToArray();
        }

        public async Task<KBArticleTypeDetails[]> GetArticleTypesAsync(CancellationToken cancellationToken)
        {
            if (!(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
            {
                return null;
            }
            var types = await _knowledgeBaseQuery.GetArticleTypesAsync(cancellationToken);
            return types.Select(x => new KBArticleTypeDetails
            {
                ID = x.ID,
                Name = x.Name
            })
            .ToArray();
        }

        public async Task<KBArticleFolderDetails[]> GetAllFoldersAsync(Guid? parentId, bool visible, CancellationToken cancellationToken)
        {
            KBArticleFolderItem[] folderItems = null;
            var userID = _currentUser.UserId;
            if (!visible || !(await _userAccess.HasRolesAsync(userID, cancellationToken)))
            {
                folderItems = await _knowledgeBaseQuery.GetAccessFoldersAsync(userID, cancellationToken);
            } 
            else
            {
                folderItems = await _knowledgeBaseQuery.GetAllFoldersAsync(cancellationToken);
            }

            var folders = folderItems.Where(x =>(!x.ParentId.HasValue || x.ParentId == parentId))
                                     .Select(x => new KBArticleFolderDetails
                                     {
                                        ID = x.ID,
                                        Name = x.Name,
                                        FullName = x.FullName,
                                        Note = x.Note,
                                        Visible = x.Visible,
                                        HasChilds = x.HasChilds
                                     }).ToArray();
            return folders;
        }

        public async Task EditFolderAsync(Guid folderId, KBArticleFolderDetails folderModel, bool seeInvisible, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(folderModel.Name))
                throw new ArgumentException("Folder Name is wrong");

            bool grantedOperationUpdate =
                await _userAccess.UserHasOperationAsync(_currentUser.UserId, OperationID.KBArticleFolder_Update, cancellationToken);
            if (seeInvisible && !grantedOperationUpdate)
                throw new AccessDeniedException("Don't have rights");

            var folder = await _folderRepository.FirstOrDefaultAsync(x => x.ID == folderId, cancellationToken);
            if (folder == null)
                throw new ObjectNotFoundException($"Folder ('{folderId}') not found");

            folder.Name = folderModel.Name;
            folder.Note = folderModel.Note ?? string.Empty;

            await _saveChanges.SaveAsync(cancellationToken);
        }

        /// <summary>
        /// Каскадное удаление папок Базы знаний
        /// </summary>
        /// <param name="ID">идентификатор папки</param>
        /// <param name="seeInvisible">параметр видимости папки</param>
        /// <returns></returns>
        /// <exception cref="AccessDeniedException">исключение, при отсутствие прав пользователя на удаление</exception>
        /// <exception cref="ObjectNotFoundException">исключение, при отсутствии папки по ID</exception>
        public async Task DeleteFolderAsync(Guid ID, bool seeInvisible, CancellationToken cancellationToken)
        {
            bool grantedOperationDelete =
                await _userAccess.UserHasOperationAsync(_currentUser.UserId, OperationID.KBArticleFolder_Delete, cancellationToken);
            if (seeInvisible && !grantedOperationDelete)
                throw new AccessDeniedException("Don't have rights");

            // получение папки на удаление, верхнего уровня
            var folderMain = await _folderRepository.FirstOrDefaultAsync(x => x.ID == ID, cancellationToken);
            if (folderMain == null)
                throw new ObjectNotFoundException($"Folder ('{ID}') not found");

            // удаление папок
            Guid deleteGuid = Guid.Empty;
            while (ID != deleteGuid)
            {                
                deleteGuid = await RemoveChildFolderAsync(ID, cancellationToken);
            }            
        }

        /// <summary>
        /// Удаление папки, не имеющей дочерних папок
        /// </summary>
        /// <param name="ID">идентификатор папки</param>
        /// <param name="cancellationToken">токен прерывания</param>
        /// <returns>ID удаленной папки</returns>
        private async Task<Guid> RemoveChildFolderAsync(Guid ID, CancellationToken cancellationToken)
        {
            var guid = Guid.Empty;
            // получаем дочерние папки для текущей
            var childFolders = _folderRepository.Where(x => x.ParentID == ID).ToList();
            if (childFolders.Count > 0)
            {
                // есть дочерние - запускаем заново
                foreach (var folder in childFolders)
                {
                    await RemoveChildFolderAsync(folder.ID, cancellationToken);
                }
            }
            else
            {
                // дочерних нет - удаляем
                var childFolder = await _folderRepository.FirstOrDefaultAsync(x => x.ID == ID, cancellationToken);
                if(childFolder == null)
                    throw new ObjectNotFoundException($"Folder ('{ID}') not found");

                _folderRepository.Delete(childFolder);
                await _saveChanges.SaveAsync(cancellationToken);
                guid = childFolder.ID;
            }
            return guid;
        }

        public async Task<KBArticleDetails> EditArticleAsync(Guid articleId, KBArticleEditData model, CancellationToken cancellationToken)
        {
            var kbArticle = await _articleRepository.FirstOrDefaultAsync(x => x.ID == articleId, cancellationToken);
            if (kbArticle == null)
                throw new ObjectNotFoundException($"Article ('{articleId}') not found");

            _mapper.Map(model, kbArticle);
            kbArticle.ModifierID = _currentUser.UserId;
            kbArticle.UtcDateModified = DateTime.UtcNow;

            await _saveChanges.SaveAsync(cancellationToken);

            var savedTags = await UpdateArticleTagsAsync(articleId, model.Tags, cancellationToken);

            await UpdateArticleReferenciesAsync(articleId, model.KBArticleDependencyList, cancellationToken);

            var articleModel = _mapper.Map<KBArticle, KBArticleDetails>(kbArticle);
            articleModel.AuthorFullName = await GetUserFullNameAsync(kbArticle.AuthorID, cancellationToken);
            articleModel.ModifierFullName = await GetUserFullNameAsync(kbArticle.ModifierID, cancellationToken);
			articleModel.Tags = savedTags.Select(t => t.Name).ToArray();
            articleModel.KBArticleDependencyList = await GetArticleDependciesAsync(articleId, cancellationToken);
            
            return articleModel;
        }

        private async Task UpdateArticleReferenciesAsync(Guid articleID, Guid[] related, CancellationToken cancellationToken)
        {
            List<KBArticleShortItem> existing = (await _knowledgeBaseQuery.GetObjectArticlesAsync(articleID, ObjectClass.KBArticle, true, cancellationToken)).ToList();

            IEnumerable<Guid> inserting = related.Where(r => !existing.Exists(p => p.ID == r));
            foreach (Guid relatedID in inserting)
                await AttachReferenceAsync(relatedID, new InframanagerObject(articleID, ObjectClass.KBArticle), cancellationToken);

            IEnumerable<Guid> deleting = existing
                .Where(p => !related.Contains(p.ID))
                .Select(p => p.ID);

            foreach (Guid relatedID in deleting)
                await DetachReferenceAsync(relatedID, new InframanagerObject(articleID, ObjectClass.KBArticle), cancellationToken);

            await _saveChanges.SaveAsync(cancellationToken);
        }

        private Task<string> GetUserFullNameAsync(Guid userID, CancellationToken cancellationToken)
        {
            return _userRepository.Query().AsNoTracking()
                       .Where(x => x.IMObjID == userID)
                       .Select(x => (x.Removed ? "[УДАЛЕН] " : ((x.Surname + " " + x.Name).Trim() + " " + x.Patronymic).Trim()).Trim())
                       .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<KBArticleFolderDetails> AddFolderAsync(KBArticleFolderDetails folderModel, bool seeInvisible, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(folderModel.Name))
                throw new ArgumentException("Folder Name is wrong");
            if (!folderModel.ParentID.HasValue || folderModel.ParentID == Guid.Empty)
                throw new ArgumentException("Folder parent ID is wrong");
            if (seeInvisible && !(await _userAccess.HasRolesAsync(_currentUser.UserId, cancellationToken)))
                throw new AccessDeniedException("Don't have rights");

            var folder = new KBArticleFolder(folderModel.Name, folderModel.Note, folderModel.ParentID.Value, true);
            _folderRepository.Insert(folder);

            await _saveChanges.SaveAsync(cancellationToken);

            return new KBArticleFolderDetails
            {
                ID = folder.ID,
                Name = folder.Name,
                Note = folder.Note,
                Visible = folder.Visible,
            };
        }

        public async Task<KBArticleDetails> AddArticleAsync(KBArticleEditData article, Guid? folderId, Guid TmpID, CancellationToken cancellationToken)
        {
            KBArticle kbArticle;
            var savedTags = Enumerable.Empty<KBTag>();
            var accessListEntities = await _accessListRepository.ToArrayAsync(x => x.KbArticleID == TmpID, cancellationToken);
            var accessListModels = _mapper.Map<KBArticleAccessListModel[]>(accessListEntities);

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                kbArticle = _mapper.Map<KBArticleEditData, KBArticle>(article);
                kbArticle.AuthorID = _currentUser.UserId;
                kbArticle.ModifierID = _currentUser.UserId;
                kbArticle.UtcDateCreated = DateTime.UtcNow;
                kbArticle.UtcDateModified = DateTime.UtcNow;

                _articleRepository.Insert(kbArticle);

                await _saveChanges.SaveAsync(cancellationToken);

                foreach (var accessList in accessListModels)
                {
                    //Во время создания новой статьи kBArticleAccessList сохраняются привязанными к "временному" гуиду, после сохранения статьи нужно поменять "временный" гуид на актуальный
                    await _knowledgeBaseAccessBLL.DeleteAsync(accessList, cancellationToken);
                    accessList.KbArticleID = kbArticle.ID;
                    await _knowledgeBaseAccessBLL.AddAsync(accessList, cancellationToken);
                }
                await _saveChanges.SaveAsync(cancellationToken);

                if ((article.Tags?.Length ?? 0) > 0)
                    savedTags = await AddArticleTagsAsync(kbArticle.ID, article.Tags, cancellationToken);
                
                if (folderId.HasValue)
                {
                    var articleRef = new KBArticleReference
                    {
                        ArticleId = kbArticle.ID,
                        ObjectClassID = ObjectClass.KBArticleFolder,
                        ObjectId = folderId.Value
                    };
                    _articleRefRepository.Insert(articleRef);
                    await _saveChanges.SaveAsync(cancellationToken);
                }

                transaction.Complete();
            }
            var authorFullName = await GetUserFullNameAsync(kbArticle.AuthorID, cancellationToken);
            var articleModel = _mapper.Map<KBArticle, KBArticleDetails>(kbArticle);
            articleModel.AuthorFullName = authorFullName;
            articleModel.ModifierFullName = authorFullName;
            articleModel.Tags = savedTags.Select(t => t.Name).ToArray();
            return articleModel;
        }

        public async Task<IList<KBTag>> UpdateArticleTagsAsync(Guid articleId, string[] tags, CancellationToken cancellationToken)
        {
            var tegRefs = await _articleRefRepository
                                   .ToArrayAsync(x => x.ArticleId == articleId && x.ObjectClassID == ObjectClass.KBArticleTag,
                                                 cancellationToken);
            if (tegRefs.Length != 0)
            {
                tegRefs.ForEach(x => _articleRefRepository.Delete(x));
                await _saveChanges.SaveAsync(cancellationToken);
            }
            return await AddArticleTagsAsync(articleId, tags, cancellationToken);
        }

        public async Task<IList<KBTag>> AddArticleTagsAsync(Guid articleId, string[] tags, CancellationToken cancellationToken)
        {
            if (tags.Length == 0)
                return Array.Empty<KBTag>();

            var savedTagsArray = await _tagsRepository.ToArrayAsync(x => tags.Contains(x.Name), cancellationToken);
            var savedTags = new List<KBTag>(savedTagsArray);
            foreach (var tag in tags)
            {
                var saved = savedTags.FirstOrDefault(x => x.Name == tag);
                if (saved == null)
                {
                    var kbTag = new KBTag
                    {
                        Id = Guid.NewGuid(),
                        Name = tag
                    };
                    _tagsRepository.Insert(kbTag);
                    await _saveChanges.SaveAsync(cancellationToken);

                    savedTags.Add(kbTag);
                    saved = kbTag;
                }
                var articleRef = new KBArticleReference
                {
                    ArticleId = articleId,
                    ObjectClassID = ObjectClass.KBArticleTag,
                    ObjectId = saved.Id
                };
                _articleRefRepository.Insert(articleRef);
                await _saveChanges.SaveAsync(cancellationToken);
            }

            return savedTags;
        }

        private static IEnumerable<KBArticleFolderItem> GetFolderHierarchy(IEnumerable<KBArticleFolderItem> entities, Guid? parentNodeId = null)
        {
            if (entities != null)
            {
                var myEntities = entities as KBArticleFolderItem[] ?? entities.ToArray();
                var childNodes = myEntities.Where(x => x.ParentId == parentNodeId);
                foreach (var currentNode in childNodes)
                {
                    yield return currentNode;

                    // Get sub child
                    foreach (var trail in GetFolderHierarchy(myEntities, currentNode.ID))
                    {
                        yield return trail;
                    }
                }
            }
        }

        public async Task EditReferenceAsync(Guid articleID, Guid objectID, ObjectClass objectClass, bool creating, CancellationToken cancellationToken)
        {
            if (creating) //TODO: нужно вообще 1 метод на всю систему сделать, а внутри if (creating_call == true) { ... } else if (updating_user) { ... }
            {
                // Тут возможна проверка на дубликат только через unique индекс БД
                var articleRef = new KBArticleReference
                {
                    ArticleId = articleID,
                    ObjectClassID = objectClass,
                    ObjectId = objectID
                };
                _articleRefRepository.Insert(articleRef);
            }
            else
            {
                var articleRef = await _articleRefRepository.FirstOrDefaultAsync(
                    x => x.ArticleId == articleID
                        && x.ObjectId == objectID
                        && x.ObjectClassID == objectClass,
                   cancellationToken);
                if (articleRef != null)
                    _articleRefRepository.Delete(articleRef);
            }
            await _saveChanges.SaveAsync(cancellationToken);
        }

        public async Task AddReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default)
        {
            await AttachReferenceAsync(articleID, reference, cancellationToken);
            await _saveChanges.SaveAsync(cancellationToken);
        }

        public async Task AttachReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default)
        {
            var acticle = await _articleRepository.FirstOrDefaultAsync(x => x.ID == articleID, cancellationToken)
                ?? throw new ObjectNotFoundException<Guid>(articleID, ObjectClass.KBArticle);
            _articleRefRepository.Insert(                
                new KBArticleReference 
                { 
                    ArticleId = articleID,
                    ObjectId = reference.Id,
                    ObjectClassID = reference.ClassId
                });
        }

        public async Task RemoveReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default)
        {
            await DetachReferenceAsync(articleID, reference, cancellationToken);
            await _saveChanges.SaveAsync(cancellationToken);
        }

        public async Task DetachReferenceAsync(Guid articleID, InframanagerObject reference, CancellationToken cancellationToken = default)
        {
            var articleReference = await _articleRefRepository
                .FirstOrDefaultAsync(
                    x => x.ArticleId == articleID 
                        && x.ObjectId == reference.Id 
                        && x.ObjectClassID == reference.ClassId, 
                    cancellationToken) 
                ?? throw new ObjectNotFoundException($"Ссылка на статью базы знаний (ID = {articleID}) на объект {reference}.");
            _articleRefRepository.Delete(articleReference);
        }
    }
}
