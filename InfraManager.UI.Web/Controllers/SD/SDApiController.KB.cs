using InfraManager.Core.Data;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Logging;
using InfraManager.Web.BLL.SD.KB;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InfraManager.ResourcesArea;
using System.Threading.Tasks;

namespace InfraManager.Web.Controllers.SD
{
    partial class SDApiController
    {
        #region SendEmail
        [HttpPost]
        [Route(template: "sdApi/SendEmail", Name = "SendEmail")]
        public ResultWithMessage SendEmail([FromBody] SendEmailInfo model)
        {
            var user = base.CurrentUser;
            //
            if (model != null && user != null)
            {
                try
                {
                    List<object> documentList;
                    List<string> paths;
                    var api = new FileApiController(_environment);
                        if (!api.GetDocumentFromFiles(model.Files, out documentList, out paths, user))
                            //return new CallRegistrationResponse(RequestResponceType.BadParamsError, Resources.UploadedFileNotFoundAtServerSide, null);
                            return ResultWithMessage.Create(RequestResponceType.GlobalError, "error");
                    //
                    string error = null;
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        error = KBArticle.SendEmail(model, documentList, dataSource);
                    }
                    //
                    foreach (var filePath in paths)
                        System.IO.File.Delete(filePath);
                    //
                    return ResultWithMessage.Create(string.IsNullOrEmpty(error) ? RequestResponceType.Success : RequestResponceType.GlobalError, error);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return ResultWithMessage.Create(RequestResponceType.GlobalError, e.Message);
                }
            }
            //
            return ResultWithMessage.Create(RequestResponceType.GlobalError, "error");
        }
        #endregion

        #region CheckMailServiceConnection
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/CheckMailServiceConnection", Name = "CheckMailServiceConnection")]
        public MailServiceConnectionInfo CheckMailServiceConnection()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("SDApiController.CheckMailServiceConnection userID={0}, userName={1}", user.Id, user.UserName);
                //
                return BLL.SD.KB.KBArticle.CheckMailServiceConnection();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return null;
            }
        }
        #endregion

        #region method SearchKBA
        [HttpGet]
        [Route("sdApi/searchKBA", Name = "SearchKBA")]
        public IList<DTL.SD.KB.KBArticleShort> SearchKBA([FromQuery] String text, [FromQuery] bool seeInvisible)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.SearchKBA userID={0}, userName={1}, text='{2}', seeInvisible={3}", user.Id, user.UserName, text, seeInvisible);
            if (seeInvisible && !user.User.HasRoles)
            {
                Logger.Warning("SDApiController.SearchKBA userID={0}, userName={1}, text='{2}', seeInvisible={3} failed (user is client)", user.Id, user.UserName, text, seeInvisible);
                return null;
            }
            try
            {
                text = System.Web.HttpUtility.UrlDecode(text);
                using (var dt = DataSource.GetDataSource())
                {
                    var retval = KBArticleShort.Search(text, dt);
                    if (!seeInvisible && retval != null)
                        retval = retval.Where(f => f.Visible).ToList();
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка поиска статей БЗ");
                return null;
            }
        }
        #endregion

        #region method GetFullKBAList
        [HttpGet]
        [Route("sdApi/getFullKBAList", Name = "GetFullKBAList")]
        public IList<KBArticleShort> GetFullKBAList([FromQuery] bool seeInvisible)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetFullKBAList userID={0}, userName={1}, seeInvisible={2}", user.Id, user.UserName, seeInvisible);
            if (seeInvisible && !user.User.HasRoles)
            {
                Logger.Warning("SDApiController.GetFullKBAList userID={0}, userName={1}, seeInvisible={2} failed (user is client)", user.Id, user.UserName, seeInvisible);
                return null;
            }
            try
            {
                using (var dt = DataSource.GetDataSource())
                {
                    var retval = KBArticleShort.GetList(dt);
                    if (!seeInvisible && retval != null)
                        retval = retval.Where(f => f.Visible).ToList();
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения статей БЗ");
                return null;
            }
        }
        #endregion

        #region method GetKBAList
        [HttpGet]
        [Route("sdApi/getKBAList", Name = "GetKBAList")]
        public IList<KBArticleShort> GetKBAList([FromQuery] Guid folderID, [FromQuery] bool seeInvisible)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetKBAList userID={0}, userName={1}, folderID={2}, seeInvisible={3}", user.Id, user.UserName, folderID, seeInvisible);
            if (seeInvisible && !user.User.HasRoles)
            {
                Logger.Warning("SDApiController.GetKBAList userID={0}, userName={1}, folderID={2}, seeInvisible={3} failed (user is client)", user.Id, user.UserName, folderID, seeInvisible);
                return null;
            }
            try
            {
                using (var dt = DataSource.GetDataSource())
                {
                    
                    var retvalArticleShort = KBArticleShort.GetList(folderID, dt);
                    var KBAIDAccess = KBArticleShort.GetKBAIDAccessByUser(user.User.ID, dt);

                    retvalArticleShort.RemoveAll(x => !KBAIDAccess.Exists(y => y == x.Id));

                    var retvalFolders = KBAFolder.GetFullFolders();

                    var kbaFolderHierarchy = KBAFolder.GetFolderHierarchy(retvalFolders, folderID);

                    if (kbaFolderHierarchy != null && folderID != Guid.Empty)
                    {
                        var folderHierarchy = kbaFolderHierarchy as KBAFolder[] ?? kbaFolderHierarchy.ToArray();

                        if (folderHierarchy.Length != 0)
                        {
                            var foldersId = folderHierarchy.Select(p => p.ID).ToList();
                            //
                            foldersId.Add(folderID);
                            //
                            var list = KBArticleShort.GetList(foldersId, dt);

                            list.RemoveAll(x => !KBAIDAccess.Exists(y => y == x.Id));
                            //
                            return list;
                        }
                    }
                    return retvalArticleShort;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения статей БЗ по папке");
                return null;
            }
        }
        #endregion

        #region method GetKBAReferenceList
        public sealed class GetKBAReferenceListOutModel
        {
            public IList<KBArticleShort> List { get; set; }
            public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [Route("sdApi/getKBAReferenceList", Name = "GetKBAReferenceList")]
        public GetKBAReferenceListOutModel GetKBAReferenceList([FromQuery] Guid objectID, [FromQuery] int objectClassID, [FromQuery] bool seeInvisible)
        {
            return new GetKBAReferenceListOutModel
            {
                List = new List<KBArticleShort>(),
                Result = RequestResponceType.Success
            };

            var user = base.CurrentUser;
            if (user == null)
                return new GetKBAReferenceListOutModel() { Result = RequestResponceType.NullParamsError };
            //
            Logger.Trace("SDApiController.GetKBAReferenceList userID={0}, userName={1}, objectID={2}, objectClassID={3}, seeInvisible={4}", user.Id, user.UserName, objectID, objectClassID, seeInvisible);
            try
            {
                using (var ds = DataSource.GetDataSource())
                {
                    var retval = KBArticleShort.GetList(objectID, objectClassID, ds);
                    if (!seeInvisible && retval != null)
                        retval = retval.Where(f => f.Visible).ToList();
                    return new GetKBAReferenceListOutModel() { List = retval, Result = RequestResponceType.Success };
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения статей БЗ для связанного объекта");
                return new GetKBAReferenceListOutModel() { Result = RequestResponceType.GlobalError };
            }
        }
        #endregion

        #region method GetKBAFolders
        [HttpGet]
        [Route("sdApi/getKBAFolders", Name = "GetKBAFolders")]
        public IList<KBAFolder> GetKBAFolders([FromQuery] string parentID, [FromQuery] bool seeInvisible)
        {
            var user = base.CurrentUser;
            // 
            Logger.Trace("SDApiController.GetKBAFolders userID={0}, userName={1}, parentID='{2}', seeInvisible='{3}'", user.Id, user.UserName, parentID ?? string.Empty, seeInvisible);
            if (seeInvisible && !user.User.HasRoles)
            {
                Logger.Warning("SDApiController.GetKBAFolders userID={0}, userName={1}, parentID='{2}', seeInvisible='{3}' failed (user is client)", user.Id, user.UserName, parentID ?? string.Empty, seeInvisible);
                return null;
            }
            //
            Guid? id = null;
            Guid tempId;
            if (Guid.TryParse(parentID, out tempId))
                id = tempId;
            else if (!String.IsNullOrWhiteSpace(parentID))
                throw new ArgumentException("Parent ID is wrong");
            //
            try
            {
                var retval = KBAFolder.GetList(id);
                if (!seeInvisible)
                    retval = retval.Where(f => f.Visible).ToList();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, String.Concat("Ошибка запроса папок базы знаний. ID родителя: ", parentID ?? string.Empty));
                return null;
            }
        }

        [HttpGet]
        [Route("sdApi/getKBAFolderHierarchy", Name = "GetKBAFolderHierarchy")]
        public IList<KBAFolder> GetKBAFolderHierarchy([FromQuery] string parentID, [FromQuery] bool seeInvisible)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetKBAFolders userID={0}, userName={1}, parentID='{2}', seeInvisible='{3}'", user.Id, user.UserName, parentID ?? string.Empty, seeInvisible);
            if (seeInvisible && !user.User.HasRoles)
            {
                Logger.Warning("SDApiController.GetKBAFolders userID={0}, userName={1}, parentID='{2}', seeInvisible='{3}' failed (user is client)", user.Id, user.UserName, parentID ?? string.Empty, seeInvisible);
                return null;
            }

            Guid? id = null;

            if (Guid.TryParse(parentID, out var tempId))
            {
                id = tempId;
            }
            else if (!string.IsNullOrWhiteSpace(parentID))
            {
                throw new ArgumentException("Parent ID is wrong");
            }

            try
            {
                var retvalFolders = KBAFolder.GetFullFolders();

                var kbaFolderHierarchy = KBAFolder.GetFolderHierarchy(retvalFolders, id);

                if (kbaFolderHierarchy != null)
                {
                    var folderHierarchy = kbaFolderHierarchy as KBAFolder[] ?? kbaFolderHierarchy.ToArray();

                    if (!seeInvisible)
                    {
                        folderHierarchy = folderHierarchy.Where(f => f.Visible).ToArray();
                    }

                    return folderHierarchy;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, string.Concat("Ошибка запроса папок базы знаний. ID родителя: ", parentID ?? string.Empty));
                return null;
            }
        }
        #endregion

        #region method SearchKBArticleInfoList
        [HttpGet]
        [Route("sdApi/SearchKBArticleInfoList", Name = "SearchKBArticleInfoList")]
        public DTL.SD.KB.KBArticleInfo[] SearchKBArticleInfoList(string text, bool clientRegistration, Guid? serviceItemAttendanceID)
        {
            return Array.Empty<DTL.SD.KB.KBArticleInfo>();
            var user = base.CurrentUser;
            if (clientRegistration && !KBArticle.UseAtCallRegistration())
            {
                Logger.Trace("SDApiController.SearchKBArticleInfoList userID={0}, userName={1}, text='{2}', serviceItemAttendanceID='{3}' canceled by settings", user.Id, user.UserName, text, serviceItemAttendanceID.HasValue ? serviceItemAttendanceID.Value.ToString() : "<null>");
                return null;
            }
            //
            Logger.Trace("SDApiController.SearchKBArticleInfoList userID={0}, userName={1}, text='{2}', serviceItemAttendanceID='{3}'", user.Id, user.UserName, text, serviceItemAttendanceID.HasValue ? serviceItemAttendanceID.Value.ToString() : "<null>");
            try
            {
                text = System.Web.HttpUtility.UrlDecode(text);
                var kbArticleInfoList = KBArticleInfo.Search(text, serviceItemAttendanceID);//only visible (filter in sql)
                //
                if (kbArticleInfoList == null)
                    return null; //no search service;
                //
                var retval = kbArticleInfoList.
                    Select(x => new DTL.SD.KB.KBArticleInfo
                    {
                        Description = x.Description,
                        HTMLDescription = x.HTMLDescription,//convert html images from kbFiles
                        HTMLSolution = x.HTMLSolution,
                        ID = x.ID,
                        Name = x.Name,
                        TagString = x.TagString
                    }).
                    ToArray();
                //
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка поиска информации о статьях БЗ.");
                return null;
            }
        }
        #endregion

        #region method GetKBArticle
        [HttpGet]
        [Route("sdApi/GetKBArticle", Name = "GetKBArticle")]
        public object GetKBArticle([FromQuery] String kbaId)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetKBArticle userID={0}, userName={1}, kbaId={2}", user.Id, user.UserName, kbaId);
            try
            {
                Guid id;
                if (Guid.TryParse(kbaId, out id))
                {
                    using (var dt = DataSource.GetDataSource())
                    {
                        var kba = BLL.SD.KB.KBArticle.Get(id);
                        var KBAIDAccess = KBArticleShort.GetKBAIDAccessByUser(user.User.ID, dt);
                        if (!KBAIDAccess.Contains(kba.Id))
                        {
                            Logger.Warning("SDApiController.GetKBArticle userID={0}, userName={1}, kbaId={2} failed (user is client)", user.Id, user.UserName, kbaId);
                            return null;
                            //TODO: check operationIsGranted for kba_properties???
                        }
                        return new
                        {
                            Id = kba.Id,
                            Number = kba.Number,
                            Name = kba.Name,
                            HTMLDescription = kba.HTMLDescription,
                            HTMLSolution = kba.HTMLSolution,
                            HTMLAlternativeSolution = kba.HTMLAlternativeSolution,
                            AuthorID = kba.AuthorID,
                            AuthorFullName = kba.AuthorFullName,
                            DateCreated = kba.DateCreatedString,
                            DateModified = kba.DateModifiedString,
                            Tags = kba.Tags,
                            Readed = kba.Rating,
                            Rated = kba.Rating,
                            ApplicationCount = kba.ApplicationCount,
                            ViewsCount = kba.ViewsCount,
                            Section = kba.Section,
                            StatusID = kba.StatusID,
                            StatusName = kba.StatusName,
                            TypeID = kba.TypeID,
                            TypeName = kba.TypeName,
                            VisibleForClient = kba.Visible,
                            ExpertID = kba.ExpertID,
                            ExpertFullName = kba.ExpertFullName,
                            DateValidUntil = kba.DateValidUntil,
                            AccessID = kba.AccessID,
                            AccessName = kba.AccessName,
                            LifeCycleStateID = kba.LifeCycleStateID,
                            LifeCycleStateName = kba.LifeCycleStateName,
                            KBADependencyList = kba.DependencyList
                        };
                    }
                }
                else return null;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetKBArticle error");
                return null;
            }
        }
        #endregion

        #region method GetKBArticleID
        [HttpGet]
        [Route("sdApi/GetKBArticleID", Name = "GetKBArticleID")]
        public Guid? GetKBArticleID([FromQuery] int number)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("SDApiController.GetKBArticleID userID={0}, userName={1}, number={2}", user.Id, user.UserName, number);
            try
            {
                var retval = KBArticle.GetIDByNumber(number);
                return retval;
            }
            catch (Exception e)
            {
                Logger.Error(e, "GetKBArticleID error");
                return null;
            }
        }
        #endregion

        #region EditKBReference
        public sealed class EditKBReferenceIncomingModel
        {
            public Guid KBArticleID { get; set; }
            public Guid ObjectID { get; set; }
            public int ObjectClassID { get; set; }
            public BLL.Fields.FieldOperation Operation { get; set; }
        }

        [HttpPost]
        [Route("sdApi/EditKBReference", Name = "EditKBReference")]
        public RequestResponceType EditKBReference(EditKBReferenceIncomingModel model)
        {
            var user = base.CurrentUser;
            if (model != null && user != null)
            {
                try
                {
                    using (var dataSource = DataSource.GetDataSource())
                    {
                        if (model.ObjectClassID == IMSystem.Global.OBJ_CALL)
                        {
                            if (!BLL.SD.Calls.Call.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiControllerKB.EditKBReference userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_WORKORDER)
                        {
                            if (!BLL.SD.WorkOrders.WorkOrder.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiControllerKB.EditKBReference userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else if (model.ObjectClassID == IMSystem.Global.OBJ_PROBLEM)
                        {
                            if (!BLL.SD.Problems.Problem.AccessIsGranted(model.ObjectID, user.User, true, dataSource))
                            {
                                Logger.Error("SDApiControllerKB.EditKBReference userID={0}, userName={1}, objID={2} failed (access denied)", user.Id, user.UserName, model.ObjectID);
                                return RequestResponceType.AccessError;
                            }
                        }
                        else throw new NotSupportedException("ObjectClassID not valid");
                        //
                        KBArticleShort.EditKBReference(model.KBArticleID, model.ObjectID, model.ObjectClassID, model.Operation, dataSource, user.User);
                        BLL.WorkflowWrapper.MakeOnSaveReaction(model.ObjectID, model.ObjectClassID, dataSource, user.User);
                        return RequestResponceType.Success;
                    }
                }
                catch (NotSupportedException e)
                {
                    Logger.Error(e, String.Format(@"EditKBReference not supported, model: '{0}'", model));
                    return RequestResponceType.BadParamsError;
                }
                catch (ArgumentValidationException e)
                {
                    Logger.Error(e, String.Format(@"EditKBReference validation error, model: '{0}'", model));
                    return RequestResponceType.ValidationError;
                }
                catch (Exception e)
                {
                    Logger.Error(e, String.Format(@"EditKBReference, model: '{0}'", model));
                    return RequestResponceType.GlobalError;
                }
            }
            else return RequestResponceType.NullParamsError;
        }
        #endregion

        #region method EditKBFolder

        public sealed class KBFolderModel
        {
            public Guid? ID { get; set; }
            public string Name { get; set; }
            public string Note { get; set; }
            public bool SeeInvisible { get; set; }
        }

        [HttpPost]
        [Route("sdApi/editKBFolder", Name = "EditKBFolder")]
        public IActionResult EditKBFolder(KBFolderModel folderModel)
        {
            if (ModelState.IsValid)
            {
                var user = base.CurrentUser;

                if (user == null)
                {
                    return this.Unauthorized();
                }

                if (folderModel.Name == null)
                {
                    Logger.Trace($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ID='{folderModel.ID}', seeInvisible='{folderModel.SeeInvisible}' failed (folder name is null)");
                    throw new ArgumentException("Folder ID is wrong");
                }

                Logger.Trace($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ID='{folderModel.ID}', seeInvisible='{folderModel.SeeInvisible}'");
                if (folderModel.SeeInvisible && !user.User.HasRoles)
                {
                    Logger.Warning($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ID='{folderModel.ID}', seeInvisible='{folderModel.SeeInvisible}' failed (user is client)");
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                if (folderModel.ID == null || Guid.Empty == folderModel.ID)
                {
                    Logger.Trace($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ID='{folderModel.ID}', seeInvisible='{folderModel.SeeInvisible}' failed (folder id is wrong)");
                    throw new ArgumentException("ID is wrong");
                }

                try
                {
                    KBAFolder.EditFolder(folderModel.ID, folderModel.Name, folderModel.Note ?? string.Empty);

                    return this.Ok(new { success = true});
                }
                catch (Exception e)
                {
                    Logger.Error(e, string.Concat("HttpPost: EdidKBFolder. Ошибка запроса папок базы знаний. ID: ", folderModel.ID));
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            Logger.Error("HttpPost: EdidKBFolder. Недопустимое состояние модели:");
            return this.BadRequest();
        }
        #endregion
        
        #region method AddKBFolder

        public sealed class AddKBFolderModel
        {
            public Guid? ParentID { get; set; }
            public string Name { get; set; }
            public string Note { get; set; }
            public bool SeeInvisible { get; set; }
        }

        [HttpPost]
        [Route("sdApi/addChildKBFolder", Name = "AddChildKBFolder")]
        public IActionResult AddChildKBFolder(AddKBFolderModel folderModel)
        {
            if (ModelState.IsValid)
            {
                var user = base.CurrentUser;

                if (user == null)
                {
                    return this.Unauthorized();
                }

                if (folderModel.Name == null)
                {
                    Logger.Trace($"SDApiController.AddChildKBFolder userID={user.Id}, userName={user.UserName}, ParentID='{folderModel.ParentID}', seeInvisible='{folderModel.SeeInvisible}' failed (folder name is null)");
                    throw new ArgumentException("Folder ID is wrong");
                }

                Logger.Trace($"SDApiController.AddChildKBFolder userID={user.Id}, userName={user.UserName}, ParentID='{folderModel.ParentID}', seeInvisible='{folderModel.SeeInvisible}'");
                if (folderModel.SeeInvisible && !user.User.HasRoles)
                {
                    Logger.Warning($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ParentID='{folderModel.ParentID}', seeInvisible='{folderModel.SeeInvisible}' failed (user is client)");
                    return StatusCode((int)HttpStatusCode.Forbidden);
                }

                if (folderModel.ParentID == null || Guid.Empty == folderModel.ParentID)
                {
                    Logger.Trace($"SDApiController.EdidKBFolder userID={user.Id}, userName={user.UserName}, ParentID='{folderModel.ParentID}', seeInvisible='{folderModel.SeeInvisible}' failed (folder id is wrong)");
                    throw new ArgumentException("ID is wrong");
                }

                try
                {
                    var kbaFolder = new DTL.SD.KB.KBAFolder
                                              {
                                                  ID = Guid.NewGuid(),
                                                  Name = folderModel.Name,
                                                  ParentID = folderModel.ParentID,
                                                  Note = folderModel.Note ?? string.Empty,
                                                  Visible = true
                    };

                    KBAFolder.AddKBAChildFolder(kbaFolder);

                    return this.Ok(new { success = true, section = kbaFolder });
                }
                catch (Exception e)
                {
                    Logger.Error(e, string.Concat("HttpPost: EdidKBFolder. Ошибка запроса папок базы знаний. ParentID: ", folderModel.ParentID));
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }
            Logger.Error("HttpPost: AddChildKBFolder. Недопустимое состояние модели.");
            return this.BadRequest();
        }
        #endregion

        #region method DeleteKBFolder

        /// <summary>
        /// deleting a folder
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="seeInvisible"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("sdApi/deleteKBFolder", Name = "DeleteKBFolder")]
        public async Task<ResultWithMessage> DeleteKBFolder([FromQuery] Guid ID, [FromQuery] bool seeInvisible)
        {
            await _knowledgeBaseBLL.DeleteFolderAsync(ID, seeInvisible);
            return ResultWithMessage.Create(RequestResponceType.Success);
        }
        #endregion

        #region GetServerAddress
        public sealed class ServerAddress
        {
            public string address { get; set; }
        }

        [HttpGet]
        [Route("sdApi/getServerAddress", Name = "GetServerAddress")]
        public ServerAddress GetServerAddress()
        {
            Logger.Trace("sdApi/getServerAddress");

            try
            {
                string address = BLL.SD.KB.KBArticle.GetWebServerAddress();

                return new ServerAddress { address = address };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения ServerAddress (sdApi/getServerAddress)");
                return new ServerAddress();
            }
        }

        #endregion

        #region method GetKbaTypeList
        public sealed class GetKbaTypeListOutModel
        {
            public KbaHelper[] List { get; set; }
            //public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetKbaTypeList", Name = "GetKbaTypeList")]
        public List<KbaHelper> GetKbaTypeList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("SDApiController.GetKbaTypeList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetKbaTypeList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = KbaHelper.GetKbaTypeList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка типов статей базы знаний.");
                return null;
            }
        }
        #endregion

        #region method GetKbaStatusList
        public sealed class GetKbaStatusListOutModel
        {
            public KbaHelper[] List { get; set; }
            //public RequestResponceType Result { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetKbaStatusList", Name = "GetKbaStatusList")]
        public List<KbaHelper> GetKbaStatusList()
        {
            try
            {
                var user = base.CurrentUser;
                //
                Logger.Trace("SDApiController.GetKbaStatusList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetKbaStatusList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = KbaHelper.GetKbaStatusList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка статусов статей базы знаний.");
                return null;
            }
        }
        #endregion

        #region EditKbArticle
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/EditKbArticle", Name = "EditKbArticle")]
        public bool EditKbArticle(KbaInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return false;
                //
                Logger.Trace("SDApiController.EditKbArticle userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    BLL.SD.KB.KBArticle.EditKbArticle(model, dataSource);
                    //
                    return true;
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"EditKbArticle not supported, model: '{0}'", model));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "EditKbArticle, model: {0}.", model);
                return false;
            }
        }
        #endregion

        #region AddKbArticle
        [HttpPost]
        [AcceptVerbs("POST")]
        [Route("sdApi/AddKbArticle", Name = "AddKbArticle")]
        public bool AddKbArticle(KbaInfo model)
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return false;
                //
                Logger.Trace("SDApiController.AddKbArticle userID={0}, userName={1}", user.Id, user.UserName);
                //
                using (var dataSource = DataSource.GetDataSource())
                {
                    BLL.SD.KB.KBArticle.AddKbArticle(model, dataSource);
                    //
                    return true;
                }
            }
            catch (NotSupportedException ex)
            {
                Logger.Error(ex, String.Format(@"AddKbArticle not supported, model: '{0}'", model));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "AddKbArticle, model: {0}.", model);
                return false;
            }
        }
        #endregion

        #region method GetKbaAccessList
        public sealed class sdApiGetKbaAccessListOutModel
        {
            public KbaHelper[] List { get; set; }
        }

        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetKbaAccessList", Name = "GetKbaAccessList")]
        public List<KbaHelper> GetKbaAccessList()
        {
            try
            {
                var user = base.CurrentUser;
                if (user == null)
                    return null;
                //
                Logger.Trace("SDApiController.GetKbaAccessList userID={0}, userName={1}", user.Id, user.UserName);
                //
                if (!user.User.HasRoles)
                {
                    Logger.Error("SDApiController.GetKbaAccessList userID={0}, userName={1} failed (access denied)", user.Id, user.UserName);
                    return null;
                }
                //
                var retval = KbaHelper.GetKbaAccessList();
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка доступов статей базы знаний.");
                return null;
            }
        }
        #endregion

        #region GetKbaTmpGuid
        [HttpGet]
        [AcceptVerbs("GET")]
        [Route("sdApi/GetKbaTmpGuid", Name = "GetKbaTmpGuid")]
        public Guid GetKbaTmpGuid()
        {
            return Guid.NewGuid();
        }
        #endregion
    }
}