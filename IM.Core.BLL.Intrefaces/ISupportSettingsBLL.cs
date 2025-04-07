using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfraManager.BLL.Settings;

namespace InfraManager.BLL;

/// <summary>
/// Интерфейс для работы с настройками службы поддержки
/// </summary>
public interface ISupportSettingsBll
{
    #region Общие методы настроек

    /// <summary>
    /// Получение настройки с заранее известным типом
    /// </summary>
    /// <typeparam name="T">тип настройки</typeparam>
    /// <param name="setting">ид настройки</param>
    /// <returns></returns>
    T GetSetting<T>(SystemSettings setting);

    /// <summary>
    /// Обновление настройки
    /// </summary>
    /// <param name="setting">ид настройки</param>
    /// <param name="value">новое значение настройки</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateSettingAsync(SystemSettings setting, object value, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление нескольких настроек
    /// </summary>
    /// <param name="settingValueDict">словарь {ид настройки}:{новое значение настройки}</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateFewSettingsAsync(Dictionary<SystemSettings, object> settingValueDict,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение нескольких настроек с заранее известными типами
    /// </summary>
    /// <param name="settingValueDict">словарь {ид настройки}:{заранее известный тип настройки}</param>
    /// <returns></returns>
    Dictionary<SystemSettings, dynamic> GetFewSettingsAsync(Dictionary<SystemSettings, Type> settingValueDict);

    #endregion

    #region Методы получения общих настроек

    /// <summary>
    /// Получение общих настроек
    /// </summary>
    /// <returns></returns>
    SupportSettingsGeneralModel GetGeneralSettings();

    /// <summary>
    /// Обновление общих настроек
    /// </summary>
    /// <param name="supportSettingsGeneralModel">модель общих настроек</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateGeneralSettings(SupportSettingsGeneralModel supportSettingsGeneralModel,
        CancellationToken cancellationToken = default);

    #endregion

    #region Методы получение настроек заданий

    /// <summary>
    /// Получение настроек заданий
    /// </summary>
    /// <returns></returns>
    SupportSettingsTasksModel GetTasksSettings();

    /// <summary>
    /// Обновление настроек заданий
    /// </summary>
    /// <param name="supportSettingsGeneralModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateTasksSettings(SupportSettingsTasksModel supportSettingsGeneralModel,
        CancellationToken cancellationToken = default);

    #endregion

    #region Методы получения настроек проблем

    /// <summary>
    /// Получение настроек проблем
    /// </summary>
    /// <returns></returns>
    SupportSettingsProblemsModel GetProblemsSettings();

    /// <summary>
    /// Обновление настроек проблем
    /// </summary>
    /// <param name="supportSettingsProblemsModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateProblemsSettings(SupportSettingsProblemsModel supportSettingsProblemsModel,
        CancellationToken cancellationToken = default);

    #endregion

    #region Методы получения настроек соглашений

    /// <summary>
    /// Получение настроек соглашений
    /// </summary>
    /// <returns></returns>
    SupportSettingsAgreementsModelDetails GetAgrementSettings();

    /// <summary>
    /// Обновление настроек соглашений
    /// </summary>
    /// <param name="model"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateAgreementSettingsAsync(SupportSettingsAgreementsModelDetails model,
        CancellationToken cancellationToken = default);

    #endregion

    #region Методы для получения настроек заявок

    /// <summary>
    /// Поллучение настроек заявок
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<SupportSettingsRequestsModel> GetRequestsSettingsAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Обновление настроек заявок 
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpdateRequestsSettingsAsync(SupportSettingsRequestsModel settings, CancellationToken cancellationToken);

    #endregion

    #region Методы для получения настроек уведомлений
    
    /// <summary>
    /// Получение шаблонов уведомлений на согласовании
    /// </summary>
    SpecialNotificationOnAgreement GetNotificationOnAgreement();

    /// <summary>
    /// Обновление шаблонов уведомлений на согласовании
    /// </summary>
    /// <param name="specialNotification">Форма с Id шаблонов</param>
    /// <param name="cancellationToken"></param>
    Task UpdateNotificationOnAgreementAsync(SpecialNotificationOnAgreement specialNotification,
        CancellationToken cancellationToken);

    /// <summary>
    /// Получение шаблонов уведомлений при контроле
    /// </summary>
    SpecialNotificationOnControl GetNotificationOnControl();
    
    
    /// <summary>
    /// Обновление шаблонов уведомлений при контроле
    /// </summary>
    /// <param name="specialNotification">Форма с Id шаблонов</param>
    /// <param name="cancellationToken"></param>
    Task UpdateNotificationOnControlAsync(SpecialNotificationOnControl specialNotification,
        CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение шаблонов уведомлений при замещении
    /// </summary>
    SpecialNotificationOnReplacement GetNotificationOnReplacement();

    /// <summary>
    /// Обновление шаблонов уведомлений при замещении
    /// </summary>
    /// <param name="specialNotification">Форма с Id шаблонов</param>
    /// <param name="cancellationToken"></param>
    Task UpdateNotificationOnReplacementAsync(SpecialNotificationOnReplacement specialNotification, CancellationToken cancellationToken);
    
    /// <summary>
    /// Получение шаблонов уведомлений по заявкам
    /// </summary>
    SpecialNotificationOnRequest GetNotificationOnRequest();
    
    
    /// <summary>
    /// Обновление шаблонов уведомлений по заявкам
    /// </summary>
    /// <param name="specialNotification">Форма с Id шаблонов</param>
    /// <param name="cancellationToken"></param>
    Task UpdateNotificationOnRequestAsync(SpecialNotificationOnRequest specialNotification, CancellationToken cancellationToken);
    
    #endregion


    
}