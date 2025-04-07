using InfraManager.BLL;
using InfraManager.BLL.AccessManagement;
using InfraManager.BLL.KB;
using InfraManager.BLL.ServiceDesk;
using InfraManager.IM.BusinessLayer.SearchService;
using InfraManager.Services.SearchService;
using InfraManager.Web.BLL.SD.KB;
using InfraManager.Web.Controllers;
using InfraManager.Web.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.Web.Helpers;
using static InfraManager.Web.Controllers.SD.SDApiController;
using InfraManager.BLL.KnowledgeBase;
using InfraManager;
using AutoMapper;

namespace InfraManager.UI.Web.Controllers.Api.ServiceDesk.KnowledgeBase
{
    [Route("api/[controller]")] //TODO: Весь функционал базы знаний перепроектировать и переписать
    [ApiController]
    [Authorize]
    public class KBController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly IObjectAccessBLL _accessManager;
        private readonly IKnowledgeBaseBLL _knowledgeBaseBLL;
        private readonly ISupportSettingsBll _supportSettingsBll;
        private readonly IKnowledgeBaseAccessBLL _knowledgeBaseAccessBLL;
        private readonly IHubContext<EventHub> _hub;

        public KBController(
                    IMapper mapper,
                    ICurrentUser currentUser,
                    IObjectAccessBLL accessManager,
                    IKnowledgeBaseBLL knowledgeBaseBLL,
                    ISupportSettingsBll supportSettingsBll,
                    IKnowledgeBaseAccessBLL knowledgeBaseAccessBLL,
                    IHubContext<EventHub> hub)
        {
            _hub = hub;
            _mapper = mapper;
            _currentUser = currentUser;
            _knowledgeBaseBLL = knowledgeBaseBLL;
            _supportSettingsBll = supportSettingsBll;
            _knowledgeBaseAccessBLL = knowledgeBaseAccessBLL;
            _accessManager = accessManager;
        }

        [HttpGet("Refs")]
        public async Task<KBArticleShortDetails[]> GetRefs(
                                    [FromQuery] Guid objectID,
                                    [FromQuery] ObjectClass objectClassID,
                                    [FromQuery] bool seeInvisible, // зашибись параметр, а можно мне в базе пентагона эндпоинт с таким же параметром, я туда true зашлю и стану очень богат ... посмертно
                                    CancellationToken cancellationToken)
        {
            var articles = await _knowledgeBaseBLL.GetObjectArticlesAsync(objectID, objectClassID, seeInvisible, cancellationToken);
            return articles;
        }

        [HttpPost("Refs")]
        public async Task<RequestResponceType> RefEditAsync(EditKBReferenceIncomingModel model, CancellationToken cancellationToken)
        {
            var supportedClasses = new[] { (int)ObjectClass.Call, (int)ObjectClass.Problem, (int)ObjectClass.WorkOrder, (int)ObjectClass.MassIncident };
            try
            {
                if (supportedClasses.Contains(model.ObjectClassID))
                {
                    var objectClass = (ObjectClass)model.ObjectClassID;
                    if (!await _accessManager.AccessIsGrantedAsync(_currentUser.UserId, model.ObjectID, objectClass))
                    {
                        // TODO: logging
                        // Logger.Error("SDApiControllerKB.EditKBReference userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                        return RequestResponceType.AccessError;
                    }
                }
                else
                    throw new NotSupportedException("ObjectClassID not valid");


                await _knowledgeBaseBLL.EditReferenceAsync(model.KBArticleID, model.ObjectID, (ObjectClass)model.ObjectClassID,
                                                           model.Operation == InfraManager.Web.BLL.Fields.FieldOperation.Create,
                                                           cancellationToken);

                // Информируем websocket клиентов
                EventHub.ObjectUpdated(
                    _hub,
                    model.ObjectClassID,
                    model.ObjectID,
                    null,
                    HttpContext.GetRequestConnectionID());

                return RequestResponceType.Success;
            }
            catch (NotSupportedException)
            {
                // TODO: On add logging
                // Logger.Error(e, String.Format(@"EditKBReference not supported, model: '{0}'", model));
                return RequestResponceType.BadParamsError;
            }
            catch (Exception)
            {
                // TODO: On add logging
                // Logger.Error(e, String.Format(@"EditKBReference, model: '{0}'", model));
                return RequestResponceType.GlobalError;
            }
        }

        [HttpGet("Search")]
        public async Task<KBArticleShortDetails[]> SearchAsync([FromQuery] string text, [FromQuery] bool seeInvisible, CancellationToken cancellationToken = default)
        {
            if (!await _knowledgeBaseBLL.CheckSearchAccessAsync(seeInvisible, cancellationToken))
                throw new AccessDeniedException("Don't have search access");

            var foundArticleIds = SearchBySearchManager(text);

            return foundArticleIds.Length > 0 ?
                    await _knowledgeBaseBLL.GetArticlesByIdsAsync(foundArticleIds, cancellationToken) :
                    Array.Empty<KBArticleShortDetails>();
        }

        [HttpGet("SearchInfo")]
        public async Task<KBArticleInfoDetails[]> SearchInfoAsync(
                            [FromQuery] string text,
                            [FromQuery] bool clientRegistration,
                            [FromQuery] Guid? serviceItemAttendanceID,
                            CancellationToken cancellationToken)
         {
            if (!UseAtCallRegistration())
            {
                // TODO: Logging
                // Logger.Trace("SDApiController.SearchKBArticleInfoList userID={0}, userName={1}, text='{2}', serviceItemAttendanceID='{3}' canceled by settings", user.Id, user.UserName, text, serviceItemAttendanceID.HasValue ? serviceItemAttendanceID.Value.ToString() : "<null>");
                return null;
            }

            text = text ?? string.Empty;
            var foundArticleIds = SearchBySearchManager(text);

            return (foundArticleIds?.Length ?? 0) > 0 ?
                      await _knowledgeBaseBLL.GetArticleInfoByIdsAsync(foundArticleIds, serviceItemAttendanceID, cancellationToken) :
                      Array.Empty<KBArticleInfoDetails>();
        }

        private bool UseAtCallRegistration()
        {
            var value = _supportSettingsBll.GetSetting<bool>(SystemSettings.SearchKBDuringRegisteringNewCall);
            return value;
        }

        private static bool CheckSearchManager()
        {
            if (!SearchManager.IsConnected)
                SearchManager.Connect();
            if (!SearchManager.IsConnected)
                return false;
            return true;
        }

        [HttpGet("Folders")]
        public async Task<KBArticleFolderDetails[]> GetFoldersAsync([FromQuery] FolderFilter folderFilter, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetAllFoldersAsync(folderFilter.ParentId, folderFilter.SeeInvisible, cancellationToken);
        }

        [HttpPost("Folders")] // sdApi/addChildKBFolder
        public async Task<IActionResult> AddFolderAsync(AddKBFolderModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var folderModel = new KBArticleFolderDetails()
                {
                    Name = model.Name,
                    Note = model.Note,
                    ParentID = model.ParentID
                };
                var kbaFolder = await _knowledgeBaseBLL.AddFolderAsync(folderModel, model.SeeInvisible, cancellationToken);

                return Ok(new { success = true, section = kbaFolder });

            }
            catch (AccessDeniedException)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("Folder/{folderId}")]// /sdApi/editKBFolder
        public async Task<IActionResult> EditFolderAsync(Guid folderId, AddKBFolderModel model, CancellationToken cancellationToken = default)
        {
            try
            {
                var folderModel = new KBArticleFolderDetails()
                {
                    Name = model.Name,
                    Note = model.Note
                };
                await _knowledgeBaseBLL.EditFolderAsync(folderId, folderModel, model.SeeInvisible, cancellationToken);

                return Ok(new { success = true });
            }
            catch (AccessDeniedException)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Удаление папок базы знаний
        /// </summary>
        /// <param name="ID">ID папки</param>
        /// <param name="seeInvisible">видимость папки</param>
        /// <returns></returns>
        [HttpDelete("DeleteKBFolder")]
        public async Task<ResultWithMessage> DeleteKBFolder(Guid ID, bool seeInvisible)
        {
            await _knowledgeBaseBLL.DeleteFolderAsync(ID, seeInvisible);
            return ResultWithMessage.Create(RequestResponceType.Success);
        }

        [HttpGet("FolderHierarchy")] // /sdApi/getKBAFolderHierarchy
        public async Task<KBArticleFolderDetails[]> GetFolderHierarchyAsync([FromQuery] FolderFilter folderFilter, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetFolderHierarchyAsync(folderFilter.ParentId, folderFilter.SeeInvisible, cancellationToken);
        }

        [HttpGet("Articles")] // sdApi/getKBAList
        public async Task<KBArticleShortDetails[]> GetArticlesAsync([FromQuery] Guid folderID, [FromQuery] bool seeInvisible, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticlesAsync(folderID, seeInvisible, cancellationToken);
        }

        [HttpPost("Articles")] // sdApi/AddKbArticle
        public async Task<bool> AddArticleAsync(KbaInfo model, CancellationToken cancellationToken = default)
        {
            var editArticle = MapKBArticleModel(model);
            var detailArticle = await _knowledgeBaseBLL.AddArticleAsync(editArticle, model.FolderID, model.TMPID, cancellationToken);
            AddDocumentSearchServiceIndex(detailArticle.ID, detailArticle);
            return true;
        }

        [HttpPut("Article")] // "sdApi/EditKbArticle"
        public async Task<KBArticleDetails> EditArticleAsync(KbaInfo model, CancellationToken cancellationToken = default)
        {
            var editArticle = MapKBArticleModel(model);
            var detailArticle = await _knowledgeBaseBLL.EditArticleAsync(model.ID, editArticle, cancellationToken);
            UpdateDocumentSearchServiceIndex(detailArticle.ID, detailArticle);
            return detailArticle;
        }

        private static Document DocumentFromArticle(KBArticleDetails article)
        {
            var document = new Document();
            var docFields = document.Fields;

            if ((article.Tags?.Length ?? 0) != 0)
            {
                var tags = new System.Text.StringBuilder();
                foreach (var tag in article.Tags)
                    tags.AppendFormat("{0} ", tag);
                docFields.Add(new Field(SearchHelper.LuceneKbaTag, tags.ToString(), true));
            }

            docFields.Add(new Field(SearchHelper.LuceneID, article.ID.ToString(), false));
            docFields.Add(new Field(SearchHelper.LuceneClassID, IMSystem.Global.OBJ_KBArticle.ToString(), true));
            docFields.Add(new Field(SearchHelper.LuceneName, article.Name.ToString(), true));
            docFields.Add(new Field(SearchHelper.LuceneDescription, article.Description, true));
            docFields.Add(new Field(SearchHelper.LuceneSolution, article.Solution, true));
            docFields.Add(new Field(SearchHelper.LuceneAlternativeSolution, article.AlternativeSolution, true));
            docFields.Add(new Field(SearchHelper.LuceneKbaAuthor, article.AuthorFullName, true));
            docFields.Add(new Field(SearchHelper.LuceneKbaAuthorID, article.AuthorID.ToString(), true));
            docFields.Add(new Field(SearchHelper.LuceneKbaModifier, article.ModifierFullName, true));
            docFields.Add(new Field(SearchHelper.LuceneKbaModifierID, article.ModifierID.ToString(), true));
            docFields.Add(new Field(SearchHelper.LuceneProcessed, bool.TrueString, false));
            docFields.Add(new Field(SearchHelper.LuceneNumber, article.Number.ToString(), false));

            return document;
        }

        private static void AddDocumentSearchServiceIndex(Guid id, KBArticleDetails article)
        {
            var document = DocumentFromArticle(article);
            SearchManager.InsertDocument(id, document);
        }

        private static void UpdateDocumentSearchServiceIndex(Guid id, KBArticleDetails article)
        {
            var document = DocumentFromArticle(article);
            SearchManager.UpdateDocument(id, document);
        }

        private KBArticleEditData MapKBArticleModel(KbaInfo model)
        {
            var article = _mapper.Map<KBArticleEditData>(model);
            article.Description = Core.Html.HtmlParser.ToText(article.HTMLDescription);
            article.Solution = Core.Html.HtmlParser.ToText(article.HTMLSolution);
            article.AlternativeSolution = Core.Html.HtmlParser.ToText(article.HTMLAlternativeSolution);

            var tagArray = !string.IsNullOrEmpty(model.TagString) ?
                                model.TagString
                                     .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Select(x => x.Trim())
                                     .Where(x => !string.IsNullOrEmpty(x)) :
                                new List<string>();
            var tags = new List<string>();
            foreach (var tag in tagArray)
            {
                if (!tags.Any(x => string.Compare(x, tag, StringComparison.OrdinalIgnoreCase) == 0))
                    tags.Add(tag);
            }
            article.Tags = tags.ToArray();
            article.KBArticleDependencyList = model.KBArticleDependencyList;

            return article;
        }

        [HttpGet("Article")]
        public async Task<KBArticleDetails> GetArticleAsync([FromQuery] Guid kbaId, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticleAsync(kbaId, cancellationToken);
        }

        [HttpGet("ArticleID")]
        public async Task<Guid> GetArticleIDByNumberAsync([FromQuery] int number, CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticleIDByNumberAsync(number, cancellationToken);
        }

        [HttpGet("ArticleTypes")]
        public async Task<KBArticleTypeDetails[]> GetArticleTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticleTypesAsync(cancellationToken);
        }

        [HttpGet("ArticleStatuses")]
        public async Task<KBArticleStatusDetails[]> GetArticleStatusesAsync(CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticleStatusesAsync(cancellationToken);
        }

        [HttpGet("ArticleAccess")]
        public async Task<KBArticleAccessModel[]> GetArticleAccessAsync(CancellationToken cancellationToken = default)
        {
            return await _knowledgeBaseBLL.GetArticleAccessAsync(cancellationToken);
        }

        private static Guid[] SearchBySearchManager(string text)
        {
            if (!CheckSearchManager())
                return Array.Empty<Guid>();

            text = System.Web.HttpUtility.UrlDecode(text);

            var queryList = new List<QueryItem>() {
                new QueryItem(text, SearchHelper.FieldNamesContextSearch)
            };
            var tagList = text
                            .Split(' ')
                            .Where(x => x.Length != 0 && x[0] == '#')
                            .Select(x => x.Remove(0, 1))
                            .ToList();
            var classList = new List<int>() {
                (int)ObjectClass.KBArticle
            };
            var foundArticles = SearchManager.Search(queryList, tagList, classList, false, SearchHelper.SearchMode.Context);
            return foundArticles?.Select(x => x.ID).ToArray() ?? Array.Empty<Guid>();
        }
    }
}
