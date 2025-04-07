using System;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.FormBuilder.Contracts;

namespace InfraManager.BLL.FormBuilder;

/// <summary>
/// Управление настройками форм доп. параметров.
/// </summary>
public interface IFormBuilderSettingsBLL
{
    /// <summary>
    /// Получить настройки формы доп. параметров асинхронно.
    /// </summary>
    /// <param name="formID">Уникальный идентификатор формы.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    /// <returns>Экземпляр <see cref="FormBuilderFormSettingDetails"/>, предоставляющий настройки формы.</returns>
    Task<FormBuilderFormSettingDetails> GetSettingsAsync(Guid formID, CancellationToken cancellationToken = default);
        
    /// <summary>
    /// Сихранить настройки формы доп. параметров асинхронно.
    /// </summary>
    /// <param name="formID">Уникальный идентификатор формы.</param>
    /// <param name="data">Предоставляет настройки.</param>
    /// <param name="cancellationToken">Токен отмены асинхронной операции.</param>
    Task SaveSettingsAsync(Guid formID, FormBuilderFormSettingData data, CancellationToken cancellationToken = default);
}