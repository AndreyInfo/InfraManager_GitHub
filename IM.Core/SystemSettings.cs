using System;

namespace InfraManager;

public enum SystemSettings : int
{
    #region cord
    /// <summary>
    /// bool
    /// Подключение корда только в рамках комнаты
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(bool))]
    ConnectCordInOneRoom = 1,
    #endregion

    #region asset: unique i/n
    /// <summary>
    /// bool
    /// Контроль уникальности инвентарных номеров
    /// </summary>      
	[SystemSettingTypeMappingAttribute(typeof(bool))]
    UniqueInvNumber = 2,
    #endregion

    #region asset: inquiry
    /// <summary>
    /// byte
    /// Опрос - создавать устройства как ОО/СО/диалог выбора
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(byte))]
    InquiryAddMode = 3,

    /// <summary>
    /// bool
    /// Позволять аудиту обновление устройств устаревшими данными
    /// </summary>
	[SystemSettingTypeMappingAttribute(typeof(bool))]
    AuditAllowUpdateByStaleData = 4,
    #endregion

    #region asset: unique s/n
    /// <summary>
    /// bool
    /// Контроль уникальности серийных номеров
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    UniqueSerialNumber = 5,
    #endregion

    #region asset: inquiry default auditRuleSet
    /// <summary>
    /// string
    /// Идентификатор AuditRuleSet.ID набора правил сопоставлений
    /// для неразмещенного оборудования и для computerInfo
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AuditRuleSetID = 6,

    /// <summary>
    /// string
    /// Идентификатор AuditRuleSet.ID набора правил сопоставлений
    /// для неразмещенного оборудования и для computerInfo
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(string))]
    AuditRuleSetSubdeviceID = 96,
    #endregion

    #region service desk
    /// <summary>
    /// bool 
    /// SD бюджет задания это статья бюджета, либо ссылка на орг. единицу
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    BudgetMode = 7,

    /// <summary>
    /// bool
    /// SD предупреждать при нарушении SLA
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    NotifyOnCallPromiseDateViolation = 8,

    /// <summary>
    /// long
    /// SD интервал обещанного времени решения заяки, если нет SLA
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(long))]
    CallPromiseDateDelta = 9,

    /// <summary>S
    /// long
    /// SD интервал обещанного завершения задания
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(long))]
    WorkOrderFinishDateDelta = 10,

    /// <summary>
    /// bool
    /// SD предупреждать при нарушении срока выполнения задания
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    NotifyOnWorkOrderFinishDateViolation = 11,

    /// <summary>
    /// string
    /// Адрес Web сервера SD
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    WebServerAddress = 12,
    #endregion

    #region asset: monitoring
    /// <summary>
    /// string
    /// communityName по умолчанию для устройств
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultCommunityName = 13,

    /// <summary>
    /// int
    /// Интервал опроса по умолчанию для мониторинга
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    DeviceMonitorIntervisitInterval = 14,

    /// <summary>
    /// int
    /// Период хранения по умолчанию для мониторинга
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    DeviceMonitorPeriodOfStorage = 15,

    /// <summary>
    /// bool
    /// Автоматически создавать показатели мониторинга
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoCreateDeviceMonitor = 16,
    #endregion

    #region asset: message events
    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что периферия пропала из устройства
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnPeripheralLosted = 17,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    PeripheralLostedText = 18,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    PeripheralLostedSeverityType = 19,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что найдено новая периферия в устройстве
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnPeripheralDetected = 20,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    PeripheralDetectedText = 21,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    PeripheralDetectedSeverityType = 22,
    #endregion

    #region man hours type
    /// <summary>
    /// bool
    /// Тип трудозатрат
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    ManHoursValueType = 23,
    #endregion

    #region service desk: problem

    /// <summary>
    /// long
    /// SD интервал обещанного завершения проблемы
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(long))]
    ProblemPromiseDateDelta = 24,

    /// <summary>
    /// bool
    /// SD предупреждать при нарушении срока выполнения проблемы
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    NotifyOnProblemPromiseDateViolation = 25,

    #endregion

    #region asset: message events

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что обнаружено новое устройство
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnDeviceDetected = 26,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DeviceDetectedText = 27,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    DeviceDetectedSeverityType = 28,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что IP адрес устройства изменился
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnDeviceIPAddressChanged = 29,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DeviceIPAddressChangedText = 30,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    DeviceIPAddressChangedSeverityType = 31,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что сетевое имя устройства изменилось
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnDeviceNameChanged = 32,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DeviceNameChangedText = 33,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    DeviceNameChangedSeverityType = 34,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что адаптер пропал из устройства
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnAdapterLosted = 35,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AdapterLostedText = 36,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    AdapterLostedSeverityType = 37,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что найден новый адаптер в устройстве
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnAdapterDetected = 38,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AdapterDetectedText = 39,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    AdapterDetectedSeverityType = 40,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что найдена новая инсталляция ПО в устройстве
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnSoftwareInstallationDetected = 41,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    SoftwareInstallationDetectedText = 42,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    SoftwareInstallationDetectedSeverityType = 43,

    /// <summary>
    /// bool
    /// Создать сообщение, сли аудит считает, что найдена новая обновление ПО в устройстве
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateMessageOnSoftwareUpdateDetected = 44,
    /// <summary>
    /// string
    /// Описание сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    SoftwareUpdateDetectedText = 45,
    /// <summary>
    /// byte
    /// Серьезность сообщения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    SoftwareUpdateDetectedSeverityType = 46,
    #endregion

    #region service desk: workflow scheme

    /// <summary>
    /// string
    /// Идентификатор рабочей процедуры для заявки по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultCallWorkflowSchemeIdentifier = 47,

    /// <summary>
    /// string
    /// Идентификатор рабочей процедуры для задания по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultWorkOrderWorkflowSchemeIdentifier = 48,

    /// <summary>
    /// string
    /// Идентификатор рабочей процедуры для проблемы по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultProblemWorkflowSchemeIdentifier = 49,

    #endregion

    #region report period type (time management)
    /// <summary>
    /// int
    /// тип отчетного периода
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    ReportPeriodType = 50,
    #endregion

    #region asset: auto allocation
    /// <summary>
    /// bool
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// автоматически размещать оборудование на рабочем месте пользователя (по логину)
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoAllocateDeviceAtUserWorkplace = 51,

    /// <summary>
    /// string
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// автоматически размещать оборудование в местоположении с идентификатором
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AutoAllocateDeviceLocationID = 52,

    /// <summary>
    /// string
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// модель по умолчанию для оконечного оборудования при автоматическом размещении
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AutoAllocateDeviceDefaultTerminalDeviceModelID = 53,
    #endregion

    #region service desk: web kb serarch
    /// <summary>
    /// bool
    /// опция включающая/отключающая использование поиска по БЗ при регистрации заявок в вебе
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    SearchKBDuringRegisteringNewCall = 54,
    #endregion

    #region messages: rules
    /// <summary>
    /// Правила обработки сообщения подсистемы электронной почты 
    /// </summary>
    /// <remarks>
    /// byte[]
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte[]))]
    MessageByEmailRuleSet = 55,
    /// <summary>
    /// Правила обработки сообщения подсистемы опроса оборудования 
    /// </summary>
    /// <remarks>
    /// byte[]
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte[]))]
    MessageByInquiryRuleSet = 56,
    /// <summary>
    /// Правила обработки сообщения подсистемы мониторинга 
    /// </summary>
    /// <remarks>
    /// byte[]
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte[]))]
    MessageByMonitoringRuleSet = 57,

    #endregion

    #region asset: cheapness
    /// <summary>
    /// Настройки обработки мало-ценных объектов
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(byte[]))]
    Cheapness = 58,
    #endregion

    #region asset: number generation
    /// <summary>
    /// Префикс инвентарного номера
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    InventoryNumber_Prefix = 59,

    /// <summary>
    /// Длина значения инвентарного номера
    /// </summary>
    /// <remarks>
    /// int32
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    InventoryNumber_Length = 60,

    /// <summary>
    /// Текущее значение (которое будет следующим) для инвентарного номера
    /// </summary>
    /// <remarks>
    /// int32
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    InventoryNumber_Value = 61,

    /// <summary>
    /// Префикс кода
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    Code_Prefix = 62,

    /// <summary>
    /// Длина значения кода
    /// </summary>
    /// <remarks>
    /// int32
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    Code_Length = 63,

    /// <summary>
    /// Текущее значение (которое будет следующим) для кода
    /// </summary>
    /// <remarks>
    /// int32
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    Code_Value = 64,
    #endregion

    #region asset: asset operion and reports
    /// <summary>
    /// Список пар (OperationType, идентификатор отчета)
    /// </summary>
    /// <remarks>
    /// byte[]
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte[]))]
    AssetOperationReports = 65,
    #endregion

    #region asset: unique code
    /// <summary>
    /// bool
    /// Контроль уникальности поля код
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(bool))]
    UniqueCode = 66,
    #endregion

    #region service desk: create email by call
    /// <summary>
    /// Шаблон оповещения для команды "Отправить по Email" заявки
    /// </summary>
    /// <remarks>
    /// Guid
    /// </remarks>
	[SystemSettingTypeMappingAttribute(typeof(Guid?))]
    CallEmailTemplateID = 67,
    #endregion

    #region manhours in closed objects
    /// <summary>
    /// bool
    /// Редактирование трудозатрат закрытых объектов
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    ManHoursInClosed = 68,
    #endregion

    #region messages: rules
    /// <summary>
    /// Правила обработки сообщения подсистемы интеграции 
    /// </summary>
    /// <remarks>
    /// byte[]
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte[]))]
    MessageByIntegrationRuleSet = 69,
    #endregion

    #region service desk: support line
    /// <summary>
    /// Количество линий поддержки в системе, по умолчанию = 1
    /// </summary>
    /// <remarks>
    /// byte
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    SupportLineCount = 70,
    #endregion

    #region service desk: negotiation start message template
    /// <summary>
    /// Шаблон почтового сообщения о начале согласования
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
	[SystemSettingTypeMapping(typeof(Guid?))]
    NegotiationStartMessageTemplate = 71,
    #endregion

    #region asset: auto allocation
    /// <summary>
    /// int
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// автоматически размещать оборудование в местоположении с классом
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    AutoAllocateDeviceLocationClassID = 72,

    /// <summary>
    /// bool
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// автоматически размещать оборудование: использовать информацию о подсетях в зданиях
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoAllocateDeviceUseBuildingSubnet = 73,

    /// <summary>
    /// bool
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// перемещать рабочее место пользователя в комнату, указанную в исходных данных опроса как местоположение оборудования
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoAllocateDeviceMoveUserWorkplaceToDeviceLocation = 74,

    /// <summary>
    /// string
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// модель по умолчанию для сетевого оборудования при автоматическом размещении
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AutoAllocateDeviceDefaultNetworkDeviceModelID = 75,

    /// <summary>
    /// bool
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// автоматически создавать модели ОО и СО без портов и слотов, если таковых нет в БД
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoAllocateDeviceCreateModel = 76,

    /// <summary>
    /// bool
    /// только для неразмещенного оборудования, в задаче по опросу индивидуальные настройки
    /// авторазмещение: автоматически создавать этаж Этаж и комнату Неизвестно
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoAllocateCreateFloorAndUnknownRoom = 77,
    #endregion

    #region asset: deviations
    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что обнаружено новое оборудование
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnNewDeviceDetected = 78,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что обнаружено новое устройство
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationWithSubdeviceDetected = 79,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что местоположение оборудования изменилось
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnMoveDevice = 80,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что найдена новая инсталляция
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnNewSoftwareDetected = 81,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что найдена новое обоновление
    /// </summary>
    [SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnNewSoftwareUpdateDetected = 82,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что обнаружено изменение сетевого имени
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnDeviceNameChanged = 83,

    /// <summary>
    /// bool
    /// Создать отклонение, сли аудит считает, что обнаружено изменение ip адреса
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnDeviceIPChanged = 84,
    #endregion

    #region service desk: negotiator deleted message template
    /// <summary>
    /// Шаблон почтового сообщения об удалении участника согласования
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
	[SystemSettingTypeMapping(typeof(Guid?))]
    NegotiatorDeleteMessageTemplate = 85,
    #endregion

    #region service desk: client Call registration message template
    /// <summary>
    /// Шаблон почтового сообщения о регистрации заявки клиента
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    ClientCallRegistrationMessageTemplate = 86,
    #endregion

    #region messages: rules
    /// <summary>
    /// Использовать правила удаления цитирования 
    /// </summary>
    /// <remarks>
    /// bool
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CitateTrimmerUsing = 87,
    #endregion

    #region service desk: timeManagement
    /// <summary>
    /// Правила обработки сводного сообщения подсистемы опроса 
    /// </summary>
    /// <remarks>
    /// bool
    /// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoTimeSheetSetStateToReview = 88,
    #endregion

    #region service desk: call classification
    /// <summary>
    /// bool
    /// Обновлять состав параметров каждый раз при смене элемента/услуги у заявки
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    RecalculateCallAdditionalParametersWithServiceChange = 89,
    #endregion
    #region service desk: problem classification
    /// <summary>
    /// bool
    /// Обновлять состав параметров каждый раз при смене типа проблемы
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    RecalculateProblemAdditionalParametersWithProblemTypeChange = 97,
    #endregion

    #region service desk: call classification
    /// <summary>
    /// bool
    /// Тиражирование значений имущественных полей из оборудования в устройства (состояние, место, использует, владеет, мол)
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CopyAssetFieldsFromDeviceToSubdevice = 90,

    /// <summary>
    /// string
    /// Идентификатор жизненного цикла по цмолчанию для опроса
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultLifeCycleID = 91,
    #endregion

    #region searchBox: search result count
    /// <summary>
    /// int
    /// Количество результатов поиска в искалках
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    ObjectSearcherResultCount = 92,
    #endregion

    #region use TTZ
    /// <summary>
    /// bool
    /// Использовать ТТЗ
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    UseTTZ = 93,
    #endregion

    #region service desk: call, add dependency
    /// <summary>
    /// int
    /// Заявка, добавление связей - режим по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(int))]
    CallAddDependencyMode = 94,
    #endregion

    #region service desk: call, add dependency
    /// <summary>
    /// byte
    /// Отсчитывать обещанное время решени от (перечисление)
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(byte))]
    CallPromiseDateCalculationMode = 95,
    #endregion
    //
    #region service desk: add controllers
    /// <summary>
		/// Шаблон почтового сообщения о добавлении контролеров
		/// </summary>
		/// <remarks>
		/// string
		/// </remarks>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    AddCustomControllers = 98,
    #endregion

    #region service desk: del controllers
    /// <summary>
    /// byte
    /// Шаблон почтового сообщения об исключении контролеров
    /// </summary>
	[SystemSettingTypeMapping(typeof(Guid?))]
    DeleteCustomControllers = 99,
    #endregion

    #region license management: pool licensing rule

    /// <summary>
    /// bool
    /// Правило выдачи лицензий из пула (FIFO / LIFO)
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    LicensePoolDistributionRule = 100,

    #endregion

    #region license management: turn license ditribution centers usage on

    /// <summary>
    /// bool
    /// Признак включения ЦРПО
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    DistributionCenterTurnedOn = 101,

    #endregion

    #region service desk: negotiation comment
    /// <summary>
    /// Комментарии обязательны при голосовании За
    /// bool
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CommentPlacet = 102,
    #endregion

    #region service desk: negotiation not comment
    /// <summary>
    /// Комментарии обязательны при голосовании За
    /// bool
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CommentNonPlacet = 103,
    #endregion

    #region asset: deviations network units
    /// <summary>
    /// bool
    /// Создать отклонение, если аудит считает, что обнаружен новый узел сети
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnNewNetworkUnitDetected = 104,

    /// <summary>
    /// bool
    /// Создать отклонение, если аудит считает, что местоположение узла сети изменилось
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CreateAssetDeviationOnMoveNetworkUnit = 105,
    #endregion

    #region service desk: rfc
    /// <summary>
    /// long
    /// SD интервал обещанного завершения запроса на изменения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(long))]
    RFCPromiseDateDelta = 106,

    /// <summary>
    /// bool
    /// SD предупреждать при нарушении срока выполнения запроса на изменения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    NotifyOnRFCPromiseDateViolation = 107,
    #endregion

    #region service desk: rfc classification
    /// <summary>
    /// bool
    /// Обновлять состав параметров каждый раз при смене типа запроса на изменения
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    RecalculateRFCAdditionalParametersWithRFCTypeChange = 108,
    #endregion

    #region service desk: workflow scheme
    /// <summary>
    /// string
    /// Идентификатор рабочей процедуры для заявки по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultRFCWorkflowSchemeIdentifier = 109,
    #endregion

    #region asset: network units
    /// <summary>
    /// string
    /// Тип КЕ для оконечного оборудования по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultNetworkUnitTypeForTerminalDeviceID = 110,

    /// <summary>
    /// string
    /// Тип КЕ для сетевого оборудования по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultNetworkUnitTypeForNetworklDeviceID = 111,

    /// <summary>
    /// string
    /// Тип КЕ для логических объектов по умолчанию
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(string))]
    DefaultNetworkUnitTypeForLogicalDeviceID = 112,

    /// <summary>
    /// bool
    /// Создавать узлы сети автоматически
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    AutoCreateNetworkUnit = 113,
    #endregion

    #region service desk: add substitution
    /// <summary>
    /// Шаблон почтового сообщения о назначении заместителем 
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
	[SystemSettingTypeMapping(typeof(Guid?))]
    AddSubstitution = 114,
    #endregion

    #region service desk: del substitution
    /// <summary>
    /// byte
    /// Шаблон почтового сообщения об исключении заместителя
    /// </summary>
    [SystemSettingTypeMapping(typeof(Guid?))]
    DeleteSubstitution = 115,
    #endregion

    #region service desk: change dates substitution 
    /// <summary>
    /// Шаблон почтового сообщения о изменении дат замещения 
    /// </summary>
    /// <remarks>
    /// string
    /// </remarks>
	[SystemSettingTypeMapping(typeof(Guid?))]
    ChangeDatesSubstitution = 116,
    #endregion

    #region service desk: hide  plase of location substitution 
    /// <summary>
    /// Скрывать поле Место оказания сервиса
    /// bool
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    CallHidePlaceOfService = 117,
    #endregion

    #region service desk: obligation to allocate a budget
    /// <summary>
    /// Обязательность выделения бюджета
    /// bool
    /// </summary>
		[SystemSettingTypeMappingAttribute(typeof(bool))]
    ObligationBudget = 118,
    #endregion

    #region service desk: сhange graph size
    /// <summary>
    /// Размер графика изменений
    /// </summary>
    /// <remarks>
    /// int32
    /// </remarks>
	[SystemSettingTypeMappingAttribute(typeof(int))]
    SizeProject = 119,
    #endregion
    
    #region session controll
    [SystemSettingTypeMapping(typeof(int))]
    InactiveUserTime = 126,
    
    [SystemSettingTypeMapping(typeof(int))]
    CacheSessionCheckTime = 127,
    
    [SystemSettingTypeMapping(typeof(int))]
    PeriodicTimeCheck = 128,
	#endregion
    //
    //TODO find out converters for next values:
    //
    #region service desk: сhange message delete time
    [SystemSettingTypeMappingAttribute(typeof(int))]
    UserImportSetting = 120,
    [SystemSettingTypeMappingAttribute(typeof(int))]
    PostageMessageSetting = 121,
    [SystemSettingTypeMappingAttribute(typeof(int))]
    ImportEquipmentAndNetworkRequestSetting = 122,
    [SystemSettingTypeMappingAttribute(typeof(int))]
    MonitoringMessageSetting = 123,
    #endregion

    #region service desk: Mail Service Settings
    
    MailServiceSetting = 124,
    #endregion

    #region massive incidents: default workflowscheme
    /// <summary>
    /// Рабочая процедура по умолчанию для массовых инцидентов
    /// </summary>
    [SystemSettingTypeMapping(typeof(string))]
    DefaultMassIncidentsWorkflowSchemeIdentifier = 125,
    #endregion





    #region massive incidents: recalculate additional parameters
    /// <summary>
    /// bool
    /// Обновлять кастомные параметры каждый раз при смене типа
    /// </summary>
    [SystemSettingTypeMapping(typeof(bool))]
    RecalculateMassIncidentsAdditionalParametersWithTypeChange = 129,
    #endregion

    #region massive incidents: calculation mode
    /// <summary>
    /// В зависимости от выбранного значения срок "Закрыть до" 
    /// должен рассчитываться с учетом графика рабочего времени от соответствующей даты.
    /// </summary>
    [SystemSettingTypeMapping(typeof(byte))]
    MassIncidentsDateCalculationMode = 130,
    #endregion

    #region massive incidents: date delta
    /// <summary>
    /// В случае, если у массового инцидента не подобрано SLA поле "Закрыть до" 
    /// должно рассчитываться с учетом заданного тут количества часов и минут по графику рабочего времени по умолчанию.
    /// </summary>
    [SystemSettingTypeMapping(typeof(long))]
    MassIncidentsDateDelta = 131,
    #endregion

    #region update to local storage    
    /// <summary>
    /// Если пользователь через настройки переключил на локальное сохранение, то будет 1.
    /// </summary>
    [SystemSettingTypeMapping(typeof(bool))]
    UpdateToLocalStorage = 132,
    #endregion

    #region allow extensions for save files    
    /// <summary>
    /// Доступные расширения для сохранения файлов. Пример: .txt, .svg
    /// </summary>
    [SystemSettingTypeMapping(typeof(string))]
    AllowExtensionsForFiles = 133,
    #endregion

    #region Knowledge article Base

    [SystemSettingTypeMapping(typeof(int))]
	KnowledgeArticleBasePerPage = 134,
	
	[SystemSettingTypeMapping(typeof(Guid?))]
	DefaultKnowledgeArticleBase = 135
    #endregion
}
