namespace InfraManager.DAL.ProductCatalogue;

public enum ProductTemplate : int
{
    /// <summary>
    /// Сетевое оборудование
    /// </summary>
    Network = 0,

    /// <summary>
    /// Маршрутизатор
    /// </summary>
    Router = 1,

    /// <summary>
    /// Компьютер
    /// </summary>
    Computer = 2,

    /// <summary>
    /// Оконечное оборудование
    /// </summary>
    Terminal = 3,

    /// <summary>
    /// Коммутатор
    /// </summary>
    Switch = 4,

    /// <summary>
    /// Принтер (оконечное оборудование)
    /// </summary>
    TerminalPrinter = 5,

    /// <summary>
    /// Сервер
    /// </summary>
    Server = 6,

    /// <summary>
    /// Модем (оконечное оборудование)
    /// </summary>
    TerminalModem = 7,

    /// <summary>
    /// Мост
    /// </summary>
    Bridge = 8,

    /// <summary>
    /// Телефон
    /// </summary>
    Phone = 9,

    /// <summary>
    /// Факс
    /// </summary>
    Fax = 10,

    /// <summary>
    /// Система хранения данных
    /// </summary>
    DataStorage = 11,

    /// <summary>
    /// Составной логический объект
    /// </summary>
    ComplexLogicalObject = 12,

    /// <summary>
    /// Принтер (сетевое оборудование)
    /// </summary>
    NetworkPrinter = 13,

    /// <summary>
    /// Расходный материал (картридж)
    /// </summary>
    CartridgeExpence = 36,

    /// <summary>
    /// Сервисный контракт
    /// </summary>
    ServiceContract = 115,

    /// <summary>
    /// Расходный материал
    /// </summary>
    Expense = 120,

    /// <summary>
    /// Приложение
    /// </summary>
    Application = 164,

    /// <summary>
    /// Информационный объект
    /// </summary>
    InformationObject = 165,

    /// <summary>
    /// Лицензия программного обеспечения (самостоятельная)
    /// </summary>
    SoftwareLicenseSelf = 183,

    /// <summary>
    /// Лицензия программного обеспечения (аренда)
    /// </summary>
    RentLicense = 184,

    /// <summary>
    /// Лицензия программного обеспечения (upgrade)
    /// </summary>
    UpgradeLicense = 185,

    /// <summary>
    /// Лицензия программного обеспечения (подписка)
    /// </summary>
    SubscribeLicense = 186,

    /// <summary>
    ///Лицензия программного обеспечения (продление подписки)
    /// </summary>
    SubscribeRenewalLicense = 187,

    /// <summary>
    /// OEM Лицензия программного обеспечения
    /// </summary>
    SoftwareLicenseOEM = 189,

    /// <summary>
    /// Лицензия программного обеспечения
    /// </summary>
    SoftwareLicense = 223,

    /// <summary>
    /// Адаптер
    /// </summary>
    Adapter = 329,

    /// <summary>
    /// Материнская плата
    /// </summary>
    MotherBoard = 330,

    /// <summary>
    /// Процессор
    /// </summary>
    Processor = 331,

    /// <summary>
    /// Модуль оперативной памяти
    /// </summary>
    Ram = 332,

    /// <summary>
    /// Видеоадаптер
    /// </summary>
    VideoAdapter = 333,

    /// <summary>
    /// Звуковая карта
    /// </summary>
    SoundCard = 334,

    /// <summary>
    /// Сетевая карта
    /// </summary>
    NetworkAdapter = 335,

    /// <summary>
    /// Жесткий диск
    /// </summary>
    HardDrive = 336,

    /// <summary>
    /// CD/DVD привод
    /// </summary>
    CdDvdRom = 337,

    /// <summary>
    /// Привод гибких дисков
    /// </summary>
    Fdd = 338,

    /// <summary>
    /// Монитор
    /// </summary>
    Display = 340,

    /// <summary>
    /// Клавиатура
    /// </summary>
    Keyboard = 341,

    /// <summary>
    /// Мышь
    /// </summary>
    Mouse = 342,

    /// <summary>
    /// Принтер (периферийное устройство)
    /// </summary>
    PeripherialPrinter = 345,

    /// <summary>
    /// Сканер
    /// </summary>
    Scanner = 346,

    /// <summary>
    /// Модем (периферийное устройство)
    /// </summary>
    PeripherialModem = 347,

    /// <summary>
    /// Контроллер системы хранения данных
    /// </summary>
    DataStorageController = 352,

    /// <summary>
    /// ИТ система
    /// </summary>
    ITSystem = 360,

    /// <summary>
    /// Периферийное устройство
    /// </summary>
    Peripherial = 376,

    /// <summary>
    /// Модем (адаптер)
    /// </summary>
    ModemAdapter = 377,

    /// <summary>
    /// Узел сети
    /// </summary>
    Node = 409,

    /// <summary>
    /// Коммутатор КЕ
    /// </summary>
    SwitchKE = 410,

    /// <summary>
    /// Маршрутизатор КЕ
    /// </summary>
    RouterKE = 411,

    /// <summary>
    /// Принт сервер КЕ
    /// </summary>
    PrintServerKE = 412,

    /// <summary>
    /// Система хранения данных КЕ
    /// </summary>
    DataStorageKE = 413,

    /// <summary>
    /// Win сервер КЕ
    /// </summary>
    WinServerKE = 414,

    /// <summary>
    /// логический объект
    /// </summary>
    LogicalObject = 415,

    /// <summary>
    /// Сервер виртуальный
    /// </summary>
    VirtualServer = 416,

    /// <summary>
    /// Компьютер виртуальный
    /// </summary>
    VirtualComputer = 417,

    /// <summary>
    /// Коммутатор виртуальный
    /// </summary>
    VirtualRouter = 418,

    /// <summary>
    /// Хост КЕ
    /// </summary>
    HostKE = 419,

    /// <summary>
    /// Кластер
    /// </summary>
    Cluster = 420,

    /// <summary>
    /// Шкаф
    /// </summary>
    Rack = 421,
    
    /// <summary>
    /// Конфигурационная единица
    /// </summary>
    ConfigurationUnit = 450
}