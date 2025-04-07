using InfraManager.DAL.ServiceDesk.MassIncidents;

namespace InfraManager.BLL.Notification.Templates;

[ObjectClassMapping(ObjectClass.MassIncident)] // TODO: Убрать этот атрибут
public class MassIncidentTemplate : ITemplate<MassIncident>
{
    [TemplateParameter("Адрес Web сервера СервисДеска")]
    public string WebServerAddress { get; set; } 

    [TemplateParameter("Номер")]
    public string NumberString { get; init; } 
    
    [TemplateParameter("Тип массового инцидента")]
    public string MassIncidentTypeFullName { get; init; } 
    
    [TemplateParameter("Краткое описание")]
    public string ShortSummary { get; init; } 
    
    [TemplateParameter("Подробное описание")]
    public string Summary { get; init; } 
    
    [TemplateParameter("Приоритет")]
    public string MassIncidentPriorityName { get; init; } 
    
    [TemplateParameter("Состояние")]
    public string EntityStateName { get; init; } 
    
    [TemplateParameter("Основной сервис")]
    public string MainService { get; init; } 
    
    [TemplateParameter("Критичность")]
    public string CriticalityName { get; init; } 
    
    [TemplateParameter("Решение")]
    public string Remedy { get; init; } 

    [TemplateParameter("Когда создан")]
    public string DateCreatedString { get; set; } 
    
    [TemplateParameter("Когда открыт")]
    public string DateOpenedString { get; set; } 
    
    [TemplateParameter("Когда зарегистрирован")]
    public string DateRegisteredString { get; set; } 
    
    [TemplateParameter("Когда выполнен")]
    public string DateAccomplishedString { get; set; } 
    
    [TemplateParameter("Когда должен быть закрыт")]
    public string DatePromisedString { get; set; } 
    
    [TemplateParameter("Когда закрыт")]
    public string DateClosedString { get; set; } 
    
    [TemplateParameter("Число прикрепленных документов")]
    public string DocumentCountString { get; set; } 
    
    [TemplateParameter("Число связанных заданий")]
    public string WorkOrderCountString { get; set; } 
    
    [TemplateParameter("Число связанных заявок")]
    public string CallCountString { get; set; } 
    
    [TemplateParameter("Число связанных проблем")]
    public string ProblemCountString { get; set; } 
    
    [TemplateParameter("Число связанных запросов на изменение")]
    public string RequestForChangeCountString { get; set; } 
    
    [TemplateParameter("Число согласований")]
    public string NegotiationCountString { get; set; }  
    
    [TemplateParameter("Число дополнительных сервисов")]
    public string AdditionalServicesCountString { get; set; } 
    
    
    
    [TemplateParameter("Фамилия инициатора")]
    public string CreatedBySurname { get; init; }
    
    [TemplateParameter("Имя инициатора")]
    public string CreatedByName { get; init; } 

    [TemplateParameter("Отчество инициатора")]
    public string CreatedByPatronymic { get; init; } 

    [TemplateParameter("Инициатор")]
    public string CreatedByFullName { get; init; } 

    [TemplateParameter("Телефон инициатора")]
    public string CreatedByPhone { get; init; } 

    [TemplateParameter("Табельный номер инициатора")]
    public string CreatedByNumber { get; init; } 

    [TemplateParameter("E-mail инициатора")]
    public string CreatedByEmail { get; init; } 

    [TemplateParameter("Факс инициатора")]
    public string CreatedByFax { get; init; } 

    [TemplateParameter("Прочее инициатора")]
    public string CreatedByPager { get; init; } 

    [TemplateParameter("Должность инициатора")]
    public string CreatedByPositionName { get; init; } 

    [TemplateParameter("Здание инициатора")]
    public string CreatedByBuildingName { get; init; } 

    [TemplateParameter("Комната инициатора")]
    public string CreatedByRoomName { get; init; } 

    [TemplateParameter("Рабочее место инициатора")]
    public string CreatedByWorkplaceName { get; init; } 

    [TemplateParameter("Организация инициатора")]
    public string CreatedByOrganizationName { get; init; } 

    [TemplateParameter("Подразделение инициатора")]
    public string CreatedBySubdivisionName { get; init; } 
    
    
    [TemplateParameter("Фамилия исполнителя")]
    public string ExecutorSurname { get; init; } 

    [TemplateParameter("Имя исполнителя")]
    public string ExecutorName { get; init; } 

    [TemplateParameter("Отчество исполнителя")]
    public string ExecutorPatronymic { get; init; } 

    [TemplateParameter("Исполнитель")]
    public string ExecutorFullName { get; init; } 

    [TemplateParameter("Телефон исполнителя")]
    public string ExecutorPhone { get; init; } 

    [TemplateParameter("Табельный номер исполнителя")]
    public string ExecutorNumber { get; init; } 

    [TemplateParameter("E-mail исполнителя")]
    public string ExecutorEmail { get; init; } 

    [TemplateParameter("Факс исполнителя")]
    public string ExecutorFax { get; init; } 

    [TemplateParameter("Прочее исполнителя")]
    public string ExecutorPager { get; init; } 

    [TemplateParameter("Должность исполнителя")]
    public string ExecutorPositionName { get; init; } 

    [TemplateParameter("Здание исполнителя")]
    public string ExecutorBuildingName { get; init; } 

    [TemplateParameter("Комната исполнителя")]
    public string ExecutorRoomName { get; init; } 

    [TemplateParameter("Рабочее место исполнителя")]
    public string ExecutorWorkplaceName { get; init; } 

    [TemplateParameter("Организация исполнителя")]
    public string ExecutorOrganizationName { get; init; } 

    [TemplateParameter("Подразделение исполнителя")]
    public string ExecutorSubdivisionName { get; init; } 
    
    
    [TemplateParameter("Фамилия владельца")]
    public string OwnedBySurname { get; init; } 

    [TemplateParameter("Имя владельца")]
    public string OwnedByName { get; init; } 

    [TemplateParameter("Отчество владельца")]
    public string OwnedByPatronymic { get; init; } 

    [TemplateParameter("Владелец")]
    public string OwnedByFullName { get; init; } 

    [TemplateParameter("Телефон владельца")]
    public string OwnedByPhone { get; init; } 

    [TemplateParameter("Табельный номер владельца")]
    public string OwnedByNumber { get; init; } 

    [TemplateParameter("E-mail владельца")]
    public string OwnedByEmail { get; init; } 

    [TemplateParameter("Факс владельца")]
    public string OwnedByFax { get; init; } 

    [TemplateParameter("Прочее владельца")]
    public string OwnedByPager { get; init; } 

    [TemplateParameter("Должность владельца")]
    public string OwnedByPositionName { get; init; } 

    [TemplateParameter("Здание владельца")]
    public string OwnedByBuildingName { get; init; } 

    [TemplateParameter("Комната владельца")]
    public string OwnedByRoomName { get; init; } 

    [TemplateParameter("Рабочее место владельца")]
    public string OwnedByWorkplaceName { get; init; } 

    [TemplateParameter("Организация владельца")]
    public string OwnedByOrganizationName { get; init; } 

    [TemplateParameter("Подразделение владельца")]
    public string OwnedBySubdivisionName { get; init; }

    [TemplateParameter("Цифровой номер")]
    public string NumberOnly { get; init; }


    [TemplateParameter("Категория технического сбоя")]
    public string CategoryName { get; init; }   
     
    [TemplateParameter("Группа массового инцидента")]
    public string GroupName { get; init; } 
       
    [TemplateParameter("Подробное описание в HTML")]
    public string HTMLSummary { get; init; }

    [TemplateParameter("Решение в HTML")]
    public string HTMLRemedy { get; init; }

    [TemplateParameter("Последняя заметка")]
    public string LastNote { get; set; }

    [TemplateParameter("Последняя заметка в HTML")]
    public string HTMLLastNote { get; set; }

    [TemplateParameter("Все заметки")] 
    public string AllNotes { get; set; }

    [TemplateParameter("Все заметки в HTML")]
    public string HTMLAllNotes { get; set; }

    [TemplateParameter("Последние 5 заметок")]
    public string Last5Notes { get; set; }

    [TemplateParameter("Последние 5 заметок в HTML")]
    public string HTMLLast5Notes { get; set; }

    [TemplateParameter("Причина массового инцидента")]
    public string MICause { get; init; }

    [TemplateParameter("Канал приема")] 
    public string ReceiptTypeString { get; init; }

    [TemplateParameter("Причина")] 
    public string Cause { get; init; }

    [TemplateParameter("Причина в HTML")] 
    public string HTMLCause { get; init; }           
}