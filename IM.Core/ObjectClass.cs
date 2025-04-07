namespace InfraManager
{
    /// <summary>
    /// Классы объектов
    /// </summary>
    public enum ObjectClass
    {
        /// <summary>
        /// Не известно
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Здание
        /// </summary>
        Building = 1,

        /// <summary>
        /// Этаж
        /// </summary>
        Floor = 2,

        /// <summary>
        /// Комната
        /// </summary>
        Room = 3,

        /// <summary>
        /// Шкаф
        /// </summary>
        Rack = 4,

        /// <summary>
        /// Активное устройство
        /// NetworkDevice
        /// </summary>
        ActiveDevice = 5,

        /// <summary>
        /// Оконечное оборудование
        /// </summary>
        TerminalDevice = 6,

        /// <summary>
        /// Конкретный пользователь
        /// </summary>
        User = 9,

        /// <summary>
        /// Тип работы
        /// </summary>
        UserActivityType = 12,

        /// <summary>
        /// Порт объекта Сетевое оборудование
        /// </summary>
        ActivePort = 13,

        /// <summary>
        /// Трудозатраты
        /// </summary>
        ManhoursWork = 18,

        /// <summary>
        /// Рабочее место
        /// </summary>
        Workplace = 22,

        /// <summary>
        /// Шаблон слота
        /// </summary>
        SlotTemplate = 26,

        /// <summary>
        /// Модуль для слота
        /// </summary>
        Slot = 27,

        /// <summary>
        /// Роль
        /// </summary>
        Role = 28,

        /// <summary>
        /// Владелец
        /// </summary>
        Owner = 29,
        /// <summary>
        /// Каталог продуктов
        /// </summary>
        ProductCatalogue = 30,
        /// <summary>
        /// Экземпляр адаптера
        /// </summary>
        Adapter = 33,

        /// <summary>
        /// Экземпляр периферийного оборудования
        /// </summary>
        Peripherial = 34,
        /// <summary>
        /// Расходный материал картриджей
        /// </summary>
        MaterialCartridge = 36,
        /// <summary>
        /// Модель лицензии программного обеспечения
        /// </summary>
        SoftwareLicenseModel = 38,

        /// <summary>
        /// Порт для объекта Адаптер
        /// </summary>
        PortAdapter = 39,

        /// <summary>
        /// Инсталляция
        /// </summary>
        SoftwareInstallation = 71,

        /// <summary>
        /// Вид среды передачи
        /// </summary>
        Medium = 79,

        /// <summary>
        /// Имущество
        /// </summary>
        Asset = 80,

        /// <summary>
        /// Тип разъема
        /// </summary>
        ConnectorType = 82,

        /// <summary>
        /// Типы технологии
        /// </summary>
        TechnologyType = 83,

        /// <summary>
        /// Тип телефона
        /// </summary>
        PhoneType = 86,

        /// <summary>
        /// Производитель
        /// </summary>
        Manufacturer = 89,

        /// <summary>
        /// Должность
        /// </summary>
        JobTitle = 90,

        /// <summary>
        /// Единица измерения
        /// </summary>
        Unit = 91,

        /// <summary>
        /// Тип программного обеспечения
        /// </summary>
        SoftwareType = 92,

        /// <summary>
        /// Модель сетевого устройства
        /// </summary>
        NetworkDeviceModel = 93,

        /// <summary>
        /// Модель терминального устройства
        /// </summary>
        TerminalDeviceModel = 94,

        /// <summary>
        /// Адаптеры
        /// </summary>
        AdapterModel = 95,

        /// <summary>
        /// Периферийное устройства
        /// </summary>
        PeripherialModel = 96,

        /// <summary>
        /// Модель программного обеспечения
        /// </summary>
        SoftwareModel = 97,

        /// <summary>
        /// Модель шкафа
        /// </summary>
        CabinetType = 98,

        /// <summary>
        /// Организация
        /// </summary>
        Organizaton = 101,

        /// <summary>
        /// Подразделение
        /// </summary>
        Division = 102,

        /// <summary>
        /// Модель расходного материала
        /// </summary>
        MaterialModel = 107,

        /// <summary>
        /// Документ
        /// </summary>
        Document = 110,

        ServiceCenter = 114,
        /// <summary>
        /// Сервисный контракт
        /// </summary>
        ServiceContract = 115,
        /// <summary>
        /// Поставщик
        /// </summary>
        Supplier = 116,

        /// <summary>
        /// Оповещения
        /// </summary>
        Notification = 117,

        /// <summary>
        /// Задание
        /// </summary>
        WorkOrder = 119,

        /// <summary>
        /// Расходный материал
        /// </summary>
        Material = 120,

        /// <summary>
        /// Каталог сервисов
        /// </summary>
        ServiceCatalogue = 127,

        /// <summary>
        /// Правила соглашений
        /// </summary>
        Rule = 129,

        /// <summary>
        /// SLA
        /// </summary>
        SLA = 130,

        /// <summary>
        /// Описание заявки
        /// </summary>
        CallSummary = 132,

        /// <summary>
        /// Влияние
        /// </summary>
        Influence = 133,

        /// <summary>
        /// Срочность
        /// </summary>
        Urgency = 134,

        /// <summary>
        /// Приоритеты
        /// </summary>
        Priority = 135,

        /// <summary>
        /// Тип заявки
        /// </summary>
        CallType = 136,

        /// <summary>
        /// Статья KB
        /// </summary>
        KBArticle = 137,

        /// <summary>
        /// Каталог статьи
        /// </summary>
        KBArticleFolder = 138,

        /// <summary>
        /// Тэг для статьи
        /// </summary>
        KBArticleTag = 139,

        /// <summary>
        /// Статус статьи базы знаний
        /// </summary>
        KnowledgeBaseArticleStatus = 140,


        /// <summary>
        /// Приоритет задания
        /// </summary>
        WorkOrderPriority = 141,

        /// <summary>
        /// Тип заданий
        /// </summary>
        WorkOrderType = 142,

        /// <summary>
        /// Параметры шаблонов оборудования мониторинга
        /// </summary>
        DeviceMonitorParameterTemplate = 146,

        /// <summary>
        /// Виджет статистики
        /// </summary>
        DashboardItem = 151,

        /// <summary>
        /// Панель статистики
        /// </summary>
        Dashboard = 152,

        /// <summary>
        /// Папка статистики
        /// </summary>
        DashboardFolder = 153,

        /// <summary>
        /// Каталог шаблона задания
        /// </summary>
        WorkOrderTemplateFolder = 154,

        /// <summary>
        /// Шаблон заданий
        /// </summary>
        WorkOrderTemplate = 155,

        /// <summary>
        /// Папка регламентных работ
        /// </summary>
        MaintenanceFolder = 156,

        /// <summary>
        /// Регламентная работа
        /// </summary>
        Maintenance = 157,

        /// <summary>
        /// Согласование
        /// </summary>
        Negotiation = 160,

        /// <summary>
        /// Устройство приложения
        /// </summary>
        DeviceApplication = 164,

        /// <summary>
        /// Информационный объект.
        /// </summary>
        DataEntity = 165,

        /// <summary>
        /// Параметры
        /// </summary>
        ParameterEnum = 169,

        /// <summary>
        /// Календарь выходных дней
        /// </summary>
        CalendarWeekend = 170,

        /// <summary>
        /// Календарь праздничных дней
        /// </summary>
        CalendarHoliday = 171,

        /// <summary>
        /// Причины отклонения от графика
        /// </summary>
        Exclusion = 172,

        /// <summary>
        /// Календарная работа по расписанию
        /// </summary>
        CalendarWorkSchedule = 173,

        /// <summary>
        /// Сервисные блоки
        /// </summary>
        ServiceUnit = 175,

        /// <summary>
        /// Модель сервисного контракта
        /// </summary>
        ServiceContractModel = 182,

        /// <summary>
        /// Лицензия ПО (самостоятельная)
        /// </summary>
        IndependentSoftwareLicence = 183,

        /// <summary>
        /// Лицензия ПО (аренда)
        /// </summary>
        RentSoftwareLicence = 184,


        /// <summary>
        /// Лицензия ПО (upgrade)
        /// </summary>
        UpgradeSoftwareLicence = 185,

        /// <summary>
        /// Лицензия ПО (подписка)
        /// </summary>
        SubscriptionSoftwareLicence = 186,

        /// <summary>
        /// Лицензия ПО (продление подписки)
        /// </summary>
        ExtensionSoftwareLicence = 187,

        /// <summary>
        /// Контроль
        /// </summary>
        CustomController = 188,

        /// <summary>
        /// OEM Лицензия ПО
        /// </summary>
        OEMSoftwareLicence = 189,

        /// <summary>
        /// Правило для выделения строк в списке
        /// </summary>
        Highlighting = 190,

        /// <summary>
        /// Условия для правила выделения строк в списке
        /// </summary>
        HighlightingCondition = 191,

        /// <summary>
        /// Значения условий для правила выделения строк в списке
        /// </summary>
        HighlightingConditionValue = 192,

        /// <summary>
        /// Задача импорта имущества
        /// </summary>
        ITAssetImportSetting = 193,

        /// <summary>
        /// Конфигурация CSV задачи импорта имущества
        /// </summary>
        ITAssetImportCSVConfiguration = 194,
		
		 /// <summary>
        /// Объект «Правило распознавания»
        /// </summary>
        SnmpDeviceModel = 195,

        /// <summary>
        /// Профиль объекта «Правило распознавания»
        /// </summary>
        SnmpDeviceProfile = 196,

        /// <summary>
        /// Лицензия на программное обеспечение
        /// </summary>
        SoftwareLicence = 223,

        /// <summary>
        /// Материнская плата
        /// </summary>
        Motherboard = 330,

        /// <summary>
        /// Процессор
        /// </summary>
        Processor = 331,

        /// <summary>
        /// Модуль оперативной памяти
        /// </summary>
        Memory = 332,

        /// <summary>
        /// Видеоадаптер
        /// </summary>
        VideoAdapter = 333,

        /// <summary>
        /// Звуковая карта
        /// </summary>
        Soundcard = 334,

        /// <summary>
        /// Сетевая карта
        /// </summary>
        NetworkAdapter = 335,

        /// <summary>
        /// Жесткий диск
        /// </summary>
        Storage = 336,

        /// <summary>
        /// CD/DVD привод
        /// </summary>
        CDAndDVDDrives = 337,

        /// <summary>
        /// Привод гибких дисков
        /// </summary>
        Floppydrive = 338,

        /// <summary>
        /// Контроллер системы хранения данных
        /// </summary>
        StorageController = 352,

        /// <summary>
        /// Панель статистики devExpress
        /// </summary>
        DevExpressDashboard = 364,

        /// <summary>
        /// Права доступа
        /// </summary>
        AccessPermission = 365,

        /// <summary>
        /// Категория каталога товаров
        /// </summary>
        ProductCatalogCategory = 374,

        /// <summary>
        /// Жизненный цикл  
        /// </summary>
        LifeCycle = 375,

        /// <summary>
        /// Модем (адаптер)
        /// </summary>
        Modem = 377,

        /// <summary>
        /// Тип каталога продуктов
        /// </summary>
        ProductCatalogType = 378,

        /// <summary>
        /// WorkOrder reference from other objects like Problem/Call etc.
        /// </summary>
        WorkOrderReference = 380,

        /// <summary>
        /// Контактные лица поставщиков
        /// </summary>
        SupplierContactPerson = 384,

        /// <summary>
        /// Тип использования ПО
        /// </summary>
        SoftwareModelUsingType = 385,

        /// <summary>
        /// Задача импорта каталога продуктов
        /// </summary>
        ProductCatalogImportSetting = 393,

        /// <summary>
        /// Конфигурация CSV задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportCSVConfiguration = 394,

        /// <summary>
        /// Склад
        /// </summary>
        StorageLocation = 397,

        /// <summary>
        /// Сервис категория
        /// </summary>
        ServiceCategory = 405,

        /// <summary>
        /// Элемент сервиса
        /// </summary>
        ServiceItem = 406,

        /// <summary>
        /// Услуга сервиса
        /// </summary>
        ServiceAttendance = 407,

        /// <summary>
        /// Сервис
        /// </summary>
        Service = 408,

        /// <summary>
        /// Узел сети
        /// </summary>
        NetworkNode = 409,

        /// <summary>
        /// Коммутатор КЕ
        /// </summary>
        SwitchConfigurationUnit = 410,

        /// <summary>
        /// Маршрутизатор КЕ
        /// </summary>
        RouterConfigurationUnit = 411,

        /// <summary>
        /// Принт сервер КЕ
        /// </summary>
        PrinterConfigurationUnit = 412,

        /// <summary>
        /// Система хранения данных КЕ
        /// </summary>
        StorageSystemConfigurationUnit = 413,

        /// <summary>
        /// Win сервер КЕ
        /// </summary>
        ServerConfigurationUnit = 414,

        /// <summary>
        /// Виртуальный сервер
        /// </summary>
        VirtualServer = 416,

        /// <summary>
        /// Хост КЕ
        /// </summary>
        HostConfigurationUnit = 419,

        /// <summary>
        /// Конфигурационная единица
        /// </summary>
        ConfigurationUnitBase = 450,

        UndisposedDevice = 666,

        /// <summary>
        /// Заявка
        /// </summary>
        Call = 701,

        /// <summary>
        /// Проблема
        /// </summary>
        Problem = 702,

        /// <summary>
        /// Запрос на изменение
        /// </summary>
        ChangeRequest = 703,

        /// <summary>
        /// Бегущая строка
        /// </summary>
        CreepingLine = 705,

        /// <summary>
        /// Результаты работы по заявке-услуге
        /// </summary>
        RFCResult = 706,

        /// <summary>
        /// Результаты работы по заявке-инциденту
        /// </summary>
        IncidentResult = 707,

        /// <summary>
        /// Тип проблемы
        /// </summary>
        ProblemType = 708,

        /// <summary>
        /// Причина проблемы
        /// </summary>
        ProblemCause = 709,

        /// <summary>
        /// RFC тип
        /// </summary>
        ChangeRequestType = 710,

        /// <summary>
        /// RFC категория
        /// </summary>
        ChangeRequestCategory = 711,

        /// <summary>
        /// Сторонняя организация
        /// </summary>
        SideOrganization = 721,

        /// <summary>
        /// Группы
        /// </summary>
        Group = 722,

        /// <summary>
        /// Папки отчетов
        /// </summary>
        ReportFolder = 727,

        /// <summary>
        /// Оточет
        /// </summary>
        Report = 728,

        /// <summary>
        /// Сообщения
        /// </summary>
        Message = 734,

        /// <summary>
        /// Сообщение электронной почты
        /// </summary>
        MessageByEmail = 735,

        /// <summary>
        /// Сообщение мониторинга
        /// </summary>
        MessageByMonitoring = 736,

        /// <summary>
        /// Сообщение опроса
        /// </summary>
        MessageByInquiry = 737,

        /// <summary>
        /// Готовое решение проблемы
        /// </summary>
        Solution = 738,

        /// <summary>
        /// Сообщение задачи опроса
        /// </summary>
        MessageByInquiryTask = 739,

        /// <summary>
        /// Сообщение системы интеграции
        /// </summary>
        MessageByIntegration = 740,

        /// <summary>
        /// Сообщение импорта оргструктуры
        /// </summary>
        MessageByOrganizationStructureImport = 741,

        /// <summary>
        /// Сообщение задачи импорта оргструктуры
        /// </summary>
        MessageByTaskForUsers = 742,

        /// <summary>
        /// Замещение
        /// </summary>
        Substitution = 745,

        /// <summary>
        /// Схема лицензирования
        /// </summary>
        LicenceScheme = 750,

        ELPSetting = 820,

        /// <summary>
        /// Состояние жизненного цикла
        /// </summary>
        LifeCycleState = 821,

        /// <summary>
        /// Синоним
        /// </summary>
        Synonym = 822,

        /// <summary>
        /// Массовый инцидент
        /// </summary>
        MassIncident = 823,

        /// <summary>
        /// Тип массовых инцидентов
        /// </summary>
        MassIncidentType = 824,

        /// <summary>
        /// Причина массовых инцидентов
        /// </summary>
        MassIncidentCause = 825,

        /// <summary>
        /// Модели
        /// </summary>
        AbstractModel = 900,

        /// <summary>
        /// Причин отклонения от графика
        /// </summary>
        CalendarExclusion = 901,

        /// <summary>
        /// Учетная запись используемая при импорте, интеграции с внешними системами и при опросе сети
        /// </summary>
        UserAccount = 902,

        /// <summary>
        /// Объекта форма
        /// </summary>
        FormBuilder = 903,

        /// <summary>
        /// Задача планировщика
        /// </summary>
        ScheduleTask = 904,

        /// <summary>
        /// Вложенный адаптер
        /// </summary>
        AdapterModelReference = 905,

        /// <summary>
        /// Журнал событый
        /// </summary>
        Event = 906,

        /// <summary>
        /// Категории технических сбоев
        /// </summary>
        TechnicalFailuresCategoryType = 907,
        
        /// <summary>
        /// OLA
        /// </summary>
        OperationalLevelAgreement = 909,

        /// <summary>
        /// Документальное оформление операций
        /// </summary>
        OperationalDocumentation = 911,

        /// <summary>
        /// Право контракта на ПО
        /// </summary>
        ServiceContractLicence = 1201,

        /// <summary>
        /// Путь к узлу LDAP
        /// </summary>
        UIADPath = 11000,

        /// <summary>
        /// Настройки импорта из базы данных
        /// </summary>
        UIDBSettings = 11001,
        
        /// <summary>
        /// Настройки таблиц для импорта из базы данных
        /// </summary>
        UIDBTables = 11002,

        /// <summary>
        /// Настройки полей для импорта из базы данных
        /// </summary>
        UIDBFileds = 11003,
        
        /// <summary>
        /// Конфигурация импорта из базы данных
        /// </summary>
        UIDBConfiguration = 11004,
        
        /// <summary>
        /// Строка подключения из базы данных
        /// </summary>
        UIDBConnectionString = 11005,
        
        /// <summary>
        /// Тип импорта из базы данных
        /// </summary>
        UIDBImportType = 11006,
        
        /// <summary>
        /// Определение типа импорта для поля  для импорта из базы данных
        /// </summary>
        UIDBFieldConfig = 11007
    }
}
