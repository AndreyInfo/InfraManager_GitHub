namespace InfraManager
{
    public enum OperationID : int
    {
        None = -1,
        Add = 0,
        Organization_Properties = 2,
        Organization_Delete = 3,
        Organization_Add = 4,
        Subdivision_View = 5,
        Subdivision_Delete = 6,
        Subdivision_Add = 7,
        Building_Delete = 9,
        Building_Add = 10,
        Floor_Delete = 12,
        Floor_Add = 13,
        Sheme_Floor = 14,
        Room_Delete = 16,
        Room_Add = 17,

        /// <summary>
        /// Получение шкафа
        /// </summary>
        Rack_Properties = 18,

        /// <summary>
        /// Удаление шкафа
        /// </summary>
        Rack_Delete = 19,

        /// <summary>
        /// Добавление шкафа
        /// </summary>
        Rack_Add = 20,

        /// <summary>
        /// Получение бегущей строки
        /// </summary>
        CreepingLine_Properties = 21,

        /// <summary>
        /// Операция со схемой шкафа
        /// </summary>
        Rack_Scheme = 22,
        /// <summary>
        /// Просмотр сетевого оборудования
        /// </summary>
        NetworkDevice_Properties = 23,
        /// <summary>
        /// Удаление сетевого оборудования
        /// </summary>
        NetworkDevice_Delete = 24,
        /// <summary>
        /// Добавление сетевого оборудования
        /// </summary>
        NetworkDevice_Add = 25,

        /// <summary>
        /// Просмотр порта объекта Сетевое оборудование
        /// </summary>
        ActivePort_Properties = 37,

        /// <summary>
        /// Добавление бегущей строки
        /// </summary>
        CreepingLine_Add = 56,

        /// <summary>
        /// Удаление бегущей строки
        /// </summary>
        CreepingLine_Delete = 57,

        Workplace_Delete = 62,
        Workplace_Add = 63,

        /// <summary>
        /// Обновление бегущей строки
        /// </summary>
        CreepingLine_Update = 64,
        /// <summary>
        /// Получение оконечного оборудования
        /// </summary>
        TerminalDevice_Properties = 65,
        /// <summary>
        /// Удаление оконечного оборудования
        /// </summary>
        TerminalDevice_Delete = 66,
        /// <summary>
        /// Добавление оконечного оборудования
        /// </summary>
        TerminalDevice_Add = 67,
        /// <summary>
        /// Подключить корд оконечного оборудования
        /// </summary>
        TerminalDevice_Connect_Cord = 68,
        /// <summary>
        /// Отключить корд оконечного оборудования
        /// </summary>
        TerminalDevice_Disconnect_Cord = 69,
        /// <summary>
        /// Просмотр пути оконечного оборудования
        /// </summary>
        TerminalDevice_Path = 70,
        User_View = 71,
        User_Delete = 72,
        User_Add = 73,
        UserImport_Abort = 74,

        /// <summary>
        /// Добавление Видов деятельности
        /// </summary>
        UserActivityType_Add = 76,
        /// <summary>
        /// Получение данных адаптера
        /// </summary>
        Adapter_Details = 77,
        /// <summary>
        /// Удаление адаптера
        /// </summary>
        Adapter_Delete = 78,
        /// <summary>
        /// Просмотр периферийных устройств
        /// </summary>
        Peripheral_Properties = 79,
        /// <summary>
        /// Удаление периферийных устройств
        /// </summary>
        Peripheral_Delete = 80,
        /// <summary>
        /// Добавлеие адаптера
        /// </summary>
        Adapter_Insert = 84,
        /// <summary>
        /// Добавление периферийных устройств
        /// </summary>
        Peripheral_Add = 85,
        /// <summary>
        ///  Получение списка инсталяций
        /// </summary>
        SoftwareInstallation_List = 87,

        /// <summary>
        /// Получение типа разъема
        /// </summary>
        ConnectorType_Properties = 89,

        /// <summary>
        /// Добавление типа разъема
        /// </summary>
        ConnectorType_Add = 90,

        /// <summary>
        /// Получение типов технологий
        /// </summary>
        TechnologyType_Properties = 91,

        /// <summary>
        /// Добавление типов технлогий
        /// </summary>
        TechnologyType_Add = 92,
        SlotType_Properties = 93,
        SlotType_Add = 94,
        RoomType_Properties = 95,
        RoomType_Add = 96,

        /// <summary>
        /// Получение вида среды передач
        /// </summary>
        Medium_Properties = 97,

        TelephoneType_Properties = 105,
        TelephoneType_Add = 106,
        CartridgeType_Properties = 110,
        CartridgeType_Add = 111,
        CartridgeType_Delete = 112,
        /// <summary>
        /// Получение производителя
        /// </summary>
        Manufacturer_Properties = 113,
        /// <summary>
        /// Добавление производителя
        /// </summary>
        Manufacturer_Add = 114,
        /// <summary>
        /// Удаление производителя
        /// </summary>
        Manufacturer_Delete = 115,
        /// <summary>
        /// Управление синонимами производителя
        /// </summary>
        Manufacturer_Synonim = 116,

        /// <summary>
        /// Получение Должности
        /// </summary>
        Position_Properties = 117,

        /// <summary>
        /// Сохранение Должности
        /// </summary>
        Position_Add = 118,

        /// <summary>
        /// Удаление Должности
        /// </summary>
        Position_Delete = 119,

        /// <summary>
        /// Получение единицы измерения
        /// </summary>
        Unit_Properties = 121,

        /// <summary>
        /// Добавление единицы измерения
        /// </summary>
        Unit_Add = 122,

        /// <summary>
        /// Удаление единицы измерения
        /// </summary>
        Unit_Delete = 123,

        /// <summary>
        ///  Получение программного обеспечения
        /// </summary>
        SoftwareType_Properties = 124,
        /// <summary>
        ///  Добавление программного обеспечения
        /// </summary>
        SoftwareType_Add = 125,
        /// <summary>
        /// Удаление типа программного обеспечения
        /// </summary>
        SoftwareType_Delete = 126,
        /// <summary>
        /// Модель сетевого устройства, просмотр
        /// </summary>
        NetworkDeviceModel_Properties = 127,
        /// <summary>
        /// Модель сетевого устройства, добавление
        /// </summary>
        NetworkDeviceModel_Add = 128,
        /// <summary>
        /// Модель сетевого устройства, удаление
        /// </summary>
        NetworkDeviceModel_Delete = 129,
        /// <summary>
        /// Модель сетевого устройства, Открыть список портов
        /// </summary>
        NetworkDeviceModel_Ports = 130,
        /// <summary>
        /// Модель оконечного устройства, просмотр
        /// </summary>
        TerminalDeviceModel_Properties = 131,
        /// <summary>
        /// Модель оконечного устройства, добавление
        /// </summary>
        TerminalDeviceModel_Add = 132,
        /// <summary>
        /// Модель оконечного устройства, удаление
        /// </summary>
        TerminalDeviceModel_Delete = 133,
        /// <summary>
        /// Модель адаптера, получение
        /// </summary>
        AdapterModel_Properties = 134,
        /// <summary>
        /// Модель адаптера, добавление
        /// </summary>
        AdapterModel_Add = 135,
        /// <summary>
        /// Модель адаптера, удаление
        /// </summary>
        AdapterModel_Delete = 136,
        /// <summary>
        /// Модель адаптера, Управление синонимами
        /// </summary>
        AdapterModel_Synonim = 137,
        /// <summary>
        /// Модель периферии, просмотр
        /// </summary>
        PeripheralModel_Properties = 138,
        /// <summary>
        /// Модель периферии, добавление
        /// </summary>
        PeripheralModel_Add = 139,
        /// <summary>
        /// Модель периферии, добавление
        /// </summary>
        PeripheralModel_Delete = 140,
        /// <summary>
        /// Модель периферии, синоним
        /// </summary>
        PeripheralModel_Synonim = 141,
        /// <summary>
        /// Получение модели ПО
        /// </summary>
        SoftwareModel_Properties = 142,
        /// <summary>
        /// Добавление модели ПО
        /// </summary>
        SoftwareModel_Add = 143,
        /// <summary>
        /// Удаление модели ПО
        /// </summary>
        SoftwareModel_Delete = 144,
        /// <summary>
        /// Получение данных шкафа
        /// </summary>
        CabinetType_Details = 145,
        /// <summary>
        /// Добавление шкафа
        /// </summary>
        CabinetType_Insert = 146,
        /// <summary>
        /// Удаление шкафа
        /// </summary>
        CabinetType_Delete = 147,
        /// <summary>
        /// Модель расходного материала, получение
        /// </summary>
        MaterialModel_Properties = 167,
        /// <summary>
        /// Модель расходного материала, добавление
        /// </summary>
        MaterialModel_Add = 168,
        /// <summary>
        /// Модель расходного материала, удаление
        /// </summary>
        MaterialModel_Delete = 169,


        Document_View = 176,
        Document_Edit = 177,
        Document_SaveAs = 178,
        Document_Delete = 179,
        NewDocument_Document = 180,
        Document_Checkout = 182,
        Document_Checkin = 183,
        Document_Properties = 184,
        /// <summary>
        /// Получение расходного материала для картриджей
        /// </summary>
        MaterialConsumption_Properties = 185,
        /// <summary>
        /// Добавление расходного материала для картриджей
        /// </summary>
        MaterialConsumption_Add = 186,
        /// <summary>
        /// Удаление расходного материала для картриджей
        /// </summary>
        MaterialConsumption_Delete = 187,
        /// <summary>
        /// Получение сервисного контракта
        /// </summary>
        ServiceContract_Properties = 211,
        /// <summary>
        /// Добавление сервисного контракта
        /// </summary>
        ServiceContract_Add = 212,
        /// <summary>
        /// Удаление сервисного контракта
        /// </summary>
        ServiceContract_Delete = 213,
        /// <summary>
        /// Просмотр оповещения
        /// </summary>
        Notification_Properties = 220,

        Problem_AddAs = 221,
        Problem_Properties = 222,
        Owner_Update = 226,
        Organization_Update = 227,
        Subdivision_Update = 228,
        Building_Update = 229,
        Floor_Update = 230,
        Room_Update = 231,

        /// <summary>
        /// Обновление шкафа
        /// </summary>
        Rack_Update = 232,

        /// <summary>
        /// Обновление сетевого оборудования
        /// </summary>
        NetworkDevice_Update = 233,

        /// <summary>
        /// Обновление порта объекта Сетевое оборудование
        /// </summary>
        ActivePort_Update = 235,

        Workplace_Update = 239,

        /// <summary>
        /// Обновление оконечного оборудования
        /// </summary>
        TerminalDevice_Update = 240,
        User_Update = 241,

        /// <summary>
        /// Обновление адаптера
        /// </summary>
        Adapter_Update = 242,
        /// <summary>
        /// Обновление переферийных устройств
        /// </summary>
        Peripheral_Update = 243,

        /// <summary>
        /// Обновление типа разъема
        /// </summary>
        ConnectorType_Update = 246,

        SlotType_Update = 248,

        /// <summary>
        /// Обновление типов технологий
        /// </summary>
        TechnologyType_Update = 247,
        RoomType_Update = 249,
        TelephoneType_Update = 254,
        CartridgeType_Update = 256,
        /// <summary>
        /// Обновление производителя
        /// </summary>
        Manufacturer_Update = 257,
        /// <summary>
        /// Обновление Должности
        /// </summary>
        Position_Update = 258,

        /// <summary>
        /// Обновление единицы измерения
        /// </summary>
        Unit_Update = 259,

        /// <summary>
        /// Обновление программного обеспечения
        /// </summary>
        SoftwareType_Update = 260,
        /// <summary>
        /// Модель сетевого устройства, обновление
        /// </summary>
        NetworkDeviceModel_Update = 261,
        /// <summary>
        /// Модель оконечного устройства, обновление
        /// </summary>
        TerminalDeviceModel_Update = 262,
        /// <summary>
        /// Модель адаптера, обновление
        /// </summary>
        AdapterModel_Update = 263,
        /// <summary>
        /// Модель периферии, обновление
        /// </summary>
        PeripheralModel_Update = 264,
        /// <summary>
        /// Обновление модели ПО
        /// </summary>
        SoftwareModel_Update = 265,

        /// <summary>
        /// Обновление шкафа
        /// </summary>
        CabinetType_Update = 266,
        /// <summary>
        /// Модель расходного материала, обновление
        /// </summary>
        MaterialModel_Update = 273,
        /// <summary>
        /// Обновление расходного материала для картриджей
        /// </summary>
        MaterialConsumption_Update = 274,
        Document_Update = 281,
        /// <summary>
        /// Обновление сервисного контракта
        /// </summary>
        ServiceContract_Update = 283,
        /// <summary>
        /// Обновление оповещения
        /// </summary>
        Notification_Update = 286,

        Role_Properties = 288,
        Role_Add = 289,
        Role_Delete = 290,
        Role_SetRole = 291,
        Role_Update = 292,
        Document_Save = 297,
        AddReference_Document = 299,
        Document_ResetState = 300,
        WorkOrder_Add = 301,
        WorkOrder_Properties = 302,

        /// <summary>
        /// Голосование
        /// </summary>
        SD_General_VotingUser = 303,

        /// <summary>
        /// Удаление Видов деятельности
        /// </summary>
        UserActivityType_Delete = 304,

        Roles_Add = 305,
        Call_Add = 309,
        Call_Delete = 310,

        /// <summary>
        /// Обновление Видов деятельности
        /// </summary>
        UserActivityType_Update = 312,

        Call_Update = 313,

        /// <summary>
        /// Добавление трудозатрат
        /// </summary>
        ManhoursWork_Add = 314,

        Call_AddAs = 315,

        /// <summary>
        /// Удаление трудозатрат
        /// </summary>
        ManhoursWork_Delete = 316,

        /// <summary>
        /// Получение информации о трудозатратах
        /// </summary>
        ManhoursWork_Properties = 317,

        /// <summary>
        /// Обновление трудозатрат
        /// </summary>
        ManhoursWork_Update = 318,

        Problem_Add = 319,
        Problem_Delete = 320,
        Problem_Update = 323,
        WorkOrder_Delete = 330,
        WorkOrder_AddAs = 331,
        WorkOrder_Update = 333,

        /// <summary>
        /// Добавление оповещения
        /// </summary>
        Notification_Add = 342,

        /// <summary>
        /// Удаление оповещения
        /// </summary>
        Notification_Delete = 343,


        NavigateToObject = 350,

        /// <summary>
        /// Материально ответственное лицо
        /// </summary>
        MateriallyResponsible = 356,

        SD_General_Administrator = 357,

        /// <summary>
        /// Операция дает возможность пользователю быть назначенным в качестве "Исполнителя" для "Заявок", "Заданий", "Проблем" Службы Поддержки.
        /// </summary>
        SD_General_Executor = 358,

        /// <summary>
        /// обновление SLA
        /// </summary>
        SLA_Insert = 359,

        /// <summary>
        /// просмотр SLA
        /// </summary>
        SLA_Properties = 360,

        /// <summary>
        /// Удаление SLA
        /// </summary>
        SLA_Delete = 361,

        /// <summary>
        /// обновление SLA
        /// </summary>
        SLA_Update = 362,

        /// <summary>
        /// правила SLA Добавление
        /// </summary>
        Rule_Add = 363,

        /// <summary>
        /// правила SLA Просмотр
        /// </summary>

        Rule_Properties = 364,

        /// <summary>
        /// правила SLA Удаление
        /// </summary>

        /// <summary>
        /// правила SLA удаление
        /// </summary>
        Rule_Delete = 365,

        /// <summary>
        /// правила SLA Обновление
        /// </summary>
        Rule_Update = 366,

        /// <summary>
        /// Просматривать Каталог сервисов через форму свойств
        /// </summary>
        ServiceCatalog_Properties = 367,

        /// <summary>
        /// Позволяет измененять Каталог сервисов через форму свойств
        /// </summary>
        ServiceCatalog_Update = 368,
        Control_Add = 369,
        Control_Remove = 370,
        SD_General_Owner = 373,
        Group_Properties = 379,
        Group_Add = 380,
        Group_Update = 381,
        Group_Delete = 382,

        /// <summary>
        /// Свойства запроса на изменение
        /// </summary>
        ChangeRequest_Properties = 383,

        /// <summary>
        /// Добавление запроса на изменение
        /// </summary>
        ChangeRequest_Add = 384,

        /// <summary>
        /// Обновление запроса на изменение
        /// </summary>
        ChangeRequest_Update = 386,

        /// <summary>
        /// Удаление запроса на изменение
        /// </summary>
        ChangeRequest_Delete = 385,

        /// <summary>
        /// Получение резульатат инцидента
        /// </summary>
        IncidentResult_Properties = 387,

        /// <summary>
        /// Добавление резульатат инцидента
        /// </summary>
        IncidentResult_Add = 388,

        /// <summary>
        /// Удалние резульатат инцидента
        /// </summary>
        IncidentResult_Delete = 389,

        /// <summary>
        /// Обновление резульатат инцидента
        /// </summary>
        IncidentResult_Update = 390,

        ProblemType_Properties = 391,
        ProblemType_Add = 392,
        ProblemType_Delete = 393,
        ProblemType_Update = 394,

        /// <summary>
        /// Получение причин проблем
        /// </summary>
        ProblemCause_Properties = 395,

        /// <summary>
        /// Добавление причин проблем
        /// </summary>
        ProblemCause_Add = 396,

        /// <summary>
        /// Удаление причин проблем
        /// </summary>
        ProblemCause_Delete = 397,

        /// <summary>
        /// Обновление причин проблем
        /// </summary>
        ProblemCause_Update = 398,

        /// <summary>
        /// Показать зависимости через схему зависимостей
        /// </summary>
        Dependents = 422,

        Problem_PowerfullAccess = 428,
        TaskOfImport_Abort = 434,

        /// <summary>
        /// Добавление слота
        /// </summary>
        Slot_Add = 435,

        /// <summary>
        /// Обновление слота
        /// </summary>
        Slot_Update = 436,

        /// <summary>
        /// Удаление слота
        /// </summary>
        Slot_Delete = 437,

        /// <summary>
        /// Получение слота
        /// </summary>
        Slot_Properties = 438,

        Document_RemoveReference = 439,

        /// <summary>
        /// Добавение лицензии ПО
        /// </summary>
        SoftwareLicence_Add = 440,

        /// <summary>
        /// Получение лицензии ПО
        /// </summary>
        SoftwareLicence_Properties = 441,

        /// <summary>
        /// Обновление лицензии ПО
        /// </summary>
        SoftwareLicence_Update = 442,

        /// <summary>
        /// Удаление лицензии ПО
        /// </summary>
        SoftwareLicence_Delete = 443,

        /// <summary>
        /// Добавение папок отчетов
        /// </summary>
        ReportFolder_Add = 446,

        /// <summary>
        /// Получение папок отчетов
        /// </summary>
        ReportFolder_Properties = 447,

        /// <summary>
        /// Обновление папок отчетов
        /// </summary>
        ReportFolder_Update = 448,

        /// <summary>
        /// Удаление папок отчетов
        /// </summary>
        ReportFolder_Delete = 449,

        /// <summary>
        /// Добавление отчетов
        /// </summary>
        Report_Add = 450,

        /// <summary>
        /// Получение отчетов
        /// </summary>
        Report_Properties = 451,

        /// <summary>
        /// Обновление отчетов
        /// </summary>
        Report_Update = 452,

        /// <summary>
        /// Уаделние отчетов
        /// </summary>
        Report_Delete = 453,

        Report_View = 454,
        Report_Edit = 455,
        Asset_ChangeDeviceComposition = 456,
        InquirySubdevice_Create = 458,
        InquirySubdevice_Cancel = 459,
        InquirySubdevice_Block = 460,
        Compare = 467,

        /// <summary>
        /// Получение краткого описание заявки
        /// </summary>
        CallSummary_Properties = 468,

        /// <summary>
        /// Добавление краткого описание заявки
        /// </summary>
        CallSummary_Add = 469,

        /// <summary>
        /// Удаление краткого описание заявки
        /// </summary>
        CallSummary_Delete = 470,

        /// <summary>
        /// Обновление краткого описание заявки
        /// </summary>
        CallSummary_Update = 471,

        /// <summary>
        /// Получение влияния
        /// </summary>
        Influence_Properties = 472,

        /// <summary>
        /// Добавление влияния
        /// </summary>
        Influence_Add = 473,

        /// <summary>
        /// Удаление влияния
        /// </summary>
        Influence_Delete = 474,

        /// <summary>
        /// Обновление влияния
        /// </summary>
        Influence_Update = 475,

        /// <summary>
        /// Получение срочности
        /// </summary>
        Urgency_Properties = 476,

        /// <summary>
        /// Добавление срочности
        /// </summary>
        Urgency_Add = 477,

        /// <summary>
        /// Удаление срочности
        /// </summary>
        Urgency_Delete = 478,

        /// <summary>
        /// Обновление срочности
        /// </summary>
        Urgency_Update = 479,

        Priority_Add = 481,
        Priority_Update = 483,
        Priority_Delete = 482,
        CallType_Properties = 484,
        CallType_Add = 485,
        CallType_Delete = 486,
        CallType_Update = 487,

        /// <summary>
        /// Соединение кратких описаний заявок
        /// </summary>
        CallSummary_Merge = 488,
        
        /// <summary>
        /// Статья базы знаний - Открыть свойства
        /// </summary>
        KBArticle_Properties = 489,

        /// <summary>
        /// Папка статьи базы знаний - Открыть свойства
        /// </summary>
        KBArticleFolder_Properties = 493,

        /// <summary>
        /// Добавление папки в базу знаний
        /// </summary>
        KBArticleFolder_Add = 494,

        /// <summary>
        /// Удаление папки из базы знаний
        /// </summary>
        KBArticleFolder_Delete = 495,

        /// <summary>
        /// Изменение папки базы знаний
        /// </summary>
        KBArticleFolder_Update = 496,

        /// <summary>
        /// Получения статей базы знаний
        /// </summary>
        KBArticleStatus_Properties = 501,

        /// <summary>
        /// Добавление статей базы знаний
        /// </summary>
        KBArticleStatus_Add = 502,

        /// <summary>
        /// Удаление статей базы знаний
        /// </summary>
        KBArticleStatus_Delete = 503,

        /// <summary>
        /// Обновление статей базы знаний
        /// </summary>
        KBArticleStatus_Update = 504,
        Default = 505,
        WorkOrderPriority_Properties = 506,
        WorkOrderPriority_Add = 507,
        WorkOrderPriority_Delete = 508,
        WorkOrderPriority_Update = 509,
        WorkOrderType_Properties = 510,
        WorkOrderType_Add = 511,
        WorkOrderType_Delete = 512,
        WorkOrderType_Update = 513,

        /// <summary>
        /// Заявка - Открыть свойства
        /// </summary>
        Call_Properties = 518,

        Call_PowerfullAccess = 522,
        WorkOrder_PowerfullAccess = 524,

        /// <summary>
        /// Получение шаблона параметров оборудования мониторинга
        /// </summary>
        DeviceMonitorParameterTemplate_Properties = 529,
        /// <summary>
        /// Добавление шаблона параметров оборудования мониторинга
        /// </summary>
        DeviceMonitorParameterTemplate_Add = 530,
        /// <summary>
        /// Удаление шаблона параметров оборудования мониторинга
        /// </summary>
        DeviceMonitorParameterTemplate_Delete = 531,
        /// <summary>
        /// Обновление шаблона параметров оборудования мониторинга
        /// </summary>
        DeviceMonitorParameterTemplate_Update = 532,

        SwitchOn = 547,

        /// <summary>
        /// Просмотр виджета панели статистики
        /// </summary>
        DashboardItem_Properties = 550,

        /// <summary>
        /// Добавление виджета панели статистики
        /// </summary>
        DashboardItem_Add = 551,

        /// <summary>
        /// Удаление виджета панели статистики
        /// </summary>
        DashboardItem_Delete = 552,

        /// <summary>
        /// Обновление виджета панели статистики
        /// </summary>
        DashboardItem_Update = 553,

        /// <summary>
        /// Просмотр панели статистики
        /// </summary>
        Dashboard_Properties = 554,

        // <summary>
        /// Добавление панели статистики
        /// </summary>
        Dashboard_Add = 555,

        // <summary>
        /// Удаление панели статистики
        /// </summary>
        Dashboard_Delete = 556,

        // <summary>
        /// Обновление панели статистики
        /// </summary>
        Dashboard_Update = 557,

        /// <summary>
        /// Просмотр папки панели статистики
        /// </summary>
        DashboardFolder_Properties = 558,

        // <summary>
        /// Добавление папки панели статистики
        /// </summary>
        DashboardFolder_Add = 559,

        // <summary>
        /// Удаление папки панели статистики
        /// </summary>
        DashboardFolder_Delete = 560,

        // <summary>
        /// Обновление папки панели статистики
        /// </summary>
        DashboardFolder_Update = 561,

        View = 562,

        /// <summary>
        /// Все заявки по объекту
        /// </summary>
        Calls = 563,
        WorkOrderTemplateFolder_Properties = 564,
        WorkOrderTemplateFolder_Add = 565,
        WorkOrderTemplateFolder_Delete = 566,
        WorkOrderTemplateFolder_Update = 567,
        WorkOrderTemplate_Properties = 568,
        WorkOrderTemplate_Add = 569,
        WorkOrderTemplate_Delete = 570,
        WorkOrderTemplate_Update = 571,

        /// <summary>
        /// Получение папки регламентных работ
        /// </summary>
        MaintenanceFolder_Properties = 573,

        /// <summary>
        /// Добавление папки регламентных работ
        /// </summary>
        MaintenanceFolder_Add = 574,

        /// <summary>
        /// Удаление папки регламентных работ
        /// </summary>
        MaintenanceFolder_Delete = 575,

        /// <summary>
        /// Обновление папки регламентных работ
        /// </summary>
        MaintenanceFolder_Update = 576,

        /// <summary>
        /// Получение регламентных работ
        /// </summary>
        Maintenance_Properties = 577,

        /// <summary>
        /// Добавление регламентных работ
        /// </summary>
        Maintenance_Add = 578,

        /// <summary>
        /// Удаление регламентных работ
        /// </summary>
        Maintenance_Delete = 579,

        /// <summary>
        /// Обновление регламентных работ
        /// </summary>
        Maintenance_Update = 580,

        /// <summary>
        /// Публикация Workflow схемы
        /// </summary>
        WorkflowScheme_Publish = 589,

        /// <summary>
        /// Просмотр Workflow
        /// </summary>
        Workflow_Properties = 591,

        /// <summary>
        /// Удаление Workflow
        /// </summary>
        Workflow_Delete = 593,

        Call_ShowCallsForSubdivisionInWeb = 596,
        Problem_ShowProblemsForITSubdivisionInWeb = 598,
        WorkOrder_ShowWorkOrdersForITSubdivisionInWeb = 599,

        /// <summary>
        /// Получение устройства приложения
        /// </summary>
        DeviceApplication_Properties = 610,
        /// <summary>
        /// Добавление устройства приложения
        /// </summary>
        DeviceApplication_Add = 611,
        /// <summary>
        /// Удаление устройства приложения
        /// </summary>
        DeviceApplication_Delete = 612,
        /// <summary>
        /// Обновление устройства приложения
        /// </summary>
        DeviceApplication_Update = 613,

        /// <summary>
        /// Операция позволяет просматривать свойства Информационного объекта через форму свойств, но не позволяет изменять.
        /// </summary>
        DataEntity_Details = 614,

        /// <summary>
        /// Операция дает возможность создавать новый Информационный объект, но не дает возможности просмотра и изменения Информационного объекта.
        /// </summary>
        DataEntity_Add = 615,

        /// <summary>
        /// Операция дает возможность удалять Информационный объект.
        /// </summary>
        DataEntity_Delete = 616,

        /// <summary>
        /// Операция позволяет измененять поля Информационного объекта через форму свойств.
        /// </summary>
        DataEntity_Update = 617,

        FileSystem_Properties = 618,
        FileSystem_Add = 619,
        FileSystem_Delete = 620,
        FileSystem_Update = 621,
        SD_General_Calls_View = 647,
        SD_General_Problems_View = 648,
        SD_General_WorkOrders_View = 649,


        /// <summary>
        /// Операция позволяет работать с модулем управления конфигурациями
        /// </summary>
        ApplicationModule_ConfigurationManagment_View = 650,

        ApplicationModule_ServiceDesk_View = 651,
        Document_AddFromFolder = 655,

        /// Справочник параметров добавление
        /// </summary>
        ParameterEnum_Add = 658,

        /// <summary>
        /// Справочник параметров удаление
        /// </summary>
        ParameterEnum_Delete = 659,

        /// <summary>
        /// Справочник параметров обновление
        /// </summary>
        ParameterEnum_Update = 660,

        /// <summary>
        /// Все задания по объекту
        /// </summary>
        WorkOrders = 665,
        CalendarWeekend_Properties = 667,
        CalendarWeekend_Add = 668,
        CalendarWeekend_Delete = 669,
        CalendarWeekend_Update = 670,
        CalendarHoliday_Properties = 671,
        CalendarHoliday_Add = 672,
        CalendarHoliday_Delete = 673,
        CalendarHoliday_Update = 674,

        /// <summary>
        /// Получение причин отклонения
        /// </summary>
        ExclusionProperties = 675,

        /// <summary>
        /// Добавление причин отклонения
        /// </summary>
        ExclusionAdd = 676,

        /// <summary>
        /// Удаление причин отклонения
        /// </summary>
        ExclusionDelete = 677,

        /// <summary>
        /// Обновление причин отклонения
        /// </summary>
        ExclusionUpdate = 678,

        /// <summary>
        /// Получение графика работ
        /// </summary>
        CalendarWorkSchedule_Properties = 679,

        /// <summary>
        /// Добавление графика работ
        /// </summary>
        CalendarWorkSchedule_Add = 680,

        /// <summary>
        /// Удаление графика работ
        /// </summary>
        CalendarWorkSchedule_Delete = 681,

        /// <summary>
        /// Обновление графика работ
        /// </summary>
        CalendarWorkSchedule_Update = 682,

        /// <summary>
        /// Создать по аналогии
        /// </summary>
        CalendarWorkSchedule_AddAs = 683,

        /// <summary>
        /// Получение решения
        /// </summary>
        Solution_Properties = 684,

        /// <summary>
        /// Доабвление решения
        /// </summary>
        Solution_Add = 685,

        /// <summary>
        /// Удаление решения
        /// </summary>
        Solution_Delete = 686,

        /// <summary>
        /// Обновление решения
        /// </summary>
        Solution_Update = 687,

        /// <summary>
        /// Пометить заметку как прочитанная
        /// </summary>
        Note_MarkAsReaded = 688,

        /// <summary>
        /// Пометить заметку как непрочитанная
        /// </summary>
        Note_MarkAsUnread = 689,

        /// <summary>
        /// Позволяет получить доступ ко всем заявкам (в рамках доступных операций и списков)
        /// </summary>
        Call_ViewAll = 705,

        /// <summary>
        /// Просмотр запроса на изменение
        /// </summary>
        SD_General_ChangeRequests_View = 707,

        /// <summary>
        /// Участвовать в автоназначении.
        /// </summary>
        SD_General_ParticipateInAutoAssign = 708,

        /// <summary>
        /// Вставка нормы расхода
        /// </summary>
        ProductCatalog_Material_Consumtion_Rate_Insert = 710,

        /// <summary>
        /// Обновление нормы расхода
        /// </summary>
        ProductCatalog_Material_Consumtion_Rate_Update = 712,

        /// <summary>
        /// Удаление нормы расхода
        /// </summary>
        ProductCatalog_Material_Consumtion_Rate_Delete = 713,

        /// <summary>
        /// Получение данных о норме расхода
        /// </summary>
        ProductCatalog_Material_Consumtion_Rate_ViewDetails = 714,

        /// <summary>
        /// Получение таблицы нормы расхода
        /// </summary>
        ProductCatalog_Material_Consumtion_Rate_ViewDetailsArray = 715,

        /// <summary>
        /// Создание порта объекта Сетевое оборудование
        /// </summary>
        ActivePort_Add = 722,

        /// <summary>
        /// Удаление порта объекта Сетевое оборудование
        /// </summary>
        ActivePort_Delete = 723,

        CostSetting_Abort = 747,
        /// <summary>
        /// Создание панели новое
        /// </summary>
        Dashboard_AddDevExpress = 757,
        Dashboard_AccessManage = 762,
        InfrastructureSegment_Properties = 763,
        InfrastructureSegment_Add = 764,
        InfrastructureSegment_Delete = 765,
        InfrastructureSegment_Update = 766,

        /// <summary>
        /// Просмотри критичности
        /// </summary>
        Criticality_Properties = 767,

        /// <summary>
        /// Добавление критичности
        /// </summary>
        Criticality_Add = 768,

        /// <summary>
        /// Удаление критичности
        /// </summary>
        Criticality_Delete = 769,

        /// <summary>
        /// Обновление критичности
        /// </summary>
        Criticality_Update = 770,

        Project_Properties = 786,

        /// <summary>
        /// Создание сервисов
        /// </summary>
        ServiceCatalog_Service_Add = 780,

        /// <summary>
        /// Создание категории сервисов
        /// </summary>
        ServiceCatalog_Category_Add = 781,
        WorkOrderTemplate_AddAs = 792,
        ServiceUnit_Properties = 793,
        ServiceUnit_Add = 794,
        ServiceUnit_Delete = 795,
        ServiceUnit_Update = 796,

        /// <summary>
        /// Просмотр деталей категории каталога продуктов
        /// </summary>
        ProductCatalogCategory_Details = 799,

        /// <summary>
        /// Добавление категории каталога продуктов
        /// </summary>
        ProductCatalogCategory_Insert = 800,

        /// <summary>
        /// Удаление категории каталога продуктов
        /// </summary>
        ProductCatalogCategory_Delete = 801,

        /// <summary>
        /// Обновление категории каталога продуктов
        /// </summary>
        ProductCatalogCategory_Update = 802,

        /// <summary>
        /// Просмотр свойств жизненного цикла
        /// </summary>
        LifeCycle_Properties = 803,

        /// <summary>
        /// Создание жизненного цикла
        /// </summary>
        LifeCycle_Add = 804,
        
        /// <summary>
        /// Создание по аналогии жизненного цикла
        /// </summary>
        LifeCycle_AddAs = 805,

        /// <summary>
        /// Удаление жизненного цикла
        /// </summary>
        LifeCycle_Delete = 806,

        
        /// <summary>
        /// Обновление жизненного цикла
        /// </summary>
        LifeCycle_Update = 807,

        /// <summary>
        /// Получение параметров типа каталога продуктов
        /// </summary>
        ProductCatalogType_Details = 808,

        /// <summary>
        /// Добавление типа каталога продуктов
        /// </summary>
        ProductCatalogType_Insert = 809,

        /// <summary>
        /// Удаление типа каталога продуктов
        /// </summary>
        ProductCatalogType_Delete = 810,

        /// <summary>
        /// Обновление типа каталога продуктов
        /// </summary>
        ProductCatalogType_Update = 811,
        /// <summary>
        /// Модель лицензии на ПО, получение
        /// </summary>
        SoftwareLicenseModel_Properties = 844,
        /// <summary>
        /// Модель лицензии на ПО, добавление
        /// </summary>
        SoftwareLicenseModel_Add = 845,
        /// <summary>
        /// Модель лицензии на ПО, удаление
        /// </summary>
        SoftwareLicenseModel_Delete = 846,
        /// <summary>
        /// Модель лицензии на ПО, обновление
        /// </summary>
        SoftwareLicenseModel_Update = 847,

        /// <summary>
        /// Просмотр контактных лиц поставщиков
        /// </summary>
        SupplierContactPerson_Properties = 864,

        /// <summary>
        /// Добавление контактных лиц поставщиков
        /// </summary>
        SupplierContactPerson_Add = 865,

        /// <summary>
        /// Обновление контактных лиц поставщиков
        /// </summary>
        SupplierContactPerson_Update = 867,

        /// <summary>
        /// Удаление контактных лиц поставщиков
        /// </summary>
        SupplierContactPerson_Delete = 866,

        /// <summary>
        /// Получение сервисного контракта
        /// </summary>
        ServiceContractModel_Properties = 886,
        /// <summary>
        /// Добавление сервисного контракта
        /// </summary>
        ServiceContractModel_Add = 887,
        /// <summary>
        /// Удаление сервисного контракта
        /// </summary>
        ServiceContractModel_Delete = 888,
        /// <summary>
        /// Обновление сервисного контракта
        /// </summary>
        ServiceContractModel_Update = 889,
        /// <summary>
        /// Получение типа использования ПО
        /// </summary>
        SoftwareModelUsingType_Properties = 868,

        /// <summary>
        /// Добавление типа использования ПО
        /// </summary>
        SoftwareModelUsingType_Add = 869,

        /// <summary>
        /// Удаление типа использования ПО
        /// </summary>
        SoftwareModelUsingType_Delete = 870,

        /// <summary>
        /// Обновление типа использования ПО
        /// </summary>
        SoftwareModelUsingType_Update = 871,

        /// <summary>
        /// Добавление задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportSetting_Insert = 900,

        /// <summary>
        /// Обновление задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportSetting_Update = 901,

        /// <summary>
        /// Удаление задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportSetting_Delete = 902,

        /// <summary>
        /// Детализация задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportSetting_Details = 903,

        ProductCatalogImportSetting_Abort = 905,

        /// <summary>
        /// Добавление конфигурации CSV задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportCSVConfiguration_Insert = 910,

        /// <summary>
        /// Обновление конфигурации CSV задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportCSVConfiguration_Update = 911,

        /// <summary>
        /// Удаление конфигурации CSV задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportCSVConfiguration_Delete = 912,

        /// <summary>
        /// Детализация конфигурации CSV задачи импорта каталога продуктов
        /// </summary>
        ProductCatalogImportCSVConfiguration_Details = 913,

        StorageLocation_Add = 932,
        StorageLocation_Update = 933,
        StorageLocation_Delete = 934,
        StorageLocation_Properties = 935,

        /// <summary>
        /// Добавление узла сети
        /// </summary>
        NetworkNode_Add = 952,

        /// <summary>
        /// Изменение узла сети
        /// </summary>
        NetworkNode_Update = 953,

        /// <summary>
        /// Удаление узла сети
        /// </summary>
        NetworkNode_Delete = 954,

        /// <summary>
        /// Получение узла сети
        /// </summary>
        NetworkNode_Properties = 955,

        /// <summary>
        /// Просмотр данных массового инцидента
        /// </summary>
        MassIncident_Properties = 980,

        /// <summary>
        /// Просмотр списка "Все массовые инциденты"
        /// </summary>
        MassIncident_ViewAllMassiveIncidentsReport = 981,

        /// <summary>
        /// Создание массовых инцидентов
        /// </summary>
        MassIncident_Add = 982,

        /// <summary>
        /// Редактирование массовых инцидентов
        /// </summary>
        MassIncident_Update = 983,

        /// <summary>
        /// Удаление массовых инцидентов
        /// </summary>
        MassIncident_Delete = 984,

        /// <summary>
        /// Просмотр типов массовых инцидентов
        /// </summary>
        MassIncidentType_Properties = 985,

        /// <summary>
        /// Создание типов массовых инцидентов
        /// </summary>
        MassIncidentType_Add = 986,

        /// <summary>
        /// Редактирование типов массовых инцидентов
        /// </summary>
        MassIncidentType_Update = 987,

        /// <summary>
        /// Удаление типов массовых инцидентов
        /// </summary>
        MassIncidentType_Delete = 988,

        /// <summary>
        /// Просмотр причин массовых инцидентов
        /// </summary>
        MassIncidentCause_Properties = 989,

        /// <summary>
        /// Создание причин массовых инцидентов
        /// </summary>
        MassIncidentCause_Add = 990,

        /// <summary>
        /// Редактирование причин массовых инцидентов
        /// </summary>
        MassIncidentCause_Update = 991,

        /// <summary>
        /// Удаление причин массовых инцидентов
        /// </summary>
        MassIncidentCause_Delete = 992,

        /// <summary>
        /// Создание Имущества
        /// </summary>
        Asset_Add = 993,

        /// <summary>
        /// Редактирование Имущества
        /// </summary>
        Asset_Update = 994,

        /// <summary>
        /// Удаление Имущества
        /// </summary>
        Asset_Delete = 995,

        /// <summary>
        /// Просмотр Имущества
        /// </summary>
        Asset_Properties = 996,

        /// <summary>
        /// Добавление конфигурационной единицы
        /// </summary>
        ConfigurationUnitBase_Add = 997,

        /// <summary>
        /// Изменение конфигурационной единицы
        /// </summary>
        ConfigurationUnitBase_Update = 998,

        /// <summary>
        /// Удаление конфигурационной единицы
        /// </summary>
        ConfigurationUnitBase_Delete = 999,

        /// <summary>
        /// Получение конфигурационной единицы
        /// </summary>
        ConfigurationUnitBase_Properties = 1000,

        /// <summary>
        /// Модель адаптера добавление
        /// </summary>
        AdapterModel_Insert = 1010,

        /// <summary>
        /// Модель адаптера чтение деталей
        /// </summary>
        AdapterModel_ViewDetails = 1013,

        /// <summary>
        /// Модель периферии, добавление
        /// </summary>
        PeripheralModel_Insert = 1020,

        /// <summary>
        /// Модель сетевого устройства, добавление
        /// </summary>
        NetworkDeviceModel_Insert = 1030,

        /// <summary>
        /// Модель оконечного устройства, добавление
        /// </summary>
        TerminalDeviceModel_Insert = 1040,

        /// <summary>
        /// Порт, добавление
        /// </summary>
        PortTemplate_Insert = 1060,

        /// <summary>
        /// Порт, изменение
        /// </summary>
        PortTemplate_Update = 1061,

        /// <summary>
        /// Порт, удаление
        /// </summary>
        PortTemplate_Delete = 1062,

        /// <summary>
        /// Удаление разъема
        /// </summary>
        ConectorType_Delete = 1093,

        /// <summary>
        /// Добавление согласования
        /// </summary>
        Negotiation_Insert = 1094,

        /// <summary>
        /// Изменение согласования
        /// </summary>
        Negotiation_Update = 1095,

        /// <summary>
        /// Удаление согласования
        /// </summary>
        Negotiation_Delete = 1096,

        /// <summary>
        /// Добавление пользователя в согласование
        /// </summary>
        Negotiation_AddUser = 1097,

        /// <summary>
        /// Удаление пользователя из согласования
        /// </summary>
        Negotiation_RemoveUser = 1098,

        /// <summary>
        /// Голосование
        /// </summary>
        Negotiation_Vote = 1099,

        /// <summary>
        /// Комментарий в согласовании
        /// </summary>
        Negotiation_Comment = 1100,

        /// <summary>
        /// Назначение голосования на другого
        /// </summary>
        Negotiation_Assign = 1101,

        /// <summary>
        /// Справочник учетных записей Добавление
        /// </summary>
        UserAccount_Add = 1102,

        /// <summary>
        /// Справочник учетных записей Обновление
        /// </summary>
        UserAccount_Update = 1103,

        /// <summary>
        /// Справочник учетных записей Удаление
        /// </summary>
        UserAccount_Delete = 1104,

        /// <summary>
        /// Справочник учетных записей Просмотр
        /// </summary>
        UserAccount_Properties = 1105,

        /// <summary>
        /// Добавление Формы
        /// </summary>
        FormBuilder_Add = 1106,

        /// <summary>
        /// Обновление Формы
        /// </summary>
        FormBuilder_Update = 1107,

        /// <summary>
        /// Удаление Формы
        /// </summary>
        FormBuilder_Delete = 1108,

        /// <summary>
        /// Просмотр свойств Формы
        /// </summary>
        FormBuilder_Properties = 1109,

        /// <summary>
        /// Создание по аналогии Формы
        /// </summary>
        FormBuilder_AddAnalogy = 1110,

        /// <summary>
        /// Публикация Формы
        /// </summary>
        FormBuilder_Publish = 1111,

        /// <summary>
        /// Экспорт Формы
        /// </summary>
        FormBuilder_Export = 1112,

        /// <summary>
        /// Импорт Формы
        /// </summary>
        FormBuilder_Import = 1113,

        /// <summary>
        /// Открытие задачи планировщика
        /// </summary>
        ScheduleTask_Open = 1114,

        /// <summary>
        /// Создание задачи планировщика
        /// </summary>
        ScheduleTask_Create = 1115,

        /// <summary>
        /// Изменение задачи планировщика
        /// </summary>
        ScheduleTask_Save = 1116,

        /// <summary>
        /// Остановка задачи планировщика
        /// </summary>
        ScheduleTask_Stop = 1117,

        /// <summary>
        /// Удаление  задачи планировщика
        /// </summary>
        ScheduleTask_Delete = 1118,

        /// <summary>
        /// Просмотр категорий технических сбоев
        /// </summary>
        TechnicalFailuresCategoryType_Properties = 1119,

        /// <summary>
        /// Добавление категорий технических сбоев
        /// </summary>
        TechnicalFailuresCategoryType_Add = 1120,

        /// <summary>
        /// Удаление категорий технических сбоев
        /// </summary>
        TechnicalFailuresCategoryType_Delete = 1121,

        /// <summary>
        /// Обновление категорий технических сбоев
        /// </summary>
        TechnicalFailuresCategoryType_Update = 1122,

        /// <summary>
        /// Назначение голосования на другого участника голосования
        /// </summary>
        Negotiation_AssignToAnotherNegotiator = 1123,

        /// <summary>
        /// Удаление типа телефона
        /// </summary>
        TelephoneType_Delete = 1206,

        /// <summary>
        /// Добавление шаблона слота
        /// </summary>
        SlotTemplate_Add = 1210,

        /// <summary>
        /// Обновление шаблона слота
        /// </summary>
        SlotTemplate_Update = 1211,

        /// <summary>
        /// Удаление шаблона слота
        /// </summary>
        SlotTemplate_Delete = 1212,

        /// <summary>
        /// Получение шаблона слота
        /// </summary>
        SlotTemplate_Properties = 1213,

        /// <summary>
        /// Добавление зависимости ПО
        /// </summary>
        SoftwareModelDependency_Add = 1301,

        /// <summary>
        /// Удаление зависимости ПО
        /// </summary>
        SoftwareModelDependency_Delete = 1302,

        /// <summary>
        /// Получение зависимости ПО
        /// </summary>
        SoftwareModelDependency_Properties = 1303,

        /// <summary>
        /// Добавление права контракта на ПО
        /// </summary>
        ServiceContractLicence_Add = 1311,

        /// <summary>
        /// Удаление права контракта на ПО
        /// </summary>
        ServiceContractLicence_Delete = 1312,

        /// <summary>
        /// Получение права контракта на ПО
        /// </summary>
        ServiceContractLicence_Properties = 1313,

        /// <summary>
        /// Обновление права контракта на ПО
        /// </summary>
        ServiceContractLicence_Update = 1314,

        /// <summary>
        /// Привязка объекта к задаче
        /// </summary>
        WorkOrderReference_Add = 1315,

        /// <summary>
        /// Удаление связи с задачей
        /// </summary>
        WorkOrderReference_Remove = 1316,

        /// <summary>
        /// Добавление ссылки на статью базы знаний
        /// </summary>
        KnowledgeBaseArticleReference_Add = 1317,

        /// <summary>
        /// Удаление ссылки на статью базы знаний
        /// </summary>
        KnowledgeBaseArticleReference_Remove = 1318,

        /// <summary>
        /// Добавление связи с заявкой
        /// </summary>
        CallReference_Add = 1319,

        /// <summary>
        /// Удаление связи с заявкой
        /// </summary>
        CallReference_Remove = 1320,

        /// <summary>
        /// Добавление трудозатрат
        /// </summary>
        ManhoursEntry_Add = 1321,

        /// <summary>
        /// Изменение трудозатрат
        /// </summary>
        ManhoursEntry_Update = 1322,

        /// <summary>
        /// Удаление трудозатрат
        /// </summary>
        ManhoursEntry_Remove = 1323,

        /// <summary>
        /// Добавление коментариев
        /// </summary>
        Note_Add = 1324,

        /// <summary>
        /// Просмотр событий
        /// </summary>
        Event_Properties = 1325,

        /// <summary>
        /// Быть владельцем массового инцидента
        /// </summary>
        MassIncident_BeOwner = 1326,

        /// <summary>
        /// Создание по аналогии массового инцидента
        /// </summary>
        MassIncident_Clone = 1328,

        /// <summary>
        /// Редактировать служебные поля массового инцидента
        /// </summary>
        MassIncident_EditServiceFields = 1329,

        /// <summary>
        /// Операция позволяет назначить проблеме другого владельца.
        /// </summary>
        MassIncident_ChangeOwner = 1330,

        /// <summary>
        /// Видеть массовые инциденты ИТ-сотрудников
        /// </summary>
        MassIncident_Supervisor = 1331,
        
        /// <summary>
        /// Создание OLA
        /// </summary>
        OperationalLevelAgreement_Add = 1340,
        
        /// <summary>
        /// Обновление OLA
        /// </summary>
        OperationalLevelAgreement_Update = 1341,
        
        /// <summary>
        /// Удаление OLA
        /// </summary>
        OperationalLevelAgreement_Delete = 1342,
        
        /// <summary>
        /// Просмотр OLA
        /// </summary>
        OperationalLevelAgreement_View = 1343,

        /// <summary>
        /// Правила выделения строк в списке Добавление
        /// </summary>
        Highlighting_Add = 1344,

        /// <summary>
        /// Правила выделения строк в списке Обновление
        /// </summary>
        Highlighting_Update = 1345,

        /// <summary>
        /// Правила выделения строк в списке Удаление
        /// </summary>
        Highlighting_Delete = 1346,

        /// <summary>
        /// Правила выделения строк в списке Получение
        /// </summary>
        Highlighting_Properties = 1347,

        /// <summary>
        /// Правила выделения условий для строк в списке Добавление
        /// </summary>
        HighlightingCondition_Add = 1348,

        /// <summary>
        /// Правила выделения условий для строк в списке Обновление
        /// </summary>
        HighlightingCondition_Update = 1349,

        /// <summary>
        /// Правила выделения условий для строк в списке Удаление
        /// </summary>
        HighlightingCondition_Delete = 1350,

        /// <summary>
        /// Правила выделения условий для строк в списке Получение
        /// </summary>
        HighlightingCondition_Properties = 1351,

        /// <summary>
        /// Правила выделения значений условий для строк в списке Добавление
        /// </summary>
        HighlightingConditionValue_Add = 1352,

        /// <summary>
        /// Правила выделения значений условий для строк в списке Обновление
        /// </summary>
        HighlightingConditionValue_Update = 1353,

        /// <summary>
        /// Правила выделения значений условий для строк в списке Удаление
        /// </summary>
        HighlightingConditionValue_Delete = 1354,

        /// <summary>
        /// Правила выделения значений условий для строк в списке Получение
        /// </summary>
        HighlightingConditionValue_Properties = 1355,
        
        /// <summary>
        /// Настройка задания импорта ит-активов Добавление
        /// </summary>
        ITAssetImportSetting_Add = 1356,

        /// <summary>
        /// Настройка задания импорта ит-активов Обновление
        /// </summary>
        ITAssetImportSetting_Update = 1357,

        /// <summary>
        /// Настройка задания импорта ит-активов Удаление
        /// </summary>
        ITAssetImportSetting_Delete = 1358,

        /// <summary>
        /// Настройка задания импорта ит-активов Получение
        /// </summary>
        ITAssetImportSetting_Properties = 1359,

        /// <summary>
        /// Настройка задания импорта ит-активов Создать по аналогии
        /// </summary>
        ITAssetImportSetting_AddAs = 1360,

        /// <summary>
        /// Настройка задания импорта ит-активов Запланировать
        /// </summary>
        ITAssetImportSetting_Plan = 1361,

        /// <summary>
        /// Настройка задания импорта ит-активов Выполнить
        /// </summary>
        ITAssetImportSetting_Execute = 1362,

        /// <summary>
        /// Настройка конфигурации импорта ит-активов Добавление
        /// </summary>
        ITAssetImportCSVConfiguration_Add = 1363,

        /// <summary>
        /// Настройка конфигурации импорта ит-активов Обновление
        /// </summary>
        ITAssetImportCSVConfiguration_Update = 1364,

        /// <summary>
        /// Настройка конфигурации импорта ит-активов Удаление
        /// </summary>
        ITAssetImportCSVConfiguration_Delete = 1365,

        /// <summary>
        /// Настройка конфигурации импорта ит-активов Получение
        /// </summary>
        ITAssetImportCSVConfiguration_Properties = 1366,

        /// <summary>
        /// Настройка конфигурации импорта ит-активов Создать по аналогии
        /// </summary>
        ITAssetImportCSVConfiguration_AddAs = 1367,

        /// <summary>
        /// Быть экспертом БЗ
        /// </summary>
        BeKnowledgeBaseExpert = 1368,
        
        /// <summary>
        /// Объект «Правило распознавания» добавление
        /// </summary>
        SnmpDeviceModel_Add = 1369,
        
        /// <summary>
        /// Объект «Правило распознавания» обновление
        /// </summary>
        SnmpDeviceModel_Update = 1370,
        
        /// <summary>
        /// Объект «Правило распознавания» удаление
        /// </summary>
        SnmpDeviceModel_Delete = 1371,
        
        /// <summary>
        /// Объект «Правило распознавания» свойства
        /// </summary>
        SnmpDeviceModel_Properties = 1372,

        /// <summary>
        /// Профиль объекта «Правило распознавания» добавление
        /// </summary>
        SnmpDeviceProfile_Add = 1373,
        
        /// <summary>
        /// Профиль объекта «Правило распознавания» обновление
        /// </summary>
        SnmpDeviceProfile_Update = 1374,
        
        /// <summary>
        /// Профиль объекта «Правило распознавания» удаление
        /// </summary>
        SnmpDeviceProfile_Delete = 1375,
        
        /// <summary>
        /// Профиль объекта «Правило распознавания» свойства
        /// </summary>
        SnmpDeviceProfile_Properties = 1376,
        
        /// <summary>
        /// Добавление доп. сервиса к массовому инциденту
        /// </summary>
        MassIncidentAffectedService_Add = 1377,
        
        /// <summary>
        /// Удаление доп. сервиса массового инцидента
        /// </summary>
        MassIncidentAffectedService_Delete = 1378,

        /// <summary>
        /// Убрать роль у пользователя
        /// </summary>
        Role_UnSetRole = 1379,

        /// <summary>
        /// Изменение документального оформления операций
        /// </summary>
        OperationalDocumentation_Update = 1380,

        /// <summary>
        /// Получение документального оформления операций
        /// </summary>
        OperationalDocumentation_Properties = 1381,

        /// <summary>
        /// Справочник путей до узла LDAP, добавление
        /// </summary>
        UIADPath_Insert = 11001,

        /// <summary>
        /// Справочник путей до узла LDAP, обновление
        /// </summary>
        UIADPath_Update = 11002,

        /// <summary>
        /// Справочник путей до узла LDAP, удаление
        /// </summary>
        UIADPath_Delete = 11003,
        
        /// <summary>
        /// Сообщения по почте - просмотр
        /// </summary>
        MessageByEmail_Properties = 735001,

        /// <summary>
        /// Добавление сообщения по почте
        /// </summary>
        MessageByEmail_Add = 735002,

        /// <summary>
        /// Удаление сообщения по почте
        /// </summary>
        MessageByEmail_Delete = 735003,

        /// <summary>
        /// Изменение сообщения по почте
        /// </summary>
        MessageByEmail_Update = 735004,


        ChangeRequest_PowerfullAccess = 703028,
        CommonFilters_EditForTasks = 744001,
        CommonFilters_EditForCall = 744002,
        CommonFilters_EditForWorkOrders = 744003,
        CommonFilters_EditForProblems = 744004,
        CommonFilters_EditForRFC = 744005,
        CommonFilters_EditForMyCalls = 744006,
        CommonFilters_EditForNegotiations = 744007,
        CommonFilters_EditForControl = 744008,

        /// <summary>
        /// Получение схемы лицензирования
        /// </summary>
        LicenceScheme_Properties = 750001,

        /// <summary>
        /// Создание схемы лицензирования
        /// </summary>
        LicenceScheme_Create = 750002,

        /// <summary>
        /// Редактирование схемы лицензирования
        /// </summary>
        LicenceScheme_Edit = 750003,

        /// <summary>
        /// Удаление схемы лицензирования
        /// </summary>
        LicenceScheme_Delete = 750004,

        /// <summary>
        /// Добавление настроек для поля импорта из бд
        /// </summary>
        UIDBFileds_Insert = 750005,

        /// <summary>
        /// Обновление настроек для поля импорта из бд
        /// </summary>
        UIDBFileds_Update = 750006,

        /// <summary>
        /// Удаление настроек для поля импорта из бд
        /// </summary>
        UIDBFileds_Delete = 750007,
        
        /// <summary>
        /// Просмотр настроек для поля импорта из бд
        /// </summary>
        UIDBFileds_ViewDetails = 750008,
        
        /// <summary>
        /// Просмотр списка настроек для поля импорта из бд
        /// </summary>
        UIDBFileds_ViewDetailsArray = 750009,

        /// <summary>
        /// Создание настроек для таблиц импорта из бд
        /// </summary>
        UIDBTables_Insert = 750010,

        /// <summary>
        /// Обновление настроек для таблиц импорта из бд
        /// </summary>
        UIDBTables_Update = 750011,

        /// <summary>
        /// Удаление настроек для таблиц импорта из бд
        /// </summary>
        UIDBTables_Delete = 750012,
        
        /// <summary>
        /// Просмотр настроек для таблиц импорта из бд
        /// </summary>
        UIDBTables_ViewDetails = 750013,
        
        /// <summary>
        /// Просмотр списка настроек для таблиц импорта из бд
        /// </summary>
        UIDBTables_ViewDetailsArray = 750014,

        /// <summary>
        /// Создание конфигурации для импорта из бд
        /// </summary>
        UIDBConfiguration_Insert = 750015,

        /// <summary>
        ///  Обновление конфигурации для импорта из бд
        /// </summary>
        UIDBConfiguration_Update = 750016,

        /// <summary>
        /// Удаление конфигурации для импорта из бд
        /// </summary>
        UIDBConfiguration_Delete = 750017,
        
        /// <summary>
        /// Промотр конфигурации для импорта из бд
        /// </summary>
        UIDBConfiguration_ViewDetails = 750018,
        
        /// <summary>
        /// Промотр списка конфигураций для импорта из бд
        /// </summary>
        UIDBConfiguration_ViewDetailsArray = 750019,

        /// <summary>
        /// Создание настроек для импорта из бд
        /// </summary>
        UIDBSettings_Insert = 750020,

        /// <summary>
        /// Обновление настроек для импорта из бд
        /// </summary>
        UIDBSettings_Update = 750021,

        /// <summary>
        /// Удаление настроек для импорта из бд
        /// </summary>
        UIDBSettings_Delete = 750022,
        
        /// <summary>
        /// Просмотр настройки для импорта из бд
        /// </summary>
        UIDBSettings_ViewDetails = 750023,
        
        /// <summary>
        /// Просмотр списка настроек для импорта из бд
        /// </summary>
        UIDBSettings_ViewDetailsArray = 750024,

        
        /// <summary>
        /// Создание строки подключения для импорта из бд
        /// </summary>
        UIDBConnectionString_Insert = 750025,

        /// <summary>
        /// Обновление строки подключения для импорта из бд
        /// </summary>
        UIDBConnectionString_Update = 750026,

        /// <summary>
        /// Удаление строки подключения для импорта из бд
        /// </summary>
        UIDBConnectionString_Delete = 750027,
        
        /// <summary>
        /// Просмотр строки подключения для импорта из бд
        /// </summary>
        UIDBConnectionString_ViewDetails = 750028,
        
        /// <summary>
        /// Просмотр сиска строк подключения для импорта из бд
        /// </summary>
        UIDBConnectionString_ViewDetailsArray = 750029,

        /// <summary>
        /// Создание типа импорта из бд
        /// </summary>
        UIDBImportType_Insert = 750030,

        /// <summary>
        /// Обновление типа импорта из бд
        /// </summary>
        UIDBImportType_Update = 750032,
        

        /// <summary>
        /// Удаление типа импорта из бд
        /// </summary>
        UIDBImportType_Delete = 750033,
        
        /// <summary>
        /// Просмотр типа импорта из бд
        /// </summary>
        UIDBImportType_ViewDetails = 750034,
        
        /// <summary>
        /// Просмотр списка типов импорта из бд
        /// </summary>
        UIDBImportType_ViewDetailsArray = 750035,

        /// <summary>
        /// Создание допуска для обновления для типа импорта из бд
        /// </summary>
        UIDBFieldConfig_Insert = 750036,

        /// <summary>
        /// Обновление допуска для обновления для типа импорта из бд
        /// </summary>
        UIDBFieldConfig_Update = 750037,

        /// <summary>
        ///Удаление допуска для обновления для типа импорта из бд
        /// </summary>
        UIDBFieldConfig_Delete = 750038,
        
        /// <summary>
        /// Просмотр допуска для обновления для типа импорта из бд
        /// </summary>
        UIDBFieldConfig_ViewDetails = 750039,
        
        /// <summary>
        /// Просмотр списка допусков для обновления для типа импорта из бд
        /// </summary>
        UIDBFieldConfig_ViewDetailsArray = 750040,
        
        /// <summary>
        /// Получение информации о связи между инсталяциями и лицензиями
        /// </summary>
        ELPSetting_Properties = 820001,

        /// <summary>
        /// Добавление связи между инсталяциями и лицензиями
        /// </summary>
        ELPSetting_Insert = 820002,

        /// <summary>
        /// Изменение связи между инсталяциями и лицензиями
        /// </summary>
        ELPSetting_Update = 820003,

        /// <summary>
        /// Удаление связи между инсталяциями и лицензиями
        /// </summary>
        ELPSetting_Delete = 820004,

        /// <summary>
        /// Добавление причин отклонения от графика
        /// </summary>
        CalendarExclusion_Add = 820005,

        /// <summary>
        /// Обновление причин отклонения от графика
        /// </summary>
        CalendarExclusion_Update = 820006,

        /// <summary>
        /// Удаление причин отклонения от графика
        /// </summary>
        CalendarExclusion_Delete = 820007,

        /// <summary>
        /// Получение причин отклонения от графика
        /// </summary>
        CalendarExclusion_Properties = 820008,

        /// <summary>
        /// Типы RFC добавление
        /// </summary>
        RfcType_Add = 820009,

        /// <summary>
        /// Типы RFC обновление
        /// </summary>
        RfcType_Update = 820010,

        /// <summary>
        /// Типы RFC удаление
        /// </summary>
        RfcType_Delete = 820011,

        /// <summary>
        /// Типы RFC получение
        /// </summary>
        RfcType_Properties = 820012,

        /// <summary>
        /// Просмотр портов для объекта типа Адаптер
        /// </summary>
        PortAdapter_Properties = 830026,

        /// <summary>
        /// Добавление портов для объекта типа Адаптер
        /// </summary>
        PortAdapter_Add = 830027,

        /// <summary>
        /// Обновление портов для объекта типа Адаптер
        /// </summary>
        PortAdapter_Update = 830028,

        /// <summary>
        /// Удаление портов для объекта типа Адаптер
        /// </summary>
        PortAdapter_Delete = 830029
    }
}
