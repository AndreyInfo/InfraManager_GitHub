using InfraManager.BLL.ServiceDesk;
using InfraManager.UI.Web.Helpers;
using InfraManager.Web.BLL.Helpers;
using InfraManager.Web.Controllers;
using InfraManager.Web.DTL.Repository;
using InfraManager.WebApi.Contracts.Models.Documents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.UI.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using InfraManager.BLL;

namespace InfraManager.UI.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FilesController : BaseApiController
{
    private readonly IDocumentBLL _documentBLL;
    private readonly IMemoryCache _memoryCache;

    public FilesController(
        IDocumentBLL documentBLL,
        IMemoryCache memoryCache)
    {
        _documentBLL = documentBLL;
        _memoryCache = memoryCache;
    }

    [DisableFormValueModelBinding]
    [RequestSizeLimit((long)FileSettings.MaxFileSize)]
    [RequestFormLimits(MultipartBodyLengthLimit = (long)FileSettings.MaxFileSize)]
    [HttpPost]
    public async Task<Guid> UploadAsync(CancellationToken cancellationToken)
    {

        Guid documentID = Guid.Empty;
        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            throw new Exception("Not a multipart request");

        var boundary = MultipartRequestHelper.GetBoundary(MediaTypeHeaderValue.Parse(Request.ContentType), (int)FileSettings.MaxFileSize);
        var reader = new MultipartReader(boundary, Request.Body);

        var section = await reader.ReadNextSectionAsync();

        if (section == null)
            throw new Exception("No sections in multipart defined");

        byte[] streamedFileContent = null;
        string trustedFileNameForDisplay = string.Empty;

        while (section != null)
        {
            var hasContentDispositionHeader =
                ContentDispositionHeaderValue.TryParse(
                    section.ContentDisposition, out var contentDisposition);

            if (hasContentDispositionHeader)
            {
                if (!MultipartRequestHelper
                    .HasFileContentDisposition(contentDisposition))
                {
                    throw new FileNotFoundException($"The request couldn't be processed with file.");

                }
                else
                {
                    trustedFileNameForDisplay = WebUtility.HtmlEncode(
                            contentDisposition.FileName.Value);

                    streamedFileContent = await FileHelper.ProcessStreamedFile(
                        section, (long)FileSettings.MaxFileSize);

                }
            }

            section = await reader.ReadNextSectionAsync();

        }
        if (streamedFileContent != null)
        {
            documentID = await _documentBLL.InsertDocumentAsync(trustedFileNameForDisplay, streamedFileContent, cancellationToken);
        }

        return documentID;

    }

    [HttpPost("/api/data/downloadfilesarchive")]
    public async Task<object> GetLinkForFilesArchiveAsync([FromForm] Guid[] docIDs, CancellationToken cancellationToken)
    {
        const string DOWNLOAD_URL = "api/data/downloadfilesarchive";

        var docInfos = await _documentBLL.GetDocumentsDataAsync(docIDs, cancellationToken);
        var compressedFileStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
        {
            foreach (var docModel in docInfos)
            {
                // Get file bytes
                var documentData = await _documentBLL.GetDocumentDataAsync(docModel.Id, cancellationToken);
                var zipEntry = zipArchive.CreateEntry(documentData.Name + '.' + documentData.Extension);

                // Get the stream of the attachment
                using (var fileStream = new MemoryStream(documentData.Data, false))
                using (var zipEntryStream = zipEntry.Open())
                {
                    await fileStream.CopyToAsync(zipEntryStream, cancellationToken);
                }
            }
        }
        var cacheKey = Guid.NewGuid();
        _memoryCache.Set(cacheKey, compressedFileStream);
        var requestUrl = Request.Path.Value;
        var url = requestUrl.Substring(0, requestUrl.IndexOf(DOWNLOAD_URL) + DOWNLOAD_URL.Length);
        url = string.Concat(url, "/" + cacheKey);
        return new { url = url };
    }

    [HttpGet("/api/data/downloadfilesarchive/{cacheKey}")]
    public ActionResult DownloadFilesArchive(
                            Guid cacheKey,
                            CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(cacheKey, out MemoryStream result);
        return File(result.ToArray(), GetMimeTypeForFileExtension("document.zip"), "document.zip");
    }

    [HttpGet("/api/object/{objectID}/documents")]
    public async Task<DocumentInfo[]> GetObjectDocuments(Guid objectID, CancellationToken cancellationToken)
    {
        var docInfos = await _documentBLL.GetObjectDocumentsAsync(objectID, cancellationToken);
        return docInfos
            .Select(
                x => new DocumentInfo
                {
                    ID = x.Id,
                    ObjectID = x.ObjectId,
                    Size = x.Size,
                    AuthorID = x.AuthorId,
                    Name = x.Name.Trim(),
                    Extension = x.Extension.Trim(),
                    UtcDateCreated = JSDateTimeHelper.ToJS(x.DateCreated)
                }) // TODO: Автомаппер
            .ToArray();
    }

    [HttpDelete("/api/object/{objectID}/documents")]
    public async Task DeleteObjectDocumentsAsync(Guid objectID, CancellationToken cancellationToken)
    {
        await _documentBLL.DeleteObjectDocumentsAsync(objectID, null, cancellationToken);
    }

    [HttpDelete("/api/object/{objectID}/documents/{documentId}")] // fileApi/removeUploadedFile
    public async Task DeleteObjectDocumentAsync(Guid objectID, Guid documentID, CancellationToken cancellationToken)
    {
        await _documentBLL.DeleteObjectDocumentsAsync(objectID, documentID, cancellationToken);
    }

    [HttpGet("{documentId}/url")]
    public async Task<object> CheckFile(Guid documentId, CancellationToken cancellationToken)
    {
        const string DOWNLOAD_URL = "api/files/";

        var _ = await _documentBLL.GetObjectDocumentAsync(documentId, cancellationToken);

        var requestUrl = Request.Path.Value;
        var url = requestUrl.Substring(0, requestUrl.IndexOf(DOWNLOAD_URL) + DOWNLOAD_URL.Length);
        url = string.Concat(url, documentId, "/data");
        return new { url };
    }

    [HttpGet("/api/object/{objectID}/check-attachments")]
    public async Task<object> CheckAttachments(Guid objectID, CancellationToken cancellationToken)
    {
        const string DOWNLOAD_URL = "api/object/";

        var docInfos = await _documentBLL.GetObjectDocumentsAsync(objectID, cancellationToken);
        var requestUrl = Request.Path.Value;
        var url = requestUrl.Substring(0, requestUrl.IndexOf(DOWNLOAD_URL) + DOWNLOAD_URL.Length);
        url = string.Concat(url, objectID, "/attachments");
        return new { url = url };
    }

    [HttpGet("/api/object/{objectID}/attachments")]
    public async Task<ActionResult> DownloadAttachments(Guid objectID, CancellationToken cancellationToken)
    {
        var docInfos = await _documentBLL.GetObjectDocumentsAsync(objectID, cancellationToken);

        var compressedFileStream = new MemoryStream();
        using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Create, false))
        {
            foreach (var docModel in docInfos)
            {
                // Get file bytes
                var documentData = await _documentBLL.GetDocumentDataAsync(docModel.Id, cancellationToken);
                var zipEntry = zipArchive.CreateEntry(documentData.Name + '.' + documentData.Extension);

                // Get the stream of the attachment
                using (var fileStream = new MemoryStream(documentData.Data, false))
                using (var zipEntryStream = zipEntry.Open())
                {
                    await fileStream.CopyToAsync(zipEntryStream, cancellationToken);
                }
            }
        }
        return File(compressedFileStream.ToArray(), GetMimeTypeForFileExtension("document.zip"), "document.zip");
    }

    [HttpGet("{documentId}/data")]
    public async Task<ActionResult> DownloadFile(Guid documentId, CancellationToken cancellationToken)
    {
        var documentData = await _documentBLL.GetDocumentDataAsync(documentId, cancellationToken);
        var fileName = string.Concat(documentData.Name, ".", documentData.Extension).Trim(new char[] { '.' });
        return File(documentData.Data, GetMimeTypeForFileExtension(fileName), fileName);
    }

    [HttpPost("data")]
    public async Task<Guid> AddAsync(DocumentFileModel model, CancellationToken cancellationToken = default)
    {
        var fileName = model.FileName.RemoveInvalidPathChars();
        return await _documentBLL.InsertDocumentAsync
            (fileName, model.Data, cancellationToken);
    }

    [HttpGet("is-local-save")]
    public async Task<bool> IsLocalSaveFilesAsync(CancellationToken cancellationToken)
    {
        return await _documentBLL.IsLocalSaveFilesAsync(cancellationToken);
    }

    [HttpGet("migrate")]
    public async Task MigrateFromDBToLocalSaveAsync(CancellationToken cancellationToken)
    {
        await _documentBLL.MigrateFromDBToLocalSaveAsync(cancellationToken);
    }

    
    private static string GetMimeTypeForFileExtension(string filePath)
    {
        const string DefaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out string contentType))
        {
            contentType = DefaultContentType;
        }
        return contentType;
    }
}
