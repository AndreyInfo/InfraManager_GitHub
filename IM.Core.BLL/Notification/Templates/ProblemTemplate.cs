using System;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates
{
    [ObjectClassMapping(ObjectClass.Problem)]
    public class ProblemTemplate : ITemplate<Problem>
    {
        public Guid ID { get; init; }

        [TemplateParameter("Номер проблемы")]
        public string NumberString { get; init; }

        [TemplateParameter("Описание проблемы")]
        public string Description { get; init; }

        [TemplateParameter("Описание проблемы в формате HTML")]
        public string HTMLDescription { get; init; }

        [TemplateParameter("Решение проблемы")]
        public string Solution { get; init; }

        [TemplateParameter("Решение проблемы в формате HTML")]
        public string HTMLSolution { get; init; }

        [TemplateParameter("Краткое описание проблемы")]
        public string Summary { get; init; }

        [TemplateParameter("Причина проблемы")]
        public string Cause { get; init; }

        [TemplateParameter("Причина проблемы в формате HTML")]
        public string HTMLCause { get; init; }

        [TemplateParameter("Временное решение проблемы")]
        public string Fix { get; init; }

        [TemplateParameter("Временное решение проблемы в формате HTML")]
        public string HTMLFix { get; init; }

        [TemplateParameter("Когда обнаружена проблема")]
        public string DateDetectedString { get; internal set; }

        [TemplateParameter("Когда должна быть закрыта проблема")]
        public string DatePromisedString { get; internal set; }

        [TemplateParameter("Когда решена проблема")]
        public string DateSolvedString { get; internal set; }

        [TemplateParameter("Когда закрыта проблема")]
        public string DateClosedString { get; internal set; }

        [TemplateParameter("Когда изменена проблема")]
        public string DateModifiedString { get; internal set; }

        [TemplateParameter("Тип проблемы")]
        public string ProblemTypeFullName { get; init; }

        [TemplateParameter("Краткое описание причины проблемы")]
        public string ProblemCauseName { get; init; }

        [TemplateParameter("Срочность проблемы")]
        public string UrgencyName { get; init; }

        [TemplateParameter("Влияние проблемы")]
        public string InfluenceName { get; init; }

        [TemplateParameter("Приоритет проблемы")]
        public string PriorityName { get; init; }

        [TemplateParameter("Фамилия владельца проблемы")]
        public string OwnerLastName { get; init; }

        [TemplateParameter("Имя владельца проблемы")]
        public string OwnerFirstName { get; init; }

        [TemplateParameter("Отчество владельца проблемы")]
        public string OwnerPatronymic { get; init; }

        [TemplateParameter("Владелец проблемы")]
        public string OwnerFullName { get; init; }

        [TemplateParameter("Логин владельца проблемы")]
        public string OwnerLogin { get; init; }

        [TemplateParameter("Телефон владельца проблемы")]
        public string OwnerPhone { get; init; }

        [TemplateParameter("Табельный номер владельца проблемы")]
        public string OwnerNumber { get; init; }

        [TemplateParameter("E-mail владельца проблемы")]
        public string OwnerEmail { get; init; }

        [TemplateParameter("Факс владельца проблемы")]
        public string OwnerFax { get; init; }

        [TemplateParameter("Прочее владельца проблемы")]
        public string OwnerPager { get; init; }

        [TemplateParameter("Должность владельца проблемы")]
        public string OwnerPositionName { get; init; }

        [TemplateParameter("Здание владельца проблемы")]
        public string OwnerBuildingName { get; init; }

        [TemplateParameter("Комната владельца проблемы")]
        public string OwnerRoomName { get; init; }

        [TemplateParameter("Рабочее место владельца проблемы")]
        public string OwnerWorkplaceName { get; init; }

        [TemplateParameter("Организация владельца проблемы")]
        public string OwnerOrganizationName { get; init; }

        [TemplateParameter("Подразделение владельца проблемы")]
        public string OwnerSubdivisionName { get; internal set; }

        [TemplateParameter("Число прикрепленных документов к проблеме")]
        public string DocumentCountString { get; internal set; }

        [TemplateParameter("Число согласований, относящихся к данной проблеме")]
        public string NegotiationCountString { get; internal set; }

        [TemplateParameter("Число связанных заявок проблемы")]
        public string CallCountString { get; init; }

        [TemplateParameter("Число связанных заданий проблемы")]
        public string WorkOrderCountString { get; init; }

        [TemplateParameter("Число связанных массовых инцидентов")]
        public string MassIncidentsCountString { get; internal set; }

        [TemplateParameter("Число связей проблемы")]
        public string DependencyObjectCountString { get; init; }

        [TemplateParameter("Трудозатраты проблемы")]
        public string ManhoursString { get; init; }

        [TemplateParameter("Оценка трудозатрат проблемы")]
        public string ManhoursNormString { get; init; }

        [TemplateParameter("Адрес Web сервера СервисДеска")]
        public string WebServerAddress { get; internal set; }

        [TemplateParameter("Состояние")]
        public string EntityStateName { get; internal set; }
        

        [TemplateParameter("Исполнитель проблемы")]
        public string ExecutorFullName { get; init; }
        
        [TemplateParameter("Фамилия исполнителя проблемы")]
        public string ExecutorLastName { get; init; } 

        [TemplateParameter("Имя исполнителя проблемы")]
        public string ExecutorFirstName { get; init; } 

        [TemplateParameter("Отчество исполнителя проблемы")]
        public string ExecutorPatronymic { get; init; }

        [TemplateParameter("Телефон исполнителя проблемы")]
        public string ExecutorPhone { get; init; } 

        [TemplateParameter("Табельный номер исполнителя проблемы")]
        public string ExecutorNumber { get; init; } 

        [TemplateParameter("E-mail исполнителя проблемы")]
        public string ExecutorEmail { get; init; } 

        [TemplateParameter("Факс исполнителя проблемы")]
        public string ExecutorFax { get; init; } 

        [TemplateParameter("Прочее исполнителя проблемы")]
        public string ExecutorPager { get; init; } 

        [TemplateParameter("Должность исполнителя проблемы")]
        public string ExecutorPositionName { get; init; } 

        [TemplateParameter("Здание исполнителя проблемы")]
        public string ExecutorBuildingName { get; init; } 

        [TemplateParameter("Комната исполнителя проблемы")]
        public string ExecutorRoomName { get; init; } 

        [TemplateParameter("Рабочее место исполнителя проблемы")]
        public string ExecutorWorkplaceName { get; init; } 

        [TemplateParameter("Организация исполнителя проблемы")]
        public string ExecutorOrganizationName { get; init; } 

        [TemplateParameter("Подразделение исполнителя проблемы")]
        public string ExecutorSubdivisionName { get; internal set; } 
        

        [TemplateParameter("Инициатор проблемы")]
        public string InitiatorFullName { get; init; } 
        
        [TemplateParameter("Фамилия инициатора проблемы")]
        public string InitiatorLastName { get; init; } 

        [TemplateParameter("Имя инициатора проблемы")]
        public string InitiatorFirstName { get; init; } 

        [TemplateParameter("Отчество инициатора проблемы")]
        public string InitiatorPatronymic { get; init; } 

        [TemplateParameter("Телефон инициатора проблемы")]
        public string InitiatorPhone { get; init; } 

        [TemplateParameter("Табельный номер инициатора проблемы")]
        public string InitiatorNumber { get; init; } 

        [TemplateParameter("E-mail инициатора проблемы")]
        public string InitiatorEmail { get; init; } 

        [TemplateParameter("Факс инициатора проблемы")]
        public string InitiatorFax { get; init; } 

        [TemplateParameter("Прочее инициатора проблемы")]
        public string InitiatorPager { get; init; } 

        [TemplateParameter("Должность инициатора проблемы")]
        public string InitiatorPositionName { get; init; } 

        [TemplateParameter("Здание инициатора проблемы")]
        public string InitiatorBuildingName { get; init; } 

        [TemplateParameter("Комната инициатора проблемы")]
        public string InitiatorRoomName { get; init; } 

        [TemplateParameter("Рабочее место инициатора проблемы")]
        public string InitiatorWorkplaceName { get; init; } 

        [TemplateParameter("Организация инициатора проблемы")]
        public string InitiatorOrganizationName { get; init; } 

        [TemplateParameter("Подразделение инициатора проблемы")]
        public string InitiatorSubdivisionName { get; internal set; }
        

        [TemplateParameter("Основной сервис")]
        public string MainService { get; init; }

        [TemplateParameter("Последняя заметка")]
        public string LastNote { get; internal set; }

        [TemplateParameter("Последняя заметка в формате HTML")]
        public string HTMLLastNote { get; internal set; }

        [TemplateParameter("Все заметки")]
        public string AllNotes { get; internal set; }

        [TemplateParameter("Все заметки в формате HTML")]
        public string HTMLAllNotes { get; internal set; }

        [TemplateParameter("Последние 5 заметок")]
        public string Last5Notes { get; internal set; }

        [TemplateParameter("Последние 5 заметок в формате HTML")]
        public string HTMLLast5Notes { get; internal set; }

        [TemplateParameter("Цифровой номер")]
        public string NumberOnly { get; init; }

    }
}
