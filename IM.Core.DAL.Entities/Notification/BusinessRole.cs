namespace InfraManager.DAL.Notification;

//TODO узнать по поводу располдение enum, корректно ли хранить его здесь
public enum BusinessRole
{
    None = 0,                           //0
    /// <summary>
    /// Владелец заявки
    /// </summary>
    CallOwner = 1,                      //1
    
    /// <summary>
    /// Клиент заявки
    /// </summary>
    CallClient = 1 << 1,                //2

    /// <summary>
    /// Инициатор заявки
    /// </summary>
    CallInitiator = 1 << 2,             //4

    /// <summary>
    /// Выполнивший заявку
    /// </summary>
    CallAccomplisher = 1 << 3,          //8

    /// <summary>
    /// Исполнитель заявки
    /// </summary>
    CallExecutor = 1 << 4,              //16

    /// <summary>
    /// Исполнитель задания
    /// </summary>
    WorkOrderExecutor = 1 << 5,         //32

    /// <summary>
    /// Назначивший задание
    /// </summary>
    WorkOrderAssignor = 1 << 6,         //64

    /// <summary>
    /// Инициатор задания
    /// </summary>
    WorkOrderInitiator = 1 << 7,        //128
    
    /// <summary>
    /// Материально ответственный
    /// </summary>
    MateriallyLiablePerson = 1 << 8,    //256

    /// <summary>
    /// Использующий
    /// </summary>
    Utilizer = 1 << 9,                  //512

    /// <summary>
    /// Администратор
    /// </summary>
    SDAdministrator = 1 << 10,          //1024

    /// <summary>
    /// Владелец проблемы
    /// </summary>
    ProblemOwner = 1 << 11,             //2048

    /// <summary>
    /// Участник согласования
    /// </summary>
    NegotiationParticipant = 1 << 12,   //4096

    /// <summary>
    /// Система
    /// </summary>
    System = 1 << 13,                   //8192

    /// <summary>
    /// Контроллер
    /// </summary>
    ControllerParticipant = 1 << 14,     //16384

    /// <summary>
    /// Владелец запроса на изменения
    /// </summary>
    RFCOwner = 1 << 15,                  //32768

    /// <summary>
    /// Инициатор запроса на изменения
    /// </summary>
    RFCInitiator = 1 << 16,               //65536

    /// <summary>
    /// Заместитель
    /// </summary>
    DeputyUser = 1 << 17,               //131072

    /// <summary>
    /// Замещаемый
    /// </summary>
    ReplacedUser = 1 << 18             //262144
}
