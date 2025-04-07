using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using InfraManager.BLL.FormBuilder.Contracts;
using InfraManager.DAL;
using InfraManager.DAL.FormBuilder;
using Microsoft.Extensions.Caching.Memory;

namespace InfraManager.BLL.FormBuilder;

internal class FormBuilderSettingsBLL : IFormBuilderSettingsBLL, ISelfRegisteredService<IFormBuilderSettingsBLL>
{
    private readonly IReadonlyRepository<Form> _forms;
    private readonly IRepository<FormFieldSettings> _fieldSettings;
    private readonly IUnitOfWork _saveChanges;
    private readonly ICurrentUser _currentUser;
    private readonly IMemoryCache _memoryCache;
    private readonly IMapper _mapper;

    public FormBuilderSettingsBLL(
        IReadonlyRepository<Form> forms,
        IRepository<FormFieldSettings> fieldSettings,
        IUnitOfWork saveChanges,
        ICurrentUser currentUser,
        IMemoryCache memoryCache,
        IMapper mapper)
    {
        _forms = forms;
        _fieldSettings = fieldSettings;
        _saveChanges = saveChanges;
        _currentUser = currentUser;
        _memoryCache = memoryCache;
        _mapper = mapper;
    }

    public async Task<FormBuilderFormSettingDetails> GetSettingsAsync(Guid formID, CancellationToken cancellationToken = default)
    {
        return await _memoryCache.GetOrCreateAsync(
            GetCacheKey(formID),
            async _ => new FormBuilderFormSettingDetails
            {
                FieldSettings = _mapper.Map<FormBuilderFormFieldSettingDetails[]>(
                    await GetOrCreateDefaultSettingsAsync(formID, cancellationToken)),
            });
    }

    public async Task SaveSettingsAsync(Guid formID, FormBuilderFormSettingData data, CancellationToken cancellationToken = default)
    {
        var newSettings = data.FieldSettings
            .GroupBy(x => x.FieldID)
            .ToDictionary(x => x.Key, x => x.First().Width);

        foreach (var setting in await GetOrCreateDefaultSettingsAsync(formID, cancellationToken))
        {
            if (newSettings.TryGetValue(setting.FieldID, out var width) && setting.Width != width)
            {
                setting.Width = width;
            }

            if (setting.ID == default)
            {
                _fieldSettings.Insert(setting);
            }
        }

        await _saveChanges.SaveAsync(cancellationToken); 

        _memoryCache.Remove(GetCacheKey(formID));
    }

    private async Task<IEnumerable<FormFieldSettings>> GetOrCreateDefaultSettingsAsync(Guid formID, CancellationToken cancellationToken)
    {
        var form = await _forms
                       .WithMany(frm => frm.FormTabs)
                       .ThenWithMany(tab => tab.Fields.Where(field => field.Type == FieldTypes.Table && field.ColumnFieldID == null && field.GroupFieldID == null))
                       .ThenWithMany(field => field.Columns)
                       .FirstOrDefaultAsync(frm => frm.ID == formID, cancellationToken)
                   ?? throw new ObjectNotFoundException("Form template not found.");

        var fields = form.FormTabs
            .SelectMany(tab => tab.Fields)
            .Where(field => field.Type == FieldTypes.Table)
            .SelectMany(field => field.Columns)
            .ToArray();

        var columnFieldIDs = fields.Select(x => x.ID).ToArray();

        var currentSettings = (await _fieldSettings
                .ToArrayAsync(x => x.UserID == _currentUser.UserId
                                   && columnFieldIDs.Contains(x.FieldID)
                    , cancellationToken)
            ).ToDictionary(column => column.FieldID, column => column);

        return fields.Select(field =>
            currentSettings.TryGetValue(field.ID, out var current)
                ? current
                : new FormFieldSettings
                {
                    UserID = _currentUser.UserId,
                    FieldID = field.ID,
                    Width = 120, // todo: Вместо 120 возвращать ширину колонки из шаблона формы когда будет реализовано
                });
    }

    private string GetCacheKey(Guid formID)
    {
        return $"FormSettings_{_currentUser.UserId}_{formID}";
    }
}