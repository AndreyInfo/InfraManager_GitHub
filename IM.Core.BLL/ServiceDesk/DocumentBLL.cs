using AutoMapper;
using Inframanager;
using Inframanager.BLL;
using Inframanager.BLL.AccessManagement;
using InfraManager.BLL.Localization;
using InfraManager.BLL.Settings;
using InfraManager.Core.Exceptions;
using InfraManager.Core.Extensions;
using InfraManager.Core.Helpers;
using InfraManager.DAL;
using InfraManager.DAL.Documents;
using InfraManager.DAL.Settings;
using InfraManager.ResourcesArea;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace InfraManager.BLL.ServiceDesk;

internal class DocumentBLL : IDocumentBLL, ISelfRegisteredService<IDocumentBLL>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _saveChanges;
    private readonly ICurrentUser _currentUser;
    private readonly IRepository<Document> _documentRepository;
    private readonly IRepository<Setting> _settingRepository;
    private readonly IRepository<DocumentReference> _documentReferenceRepository;
    private readonly IValidatePermissions<Document> _validatePermissions;
    private readonly ILogger<DocumentBLL> _logger;
    private readonly ISettingsBLL _settingsBLL;
    private readonly DocumentsSettingsOptions _documentOptions;
    private readonly ILocalizeText _localizeText;
    private readonly SemaphoreSlim _deleteFileLock = new SemaphoreSlim(1, 1);
    private readonly IConvertSettingValue<string> _converter;


    public DocumentBLL(
                IMapper mapper,
                ICurrentUser currentUser,
                IUnitOfWork saveChanges,
                IRepository<Document> documentRepository,
                IRepository<Setting> settingRepository,
                IRepository<DocumentReference> documentReferenceRepository,
                IValidatePermissions<Document> validatePermissions,
                ILogger<DocumentBLL> logger,
                ISettingsBLL settingsBLL,
                IOptions<DocumentsSettingsOptions> confDocumentsOptions,
                ILocalizeText localizeText,
                IConvertSettingValue<string> converter
                )
    {
        _mapper = mapper;
        _currentUser = currentUser;
        _saveChanges = saveChanges;
        _documentRepository = documentRepository;
        _settingRepository = settingRepository;
        _documentReferenceRepository = documentReferenceRepository;
        _validatePermissions = validatePermissions;
        _logger = logger;
        _settingsBLL = settingsBLL;
        _documentOptions = confDocumentsOptions.Value;
        _localizeText = localizeText;
        _converter = converter;
    }

    public async Task DeleteObjectDocumentsAsync(Guid objectID, Guid? documentID, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.Delete, cancellationToken);

        if (documentID == default) throw new ArgumentNullException($"No ID provided for Document deletion");

        await _deleteFileLock.WaitAsync();
        try
        {

            using (var transaction = TransactionScopeCreator.Create(IsolationLevel.ReadCommitted, TransactionScopeOption.Required))
            {
                //Если есть связки с другими объектами, сам документ не удаляем
                if (_documentReferenceRepository.Query()
                                 .Any(x => x.ObjectID != objectID && x.DocumentID == documentID))
                {
                    throw new InvalidObjectException(await _localizeText.LocalizeAsync(nameof(Resources.ErrorLinkDocuments), cancellationToken));
                }

                var document = await _documentRepository.FirstOrDefaultAsync(x => x.ID == documentID.Value, cancellationToken);
                if (document is not null)
                    _documentRepository.Delete(document);
                await _saveChanges.SaveAsync(cancellationToken);

                if (await IsLocalSaveAsync(cancellationToken) && !string.IsNullOrEmpty(document.EncodedName))
                {
                    var filePath = Path.Combine(_documentOptions.FilePath, document.EncodedName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }


                transaction.Complete();
            }
        }
        finally
        {
            _deleteFileLock.Release();
        }
    }

    public async Task<DocumentInfoDetails[]> GetDocumentsDataAsync(Guid[] documentIds, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var documents = await _documentRepository.ToArrayAsync(x => documentIds.Contains(x.ID), cancellationToken);
        var resultDocuments = _mapper.Map<DocumentInfoDetails[]>(documents);

        for (int i = 0; i < resultDocuments.Count(); i++)
        {
            resultDocuments[i].Size = await GetSizeAsync(documents.FirstOrDefault(x=>x.ID == resultDocuments[i].Id), cancellationToken);
        }

        return resultDocuments;
    }

    private byte[] GetDataBytes(string filePath)
    {
        return File.ReadAllBytes(filePath);
    }

    public async Task<DocumentDataDetails> GetDocumentDataAsync(Guid documentID, CancellationToken cancellationToken)
    {
        await _validatePermissions.ValidateOrRaiseErrorAsync(_logger, _currentUser.UserId, ObjectAction.ViewDetails, cancellationToken);

        var document = await _documentRepository.FirstOrDefaultAsync(x => x.ID == documentID, cancellationToken)
            ?? throw new ObjectNotFoundException<Guid>(documentID, ObjectClass.Document);

        var resultDocument = _mapper.Map<DocumentDataDetails>(document);
        if (await IsLocalSaveAsync(cancellationToken) && !string.IsNullOrEmpty(document.EncodedName))
        {
            var filePath = Path.Combine(_documentOptions.FilePath, document.EncodedName);
            if (File.Exists(filePath))
            {
                resultDocument.Data = GetDataBytes(filePath);
            }
        }

        return resultDocument;
    }

    public async Task<DocumentInfoDetails> GetObjectDocumentAsync(Guid documentID, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.FirstOrDefaultAsync(x => x.ID == documentID, cancellationToken) ?? throw new ObjectNotFoundException($"Document '{documentID}' not found");
        var resultDocument = _mapper.Map<DocumentInfoDetails>(document);

        resultDocument.Size = await GetSizeAsync(document, cancellationToken);

        return resultDocument;
    }

    public async Task<DocumentInfoDetails[]> GetObjectDocumentsAsync(Guid objectID, CancellationToken cancellationToken)
    {
        var references = await _documentReferenceRepository.ToArrayAsync(x => x.ObjectID == objectID, cancellationToken);
        var documentIDs = references.Select(x => x.DocumentID).ToList();
        var documents = await _documentRepository.ToArrayAsync(x => documentIDs.Contains(x.ID), cancellationToken);

        DocumentInfoDetails[] resultDocuments = new DocumentInfoDetails[documents.Length];

        for (int i = 0; i < documents.Length; i++)
        {
            DocumentInfoDetails resultDocument = _mapper.Map<DocumentInfoDetails>(documents[i]);
            resultDocument.ObjectId = objectID;
            if (await IsLocalSaveAsync(cancellationToken) && !string.IsNullOrEmpty(documents[i].EncodedName))
            {
                var filePath = Path.Combine(_documentOptions.FilePath, documents[i].EncodedName);
                if (File.Exists(filePath))
                {
                    resultDocument.Size = (int)new System.IO.FileInfo(filePath).Length;
                }
            }
            else
            {
                resultDocument.Size = documents[i].Data.Length;
            }
            resultDocuments[i] = resultDocument;
        }

        return resultDocuments;
    }

    public async Task<Guid> InsertDocumentAsync(string fileName, byte[] data, CancellationToken cancellationToken)
    {
        IOHelper.ParseFileName(fileName, out string name, out string extension);

        if (!await IsValidFileExtensionAsync(extension, cancellationToken))
        {
            throw new ArgumentException(string.Format(Resources.ErrorBadExtension, extension));
        }

        var document = new Document(
            name: name.Truncate(_documentOptions.MaxNameLength),
            extension: extension.Truncate(_documentOptions.MaxExtensionLength),
            documentState: DocumentState.Available,
            authorID: _currentUser.UserId
            );


        if (await IsLocalSaveAsync(cancellationToken))
        {
            document.EncodedName = await SaveFileLocalAsync(data, cancellationToken);
        }
        else
        {
            document.Data = data;
        }

        _documentRepository.Insert(document);
        await _saveChanges.SaveAsync(cancellationToken);

        return document.ID;
    }

    private async Task<int> GetSizeAsync(Document document, CancellationToken cancellationToken)
    {
        if (await IsLocalSaveAsync(cancellationToken) && !string.IsNullOrEmpty(document.EncodedName))
        {
            var filePath = Path.Combine(_documentOptions.FilePath, document.EncodedName);
            if (File.Exists(filePath))
            {
                return (int)new System.IO.FileInfo(filePath).Length;
            }
        }
        return document.Data.Length;
    }
    private async Task<bool> IsLocalSaveAsync(CancellationToken cancellationToken)
    {
        bool isFirstLaunch = await IsFirstLaunchAsync(cancellationToken);
        bool isChangedSaveToLocal = await IsChangedSaveToLocalAsync(cancellationToken);

        if (isFirstLaunch && !isChangedSaveToLocal)
        {
            await SetSaveToLocalInDBAsync(cancellationToken);
            return true;
        }
        return isChangedSaveToLocal;
    }

    public async Task<bool> IsLocalSaveFilesAsync(CancellationToken cancellationToken)
    {
        bool isFirstLaunch = await IsFirstLaunchAsync(cancellationToken);
        bool isChangedSaveToLocal = await IsChangedSaveToLocalAsync(cancellationToken);

        return isChangedSaveToLocal || isFirstLaunch;
    }

    public async Task MigrateFromDBToLocalSaveAsync(CancellationToken cancellationToken)
    {
        using var transaction =
            new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
                TransactionScopeAsyncFlowOption.Enabled);
        await SetSaveToLocalInDBAsync(cancellationToken);

        var documents = await _documentRepository.ToArrayAsync(cancellationToken);

        foreach (var document in documents)
        {
            if (document.Data?.Length > 0)
            {
                document.EncodedName = await SaveFileLocalAsync(document.Data, cancellationToken);
                document.Data = null;
            }
        }

        await _saveChanges.SaveAsync(cancellationToken);
        transaction.Complete();
    }

   

    private async Task<bool> IsChangedSaveToLocalAsync(CancellationToken cancellationToken)
    {
        var setting = await _settingRepository.FirstOrDefaultAsync(x => x.Id == SystemSettings.UpdateToLocalStorage, cancellationToken);
        return setting is null ? false : setting.Value[0] == 1;
    }

    private async Task<string> SaveFileLocalAsync(byte[] data, CancellationToken cancellationToken)
    {
        var trustedFileNameForFileStorage = Path.GetRandomFileName();

        using (var targetStream = System.IO.File.Create(
        Path.Combine(_documentOptions.FilePath, trustedFileNameForFileStorage)))
        {
            await targetStream.WriteAsync(data, cancellationToken);
        }

        return trustedFileNameForFileStorage;
    }
    private async Task SetSaveToLocalInDBAsync(CancellationToken cancellationToken)
    {
        await _settingsBLL.SetAsync(SystemSettings.UpdateToLocalStorage, new WebApi.Contracts.Settings.SettingData(true), cancellationToken);
    }

    private async Task<bool> IsFirstLaunchAsync(CancellationToken cancellationToken)
    {
        var count = await _documentRepository.CountAsync(cancellationToken);
        return count == 0;
    }

    private async Task<bool> IsValidFileExtensionAsync(string extension, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(extension))
        {
            return false;
        }

        var setting = await _settingsBLL.GetValueAsync(SystemSettings.AllowExtensionsForFiles, cancellationToken);
        if (setting == null)
        {
            return true;
        }

        var extensionsString = _converter.Convert(setting);
        extensionsString = new string(extensionsString.ToCharArray()
                          .Where(c => !c.Equals('.') && !Char.IsWhiteSpace(c))
                          .ToArray());
        if (string.IsNullOrEmpty(extensionsString))
        {
            return true;
        }

        var extensions = extensionsString.Split(',');

        return extensions.Contains(extension);
    }
}
