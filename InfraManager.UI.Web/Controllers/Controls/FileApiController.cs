using InfraManager.Core.Caching;
using InfraManager.Core.Data;
using InfraManager.Core.Logging;
using InfraManager.UI.Web.Helpers;
using InfraManager.UI.Web.ModelBinding;
using InfraManager.Web.DTL.Repository;
using InfraManager.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InfraManager.Web.Controllers
{
    public class FileApiController : BaseApiController
    {
        #region fields
        private string _uploadFolder;
        private const string DOWNLOAD_URL = "fileApi/downloadFile";
        private const string DOWNLOAD_ARCHIVE_URL = "fileApi/downloadFilesAsArchive";
        //
        private static ConcurrentDictionary<string, UploadState> __uploadStates = new ConcurrentDictionary<string, UploadState>();//received bytes on server
        #endregion

        #region constructor
        public FileApiController(IWebHostEnvironment environment)
            : base()
        {
            _uploadFolder = Path.Combine(environment.ContentRootPath, "Data/Upload");
            //
            //TODO: будет тормозить - смотрим в сторону FileWatcher
            try
            {
                if (!Directory.Exists(_uploadFolder))
                {
                    Logger.Trace("FileApiController.ctor: create upload directory '{0}'", _uploadFolder);
                    Directory.CreateDirectory(_uploadFolder);
                    Logger.Trace("FileApiController.ctor: upload directory '{0}' created", _uploadFolder);
                }
                else
                {
                    Logger.Trace("FileApiController.ctor: check old temp  files in '{0}'", _uploadFolder);
                    var files = Directory.GetFiles(_uploadFolder);
                    foreach (var file in files)
                    {
                        var fi = new FileInfo(file);
                        if ((DateTime.UtcNow - fi.CreationTimeUtc).TotalDays >= 1)
                        {
                            System.IO.File.Delete(file);
                            Logger.Trace("FileApiController.ctor: delele file '{0}'", file);
                        }
                    }
                }
                //
                //
                var uploadStates = new List<UploadState>(__uploadStates.Values);
                foreach (var uploadState in uploadStates)
                    if ((DateTime.UtcNow - uploadState.UtcCreationDate).TotalDays >= 1 ||
                        uploadState.IsUploadCompleted && (DateTime.UtcNow - uploadState.UtcCreationDate).TotalMinutes >= 1)
                    {
                        UploadState tmp;
                        __uploadStates.TryRemove(uploadState.FilePostfix, out tmp);
                    }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка создания папки на сервере.");
                //todo redirect from ctor unavailable!
                //base.Redirect(string.Format("~/Errors/Message?msg={0}", Uri.EscapeDataString(Resources.FailedToCreateDirectory)));
            }
        }
        #endregion

        #region method RemoveInvalidPathChars
        private static string RemoveInvalidPathChars(string str)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var ch in invalidChars)
                str = str.Replace(ch, '@');
            //
            return str;
        }
        #endregion

        #region method GetDocumentInfoList
        [HttpGet]
        [Route("fileApi/getDocumentInfoList", Name = "GetDocumentInfoList")]
        public List<DocumentInfo> GetDocumentInfoList([FromQuery]Guid objectID)
        {
            return new List<DocumentInfo>();
            var user = base.CurrentUser;
            //
            Logger.Trace("FileApiController.GetDocumentInfoList get files by objectID={0}, userID={1}, userName={2}", objectID, user.Id, user.UserName);
            try
            {
                var retval = BLL.Repository.Document.GetInfoList(objectID);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка прикреплений по объекту");
                return null;
            }
        }
        #endregion

        #region method GetDocumentInfo
        [HttpGet]
        [Route("fileApi/getDocumentInfo", Name = "GetDocumentInfo")]
        public DocumentInfo GetDocumentInfo([FromQuery] Guid ID, [FromQuery] Guid ObjectID)
        {
            var user = base.CurrentUser;
            //
            Logger.Trace("FileApiController.GetDocumentInfo get files by ID={0}, userID={1}, userName={2}", ID, user.Id, user.UserName);
            try
            {
                var retval = BLL.Repository.Document.GetInfo(ID, ObjectID);
                return retval;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка получения списка прикреплений по объекту");
                return null;
            }
        }
        #endregion
        //#region method RemoveDocumentWithoutReferences
        //[HttpPost]
        //[Route("fileApi/removeDocumentWithoutReferences", Name = "RemoveDocumentWithoutReferences")]
        //public bool RemoveDocumentWithoutReferences([FromQuery] Guid documentID, [FromQuery] Guid? objectID)
        //{
        //    var user = base.CurrentUser;
        //    if (user == null)
        //        return false;
        //    //
        //    Logger.Trace("FileApiController.RemoveDocumentWithoutReferences check reference and remove document without references id={0}, objectID={1}", documentID, objectID.HasValue ? objectID.Value.ToString() : string.Empty);
        //    try
        //    {
        //        bool retval = BLL.Repository.Document.RemoveDocumentWithoutReferences(documentID, objectID);
        //        //
        //        if (retval)
        //            Logger.Trace("FileApiController.RemoveDocumentWithoutReferences document removed successfully id={0}, objectID={1}", documentID, objectID.HasValue ? objectID.Value.ToString() : string.Empty);
        //        return retval;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Error(ex, "Ошибка удаления документов без связей.");
        //        return false;
        //    }
        //}
        //#endregion

        #region method UploadFile       
        [HttpPost]
        [Route("fileApi/uploadFileMobile", Name = "UploadFileMobile")]
        public async Task<IActionResult> UploadFileMobile(
            [FromQuery] string filePostfix,
            [FromQuery] Guid? objectID,
            [FromQuery] string fName,
            [FromForm] IFormFile[] files)
        {            
            var user = base.CurrentUser;
            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.NonAuthoritativeInformation, "current user is null");

            if (!files.Any())
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "bad request");
            }
            //
            Logger.Trace("FileApiController.UploadFile filePostfix: {0}", filePostfix);
            //
            try
            {
                //
                var file = files[0];
                var uploadState = new UploadState(file.Length, filePostfix);
                if (!__uploadStates.TryAdd(filePostfix, uploadState))
                {
                    Logger.Error(new Exception(HttpStatusCode.Conflict.ToString()), "files upload failed");
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
                }
                //
                Stream fileStream = null;
                //
                var fileName = fName;
                var filePath = Path.Combine(_uploadFolder, fileName + filePostfix);
                int bufferSize = 2048;
                //
                int bytesRead;
                var buffer = new byte[bufferSize];
                using (var stream = file.OpenReadStream())
                {
                    fileStream = System.IO.File.Create(filePath, bufferSize);

                    //
                    try
                    {
                        do
                        {
                            bytesRead = stream.Read(buffer, 0, bufferSize);
                            if (bytesRead > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                        } while (bytesRead > 0);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        fileStream.Close();
                    }
                }
                //
                Logger.Trace("FileApiController.UploadFile filePostfix: {0}. Files successfully uploaded.", filePostfix);
                //
                Logger.Trace("FileApiController.UploadFile file '{0}' uploaded as '{1}'", fileName, filePostfix);
                //
                if (System.IO.File.Exists(filePath))
                {
                    if (objectID.HasValue)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        //
                        byte[] data = System.IO.File.ReadAllBytes(filePath);//TODO OutOfMemory
                        string strMessage = Encoding.Default.GetString(data.Where(x => x != 0).ToArray());
                        string[] pd = strMessage.Split(',');
                        byte[] convertData = Convert.FromBase64String(pd[1]);
                        Guid documentID = BLL.Repository.Document.InsertDocument(fileName, convertData, user.User, objectID.Value, string.Empty);
                        Logger.Trace("FileApiController.UploadFile file saved to repository name='{0}', path='{1}', objectID='{2}'", fileName, filePath, objectID.Value);
                        System.IO.File.Delete(filePath);
                        //
                        uploadState.UploadComplete();
                        return Request.CreateResponse(HttpStatusCode.OK, new { id = documentID });
                    }
                    else
                    {
                        uploadState.UploadComplete();
                        return Request.CreateResponse(HttpStatusCode.OK, "upload successfull");
                    }
                }
                else
                {
                    Logger.Warning("Can't get uploaded file. File not found '{0}'.", filePath);
                    //
                    __uploadStates.TryRemove(filePostfix, out uploadState);
                    //
                    return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType, "file not found");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "files upload failed");
                return Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    ex.Message); // todo: exposing exception messages to a web client isn't secure!!!
            }
        }
    
        //
        

        [HttpPost]
        [Route("fileApi/uploadFile", Name = "UploadFile")]
        public async Task<IActionResult> UploadFile([FromQuery]string filePostfix, [FromQuery]Guid? objectID, [FromQuery]string signalR_connectionID, IFormFile file)
        {
            var files = new IFormFile[] { file };
            var user = base.CurrentUser;
            if (user == null)
                return Request.CreateErrorResponse(
                   HttpStatusCode.NonAuthoritativeInformation,
                   "currentUser is null");
            //
            Logger.Trace("FileApiController.UploadFile filePostfix: {0}", filePostfix);
            //
            if (!files.Any())
                return Request.CreateErrorResponse(
                    HttpStatusCode.UnsupportedMediaType,
                    "invalid content");
            //
            try
            {
                var uploadState = new UploadState(files[0].Length, filePostfix);
                if (!__uploadStates.TryAdd(filePostfix, uploadState))
                {
                    Logger.Error(new Exception(HttpStatusCode.Conflict.ToString()), "files upload failed");
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal error");
                }
                //
                Stream fileStream = null;
                //
                var fileName = RemoveInvalidPathChars(files[0].FileName);
                var filePath = Path.Combine(_uploadFolder, fileName + filePostfix);
                int bufferSize = 2048;
                //
                int bytesRead;
                var buffer = new byte[bufferSize];

                using (var stream = files[0].OpenReadStream())
                {
                    fileStream = System.IO.File.Create(filePath, bufferSize);

                    //
                    try
                    {
                        do
                        {
                            bytesRead = stream.Read(buffer, 0, bufferSize);
                            if (bytesRead > 0)
                            {
                                fileStream.Write(buffer, 0, bytesRead);
                            }
                        } while (bytesRead > 0);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        fileStream.Close();
                    }

                    Logger.Trace("FileApiController.UploadFile filePostfix: {0}. Files successfully uploaded.", filePostfix);
                    //
                    Logger.Trace("FileApiController.UploadFile file '{0}' uploaded as '{1}'", fileName, filePostfix);
                    //
                    if (System.IO.File.Exists(filePath))
                    {
                        if (objectID.HasValue)
                        {
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            //
                            byte[] data = System.IO.File.ReadAllBytes(filePath);//TODO OutOfMemory
                            string test = Convert.ToBase64String(data);
                            Guid documentID = BLL.Repository.Document.InsertDocument(fileName, data, user.User, objectID.Value, signalR_connectionID);
                            Logger.Trace("FileApiController.UploadFile file saved to repository name='{0}', path='{1}', objectID='{2}'", fileName, filePath, objectID.Value);
                            System.IO.File.Delete(filePath);
                            //
                            uploadState.UploadComplete();
                            return Request.CreateResponse(HttpStatusCode.OK, new { id = documentID });
                        }
                        else
                        {
                            uploadState.UploadComplete();
                            return Request.CreateResponse(HttpStatusCode.OK, "upload successfull");
                        }
                    }
                    else
                    {
                        Logger.Warning("Can't get uploaded file. File not found '{0}'.", filePath);
                        //
                        __uploadStates.TryRemove(filePostfix, out uploadState);
                        //
                        return Request.CreateErrorResponse(
                           HttpStatusCode.UnsupportedMediaType,
                           "file not found");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "files upload failed");
                //
                UploadState tmp;
                __uploadStates.TryRemove(filePostfix, out tmp);
                //
                while (ex.InnerException != null)
                    ex = ex.InnerException;
                //
                return Request.CreateErrorResponse(
                    HttpStatusCode.InternalServerError,
                    ex.Message);
            }
        }
        #endregion

        #region method GetUploadState
        [HttpGet]
        [Route("fileApi/getUploadPercentage", Name = "GetUploadPercentage")]
        public byte? GetUploadPercentage([FromQuery]string filePostfix)
        {
            if (string.IsNullOrWhiteSpace(filePostfix))
                return null;
            //
            UploadState uploadState;
            Logger.Trace("FileApiController.GetUploadPercentage try to get uploadState filePostfix={0}", filePostfix);
            //
            if (!__uploadStates.TryGetValue(filePostfix, out uploadState))
            {
                Logger.Trace("FileApiController.GetUploadPercentage failed to get uploadState filePostfix={0}", filePostfix);
                return null;
            }
            //
            if (uploadState.IsUploadCompleted)
                return 100;
            else if (uploadState.TotalSizeOfRequest.HasValue && uploadState.TotalSizeOfRequest.Value > 0)
            {
                var retval = (double)uploadState.ReceivedSize / (double)uploadState.TotalSizeOfRequest.Value * 100d;
                return (byte)retval;
            }
            else
                return 100;
        }
        #endregion


        #region method SaveUploadedFiles
        [HttpPost]
        [Route("fileApi/saveUploadedFiles", Name = "SaveUploadedFiles")]
        public bool SaveUploadedFiles(SaveFileListInfo info)
        {
            try
            {
                var user = base.CurrentUser;
                if (info == null || user == null || info.ObjectID == Guid.Empty || info.Files == null || info.Files.Count == 0)
                    return false;
                //
                string files = string.Join(", ", info.Files);
                Logger.Trace("FileApiController.SaveUploadedFiles userID={0}, userName={1}, objectID={2}, fileList='{3}'", user.Id, user.UserName, info.ObjectID, files);
                //
                List<object> documentList;
                List<string> paths;
                if (!GetDocumentFromFiles(info.Files, out documentList, out paths, user))
                    return false;
                //              
                if (documentList.Count > 0)
                    using (var dataSource = DataSource.GetDataSource())
                        foreach (var document in documentList)
                            BLL.Repository.Document.InsertDocument(document, info.ObjectID, dataSource);
                //
                foreach (var filePath in paths)
                    System.IO.File.Delete(filePath);
                //
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Ошибка сохранения новыых документов в репозиторий.");
                return false;
            }
        }
        #endregion

        #region method GetDocumentFromFiles
        internal bool GetDocumentFromFiles(
            List<UploadFileInfo> fileInfoList, 
            out List<object> documentList, 
            out List<string> paths,
            ApplicationUser user)
        {
            documentList = new List<object>();
            paths = new List<string>();
            //
            GC.Collect();
            GC.WaitForPendingFinalizers();
            //
            if (fileInfoList != null)
                foreach (var fileInfo in fileInfoList)
                    if (!fileInfo.ID.HasValue)
                    {
                        var filePath = Path.Combine(_uploadFolder, RemoveInvalidPathChars(fileInfo.FileName) + fileInfo.FilePostfix);
                        byte[] data = null;
                        try
                        {

                            if (!System.IO.File.Exists(filePath))
                            {
                                Logger.Trace("FileApiController.PrepareFiles file '{0}' not found", filePath);
                                return false;
                            }
                            paths.Add(filePath);
                            data = System.IO.File.ReadAllBytes(filePath);//TODO OutOfMemory
                        }
                        catch (IOException)
                        {
                            return false;
                        }
                        //
                        var document = BLL.Repository.Document.CreateDocument(fileInfo.FileName, data, user.User);
                        documentList.Add(document);
                    }
            //
            return true;
        }
        #endregion


        #region method RemoveUploadedFile
        [HttpPost]
        [Route("fileApi/removeUploadedFile", Name = "RemoveUploadedFile")]
        public bool RemoveUploadedFile([FromBodyOrForm] UploadFileInfo fileInfo)
        {
            if (fileInfo == null || string.IsNullOrWhiteSpace(fileInfo.FileName) || string.IsNullOrWhiteSpace(fileInfo.FilePostfix))
                return false;
            //
            if (fileInfo.ID.HasValue && fileInfo.ObjectID.HasValue)
            {//document in db
                Logger.Trace("FileApiController.RemovedUploadedFile try to remove document id={0}, name='{1}', objectID={2}", fileInfo.ID.Value, fileInfo.FileName, fileInfo.ObjectID.Value);
                try
                {
                    BLL.Repository.Document.RemoveDocument(fileInfo.ID.Value, fileInfo.ObjectID.Value);
                    //
                    Logger.Trace("FileApiController.RemovedUploadedFile document removed successfully id={0}, name='{1}', objectID={2}", fileInfo.ID.Value, fileInfo.FileName, fileInfo.ObjectID.Value);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка удаления связи файла с объектом.");
                }
            }
            else
            {
                var filePath = Path.Combine(_uploadFolder, RemoveInvalidPathChars(fileInfo.FileName) + fileInfo.FilePostfix);
                Logger.Trace("FileApiController.RemovedUploadedFile try to remove file '{0}'", filePath);
                //
                if (!System.IO.File.Exists(filePath))
                {
                    Logger.Trace("FileApiController.RemovedUploadedFile file '{0}' not found", filePath);
                    return true;//file removed -> ok!
                }
                try
                {
                    System.IO.File.Delete(filePath);
                    //
                    Logger.Trace("FileApiController.RemovedUploadedFile file '{0}' removed", filePath);
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "can't remove file '{0}'", filePath);
                }
            }
            return false;
        }
        #endregion

        #region method DownloadFile
        [HttpPost]
        [Route(DOWNLOAD_URL, Name = "DownloadFilePost")]
        public object DownloadFilePost(UploadFileInfo fileInfo)
        {
            var user = base.CurrentUser;
            if (fileInfo == null || string.IsNullOrWhiteSpace(fileInfo.FileName) || string.IsNullOrWhiteSpace(fileInfo.FilePostfix))
                return null;
            //
            var fileID = string.Concat(Guid.NewGuid(), "_", fileInfo.ID, "_", user.Id);
            var filePath = Path.Combine(_uploadFolder, RemoveInvalidPathChars(fileInfo.FileName) + "_" + fileID + fileInfo.FilePostfix);
            Logger.Trace("FileApiController.DownloadFilePost preparing file '{0}' for download", filePath);
            //
            if (fileInfo.ID.HasValue)
            {//for saved in db documents
                try
                {
                    byte[] data = BLL.Repository.Document.GetDocumentData(fileInfo.ID.Value);
                    System.IO.File.WriteAllBytes(filePath, data);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Ошибка получения документа из репозитария.");
                    return null;
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            else if (!System.IO.File.Exists(filePath))
            {
                Logger.Trace("FileApiController.DownloadFilePost file '{0}' not found", filePath);
                return null;
            }
            //
            var info = new DownloadInfo { ID = fileInfo.ID, FileName = fileInfo.FileName, FilePath = filePath, UserID = user.Id };
            CacheManager.Add(fileID, info, ExpirationType.Absolute, TimeSpan.FromSeconds(BLL.Global.LONG_CACHE_SECONDS));
            Logger.Trace("FileApiController.DownloadFilePost file '{0}' prepared for download, id='{1}'", filePath, fileID);
            //
            var requestUrl = Request.Path.Value;
            var url = requestUrl.Substring(0, requestUrl.IndexOf(DOWNLOAD_URL) + DOWNLOAD_URL.Length);
            url = string.Concat(url, "/", fileID);
            return new { url = url };
        }

        [HttpGet]
        [Route(DOWNLOAD_URL + "/{id}", Name = "DownloadFileGet")]
        public ActionResult DownloadFileGet(string id)
        {
            var user = base.CurrentUser;
            if (string.IsNullOrWhiteSpace(id))
                return BadRequest();
            //
            Logger.Trace("FileApiController.DownloadFileGet get file by id='{0}'", id);
            var info = CacheManager.Get<DownloadInfo>(id);
            string filePath = null;
            string fileExtension = null;
            string fileName = null;
            if (info == null)
            {
                DirectoryInfo uploadFolder = new DirectoryInfo(_uploadFolder);
                FileInfo[] files = uploadFolder.GetFiles("*" + id + "*.*");
                //
                if (files.Length == 1)//кэш пуст, поищем файл на диске. если на сервере iis используется несколько процессов, то кэш данного процесса может быть пустым, т.к. он мог быть создан в другом процессе 
                {
                    filePath = files[0].FullName;
                    //
                    string removeStr = "_" + id;
                    int idx = files[0].Name.IndexOf(removeStr);
                    fileName = idx < 0 ? files[0].Name : files[0].Name.Remove(idx);
                    //
                    fileExtension = Path.GetExtension(fileName).ToLower();
                }
                else
                {
                    Logger.Trace("FileApiController.DownloadFileGet fileInfo not exists, id='{0}'", id);
                    return NotFound("info by id not found");
                }
            }
            else if (info.UserID != user.Id)
                return NotFound("content not found for current user");
            else
            {
                filePath = info.FilePath;
                fileExtension = Path.GetExtension(info.FileName).ToLower();
                fileName = info.FileName;
            }
            //
            CacheManager.Remove(id);
            //
            try
            {
                HttpResponseMessage retval = new HttpResponseMessage(HttpStatusCode.OK);                
                string mediaType = "application/octet-stream";//общий вид
                //особо это делать не нужно, но если сервер подскажет тип - будет шикарно
                switch (fileExtension)
                {
                    case ".mp3": mediaType = "audio/mpeg"; break;
                    case ".ogg": case ".flac": mediaType = "audio/ogg"; break;
                    case ".wma": mediaType = "audio/x-ms-wma"; break;
                    case ".wav": mediaType = "audio/vnd.wave"; break;
                    //
                    case ".gif": mediaType = "image/gif"; break;
                    case ".jpg": case ".jpeg": mediaType = "image/jpeg"; break;
                    case ".png": mediaType = "image/png"; break;
                    case ".ico": mediaType = "image/vnd.microsoft.icon"; break;
                    case ".tif": mediaType = "image/tiff"; break;
                    case ".bmp": mediaType = "image/vnd.wap.wbmp"; break;
                    //
                    case ".html": mediaType = "text/html"; break;
                    case ".txt": case ".ini": mediaType = "text/plain"; break;
                    case ".xml": mediaType = "text/xml"; break;
                    //
                    case ".avi": case ".mpg": mediaType = "video/mpeg"; break;
                    case ".mp4": mediaType = "video/mp4"; break;
                    case ".flv": mediaType = "video/x-flv"; break;
                    case ".wmv": mediaType = "video/x-ms-wmv"; break;
                    //
                    case ".pdf": mediaType = "application/pdf"; break;
                    case ".xls": case ".xlsx": mediaType = "application/vnd.ms-excel"; break;
                    case ".doc": case ".docx": mediaType = "application/msword"; break;
                }
                retval.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                retval.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                retval.Content.Headers.ContentDisposition.FileName = fileName;
                retval.Content.Headers.ContentDisposition.FileNameStar = fileName;
                //
                if (!string.IsNullOrEmpty(filePath))
                {//планирование удаления файла по окончанию скачивания                    
                    var utcNow = DateTime.UtcNow;
                    Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                var stream = new FileStream(filePath, FileMode.Open);
                                while ((DateTime.UtcNow - utcNow).TotalMinutes < 5)
                                    if (stream.CanRead && stream.CanSeek && stream.Position < stream.Length)
                                        Thread.Sleep(1000);
                                    else
                                        break;
                                //over .. minutes or file readed to end
                                System.IO.File.Delete(filePath);
                                Logger.Trace("file deleted automaticaly by path='{0}'", filePath);
                            }
                            catch (Exception e)
                            {
                                Logger.Warning(e, "Ошибка удаления файла после скачивания.");
                            }
                        });
                }
                //
                return File(System.IO.File.ReadAllBytes(filePath), mediaType, fileName);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return NotFound("fail to get file");
            }
        }
        #endregion


        #region method DownloadFilesAsArchive
        [HttpPost]
        [Route(DOWNLOAD_ARCHIVE_URL, Name = "DownloadFilesAsArchivePost")]
        public string DownloadFilesAsArchivePost([FromQuery] Guid objectID)
        {
            var user = base.CurrentUser;
            //
            var requestUrl = Request.Path.Value;
            var url = requestUrl.Substring(0, requestUrl.IndexOf(DOWNLOAD_ARCHIVE_URL) + DOWNLOAD_ARCHIVE_URL.Length);
            url = string.Concat(url, "/", objectID);
            return url;
        }

        [HttpGet]
        [Route(DOWNLOAD_ARCHIVE_URL + "/{objectID}", Name = "DownloadFilesAsArchiveGet")]
        public IActionResult DownloadFilesAsArchiveGet(Guid objectID)
        {
            var user = base.CurrentUser;

            Logger.Trace(
                "FileApiController.DownloadFilesAsArchive get files by objectID={0}, userID={1}, userName={2}",
                objectID,
                user.Id,
                user.UserName);

            try
            {
                var fileMemStream = BLL.Repository.Document.GetMemoryStream(objectID);
                var result = new FileStreamResult(fileMemStream, "application/octet-stream");
                result.FileDownloadName = "document.zip";

                return result;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.NoContent, "fail to get file");
            }
        }
        #endregion


        #region helper class DownloadInfo
        private sealed class DownloadInfo
        {
            public DownloadInfo() { }

            public Guid? ID { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }
            public string UserID { get; set; }
        }
        #endregion

        #region helper class SaveFileListInfo
        public sealed class SaveFileListInfo
        {
            public SaveFileListInfo() { }

            public Guid ObjectID { get; set; }
            public List<UploadFileInfo> Files { get; set; }
        }
        #endregion


        #region helper class UploadState
        public sealed class UploadState
        {
            #region constructor
            public UploadState(long? totalSizeOfRequest, string filePostfix)
            {
                this.FilePostfix = filePostfix;
                this.TotalSizeOfRequest = totalSizeOfRequest;
                this.UtcCreationDate = DateTime.UtcNow;
                this.FileName = string.Empty;
                this.ReceivedSize = 0;
                this.IsUploadCompleted = false;
            }
            #endregion

            #region properties
            public string FilePostfix { get; private set; }

            public long? TotalSizeOfRequest { get; private set; }

            public DateTime UtcCreationDate { get; private set; }

            public string FileName { get; private set; }

            public long ReceivedSize { get; private set; }

            public bool IsUploadCompleted { get; private set; }
            #endregion

            #region method Process
            public void Process(long streamPosition)
            {
                this.ReceivedSize = streamPosition;
            }
            #endregion

            #region method StartUpload
            public void StartUpload(string fileName)
            {
                if (!string.IsNullOrWhiteSpace(this.FileName))
                    throw new NotSupportedException("check code");
                //
                this.FileName = fileName;
                this.ReceivedSize = 0;
            }
            #endregion

            #region method UploadComplete
            public void UploadComplete()
            {
                this.IsUploadCompleted = true;
            }
            #endregion
        }
        #endregion

        #region helper class WriteFileStreamProgress
        public sealed class WriteFileStreamProgress : FileStream
        {
            #region fields
            private Action<long> _progressAction;
            #endregion

            #region constructor
            public WriteFileStreamProgress(string path, Action<long> progressAction)
                : base(path, FileMode.Create, FileAccess.Write)
            {
                if (progressAction == null)
                    throw new ArgumentNullException("progressAction");
                _progressAction = progressAction;
            }
            #endregion

            #region override EndWrite
            public override void EndWrite(IAsyncResult asyncResult)
            {
                base.EndWrite(asyncResult);
                _progressAction(base.Position);
            }
            #endregion

            #region override Write
            public override void Write(byte[] array, int offset, int count)
            {
                base.Write(array, offset, count);
                _progressAction(base.Position);
            }
            #endregion
        }
        #endregion
    }
}
