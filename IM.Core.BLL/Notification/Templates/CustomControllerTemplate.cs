using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.BLL.Notification.Templates
{
    public class CustomControllerTemplate
    {
        [TemplateParameter("Адрес Web сервера СервисДеска")]
        public string WebServerAddress { get; set; }
        [TemplateParameter("Состояние заявки")]
        public string CallEntityStateName { get; set; }

        [TemplateParameter("Номер заявки")]
        public string CallNumberString { get; set; }

        [TemplateParameter("Способ получения заявки")]
        public string CallReceiptTypeString { get; set; }

        [TemplateParameter("Описание заявки")]
        public string CallDescription { get; set; }

        [TemplateParameter("Решение заявки")]
        public string CallSolution { get; set; }

        [TemplateParameter("Оценка заявки")]
        public string CallGradeString { get; set; }
        [TemplateParameter("Количество эскалаций заявки")]
        public string CallEscalationCountString { get; set; }

        [TemplateParameter("Когда зарегистрирована заявка")]
        public string CallDateRegisteredString { get; set; }

        [TemplateParameter("Когда открыта заявка")]
        public string CallDateOpenedString { get; set; }

        [TemplateParameter("Когда должна быть закрыта заявка")]
        public string CallDatePromisedString { get; set; }

        [TemplateParameter("Когда выполнена заявка")]
        public string CallDateAccomplishedString { get; set; }

        [TemplateParameter("Когда закрыта заявка")]
        public string CallDateClosedString { get; set; }

        [TemplateParameter("Когда изменена заявка")]
        public string CallDateModifiedString { get; set; }

        [TemplateParameter("Тип заявки")]
        public string CallCallTypeFullName { get; set; }
        [TemplateParameter("Краткое описание заявки")]
        public string CallCallSummaryName { get; set; }

        [TemplateParameter("Сервис заявки")]
        public string CallServiceName { get; set; }

        [TemplateParameter("Элемент сервиса заявки")]
        public string CallServiceItemFullName { get; set; }

        [TemplateParameter("Услуга заявки")]
        public string CallServiceAttendanceFullName { get; set; }
        [TemplateParameter("Срочность заявки")]
        public string CallUrgencyName { get; set; }
        [TemplateParameter("Влияние заявки")]
        public string CallInfluenceName { get; set; }
        [TemplateParameter("Приоритет заявки")]
        public string CallPriorityName { get; set; }
        [TemplateParameter("Результат завершения инцидента заявки")]
        public string CallIncidentResultName { get; set; }

        [TemplateParameter("Результат запроса на услугу заявки")]
        public string CallRFSResultName { get; set; }

        [TemplateParameter("Группа заявки")]
        public string CallQueueName { get; set; }

        [TemplateParameter("Фамилия заявителя заявки")]
        public string CallInitiatorLastName { get; set; }

        [TemplateParameter("Имя заявителя заявки")]
        public string CallInitiatorFirstName { get; set; }

        [TemplateParameter("Отчество заявителя заявки")]
        public string CallInitiatorPatronymic { get; set; }
        [TemplateParameter("Заявитель заявки")]
        public string CallInitiatorFullName { get; set; }

        [TemplateParameter("Логин заявителя заявки")]
        public string CallInitiatorLogin { get; set; }

        [TemplateParameter("Телефон заявителя заявки")]
        public string CallInitiatorPhone { get; set; }
        [TemplateParameter("Табельный номер заявителя заявки")]
        public string CallInitiatorNumber { get; set; }
        [TemplateParameter("E-mail заявителя заявки")]
        public string CallInitiatorEmail { get; set; }

        [TemplateParameter("Факс заявителя заявки")]
        public string CallInitiatorFax { get; set; }

        [TemplateParameter("Прочее заявителя заявки")]
        public string CallInitiatorPager { get; set; }

        [TemplateParameter("Должность заявителя заявки")]
        public string CallInitiatorPositionName { get; set; }

        [TemplateParameter("Здание заявителя заявки")]
        public string CallInitiatorBuildingName { get; set; }

        [TemplateParameter("Комната заявителя заявки")]
        public string CallInitiatorRoomName { get; set; }

        [TemplateParameter("Рабочее место заявителя заявки")]
        public string CallInitiatorWorkplaceName { get; set; }

        [TemplateParameter("Организация заявителя заявки")]
        public string CallInitiatorOrganizationName { get; set; }

        [TemplateParameter("Подразделение заявителя заявки")]
        public string CallInitiatorSubdivisionName { get; set; }

        [TemplateParameter("Фамилия клиента заявки")]
        public string CallClientLastName { get; set; }

        [TemplateParameter("Имя клиента заявки")]
        public string CallClientFirstName { get; set; }
        [TemplateParameter("Отчество клиента заявки")]
        public string CallClientPatronymic { get; set; }

        [TemplateParameter("Клиент заявки")]
        public string CallClientFullName { get; set; }

        [TemplateParameter("Логин клиента заявки")]
        public string CallClientLogin { get; set; }
        [TemplateParameter("Телефон клиента заявки")]
        public string CallClientPhone { get; set; }

        [TemplateParameter("Табельный номер клиента заявки")]
        public string CallClientNumber { get; set; }

        [TemplateParameter("E-mail клиента заявки")]
        public string CallClientEmail { get; set; }

        [TemplateParameter("Факс клиента заявки")]
        public string CallClientFax { get; set; }
        [TemplateParameter("Прочее клиента заявки")]
        public string CallClientPager { get; set; }

        [TemplateParameter("Должность клиента заявки")]
        public string CallClientPositionName { get; set; }
        [TemplateParameter("Здание клиента заявки")]
        public string CallClientBuildingName { get; set; }

        [TemplateParameter("Комната клиента заявки")]
        public string CallClientRoomName { get; set; }

        [TemplateParameter("Рабочее место клиента заявки")]
        public string CallClientWorkplaceName { get; set; }

        [TemplateParameter("Организация клиента заявки")]
        public string CallClientOrganizationName { get; set; }

        [TemplateParameter("Подразделение клиента заявки")]
        public string CallClientSubdivisionName { get; set; }

        [TemplateParameter("Фамилия владельца заявки")]
        public string CallOwnerLastName { get; set; }

        [TemplateParameter("Имя владельца заявки")]
        public string CallOwnerFirstName { get; set; }

        [TemplateParameter("Отчество владельца заявки")]
        public string CallOwnerPatronymic { get; set; }

        [TemplateParameter("Владелец заявки")]
        public string CallOwnerFullName { get; set; }

        [TemplateParameter("Логин владельца заявки")]
        public string CallOwnerLogin { get; set; }

        [TemplateParameter("Телефон владельца заявки")]
        public string CallOwnerPhone { get; set; }

        [TemplateParameter("Табельный номер владельца заявки")]
        public string CallOwnerNumber { get; set; }

        [TemplateParameter("E-mail владельца заявки")]
        public string CallOwnerEmail { get; set; }

        [TemplateParameter("Факс владельца заявки")]
        public string CallOwnerFax { get; set; }

        [TemplateParameter("Прочее владельца заявки")]
        public string CallOwnerPager { get; set; }

        [TemplateParameter("Должность владельца заявки")]
        public string CallOwnerPositionName { get; set; }

        [TemplateParameter("Здание владельца заявки")]
        public string CallOwnerBuildingName { get; set; }

        [TemplateParameter("Комната владельца заявки")]
        public string CallOwnerRoomName { get; set; }

        [TemplateParameter("Рабочее место владельца заявки")]
        public string CallOwnerWorkplaceName { get; set; }

        [TemplateParameter("Организация владельца заявки")]
        public string CallOwnerOrganizationName { get; set; }

        [TemplateParameter("Подразделение владельца заявки")]
        public string CallOwnerSubdivisionName { get; set; }

        [TemplateParameter("Фамилия исполнителя заявки")]
        public string CallExecutorLastName { get; set; }

        [TemplateParameter("Имя исполнителя заявки")]
        public string CallExecutorFirstName { get; set; }

        [TemplateParameter("Отчество исполнителя заявки")]
        public string CallExecutorPatronymic { get; set; }

        [TemplateParameter("Исполнитель заявки")]
        public string CallExecutorFullName { get; set; }

        [TemplateParameter("Логин исполнителя заявки")]
        public string CallExecutorLogin { get; set; }

        [TemplateParameter("Телефон исполнителя заявки")]
        public string CallExecutorPhone { get; set; }
        [TemplateParameter("Табельный номер исполнителя заявки")]
        public string CallExecutorNumber { get; set; }

        [TemplateParameter("E-mail исполнителя заявки")]
        public string CallExecutorEmail { get; set; }

        [TemplateParameter("Факс исполнителя заявки")]
        public string CallExecutorFax { get; set; }

        [TemplateParameter("Прочее исполнителя заявки")]
        public string CallExecutorPager { get; set; }

        [TemplateParameter("Должность исполнителя заявки")]
        public string CallExecutorPositionName { get; set; }

        [TemplateParameter("Здание исполнителя заявки")]
        public string CallExecutorBuildingName { get; set; }

        [TemplateParameter("Комната исполнителя заявки")]
        public string CallExecutorRoomName { get; set; }

        [TemplateParameter("Рабочее место исполнителя заявки")]
        public string CallExecutorWorkplaceName { get; set; }

        [TemplateParameter("Организация исполнителя заявки")]
        public string CallExecutorOrganizationName { get; set; }
        [TemplateParameter("Подразделение исполнителя заявки")]
        public string CallExecutorSubdivisionName { get; set; }

        [TemplateParameter("Фамилия выполнившего заявку")]
        public string CallAccomplisherLastName { get; set; }

        [TemplateParameter("Имя выполнившего заявку")]
        public string CallAccomplisherFirstName { get; set; }

        [TemplateParameter("Отчество выполнившего заявку")]
        public string CallAccomplisherPatronymic { get; set; }

        [TemplateParameter("Выполнивший заявку")]
        public string CallAccomplisherFullName { get; set; }

        [TemplateParameter("Логин выполнившего заявку")]
        public string CallAccomplisherLogin { get; set; }

        [TemplateParameter("Телефон выполнившего заявку")]
        public string CallAccomplisherPhone { get; set; }

        [TemplateParameter("Табельный номер выполнившего заявку")]
        public string CallAccomplisherNumber { get; set; }

        [TemplateParameter("E-mail выполнившего заявку")]
        public string CallAccomplisherEmail { get; set; }

        [TemplateParameter("Факс выполнившего заявку")]
        public string CallAccomplisherFax { get; set; }

        [TemplateParameter("Прочее выполнившего заявку")]
        public string CallAccomplisherPager { get; set; }

        [TemplateParameter("Должность выполнившего заявку")]
        public string CallAccomplisherPositionName { get; set; }

        [TemplateParameter("Здание выполнившего заявку")]
        public string CallAccomplisherBuildingName { get; set; }

        [TemplateParameter("Комната выполнившего заявку")]
        public string CallAccomplisherRoomName { get; set; }

        [TemplateParameter("Рабочее место выполнившего заявку")]
        public string CallAccomplisherWorkplaceName { get; set; }

        [TemplateParameter("Организация выполнившего заявку")]
        public string CallAccomplisherOrganizationName { get; set; }
        [TemplateParameter("Подразделение выполнившего заявку")]
        public string CallAccomplisherSubdivisionName { get; set; }
        [TemplateParameter("Число прикрепленных документов к заявке")]
        public string CallDocumentCountString { get; set; }
        [TemplateParameter("Число связанных заданий заявки")]
        public string CallWorkOrderCountString { get; set; }

        [TemplateParameter("Число связей заявки")]
        public string CallDependencyObjectCountString { get; set; }

        [TemplateParameter("Сообщения заявки (все)")]
        public string CallMessageString { get; set; }

        [TemplateParameter("Сообщения заявки (последние 5)")]
        public string CallLastMessageString { get; set; }

        [TemplateParameter("Сообщения заявки (последнее 1)")]
        public string CallFinalMessageString { get; set; }

        [TemplateParameter("Сообщения заявки (все) в формате HTML")]
        public string HTMLCallMessageString { get; set; }

        [TemplateParameter("Сообщения заявки (последние 5) в формате HTML")]
        public string HTMLCallLastMessageString { get; set; }

        [TemplateParameter("Сообщения заявки (последнее 1) в формате HTML")]
        public string HTMLCallFinalMessageString { get; set; }
        [TemplateParameter("Состояние проблемы")]
        public string ProblemEntityStateName { get; set; }

        [TemplateParameter("Номер проблемы")]
        public string ProblemNumberString { get; set; }

        [TemplateParameter("Описание проблемы")]
        public string ProblemDescription { get; set; }
        [TemplateParameter("Решение проблемы")]
        public string ProblemSolution { get; set; }

        [TemplateParameter("Краткое описание проблемы")]
        public string ProblemSummary { get; set; }

        [TemplateParameter("Причина проблемы")]
        public string ProblemCause { get; set; }

        [TemplateParameter("Временное решение проблемы")]
        public string ProblemFix { get; set; }

        [TemplateParameter("Когда обнаружена проблема")]
        public string ProblemDateDetectedString { get; set; }
        [TemplateParameter("Когда должна быть закрыта проблема")]
        public string ProblemDatePromisedString { get; set; }

        [TemplateParameter("Когда решена проблема")]
        public string ProblemDateSolvedString { get; set; }

        [TemplateParameter("Когда закрыта проблема")]
        public string ProblemDateClosedString { get; set; }

        [TemplateParameter("Когда изменена проблема")]
        public string ProblemDateModifiedString { get; set; }

        [TemplateParameter("Тип проблемы")]
        public string ProblemProblemTypeFullName { get; set; }

        [TemplateParameter("Краткое описание причины проблемы")]
        public string ProblemProblemCauseName { get; set; }

        [TemplateParameter("Срочность проблемы")]
        public string ProblemUrgencyName { get; set; }

        [TemplateParameter("Влияние проблемы")]
        public string ProblemInfluenceName { get; set; }
        [TemplateParameter("Приоритет проблемы")]
        public string ProblemPriorityName { get; set; }

        [TemplateParameter("Фамилия владельца проблемы")]
        public string ProblemOwnerLastName { get; set; }

        [TemplateParameter("Имя владельца проблемы")]
        public string ProblemOwnerFirstName { get; set; }

        [TemplateParameter("Отчество владельца проблемы")]
        public string ProblemOwnerPatronymic { get; set; }

        [TemplateParameter("Владелец проблемы")]
        public string ProblemOwnerFullName { get; set; }

        [TemplateParameter("Логин владельца проблемы")]
        public string ProblemOwnerLogin { get; set; }

        [TemplateParameter("Телефон владельца проблемы")]
        public string ProblemOwnerPhone { get; set; }

        [TemplateParameter("Табельный номер владельца проблемы")]
        public string ProblemOwnerNumber { get; set; }

        [TemplateParameter("E-mail владельца проблемы")]
        public string ProblemOwnerEmail { get; set; }

        [TemplateParameter("Факс владельца проблемы")]
        public string ProblemOwnerFax { get; set; }

        [TemplateParameter("Прочее владельца проблемы")]
        public string ProblemOwnerPager { get; set; }

        [TemplateParameter("Должность владельца проблемы")]
        public string ProblemOwnerPositionName { get; set; }

        [TemplateParameter("Здание владельца проблемы")]
        public string ProblemOwnerBuildingName { get; set; }

        [TemplateParameter("Комната владельца проблемы")]
        public string ProblemOwnerRoomName { get; set; }
        
        [TemplateParameter("Рабочее место владельца проблемы")]
        public string ProblemOwnerWorkplaceName { get; set; }


        [TemplateParameter("Организация владельца проблемы")]
        public string ProblemOwnerOrganizationName { get; set; }


        [TemplateParameter("Подразделение владельца проблемы")]
        public string ProblemOwnerSubdivisionName { get; set; }
        
        [TemplateParameter("Число прикрепленных документов к проблеме")]
        public string ProblemDocumentCountString { get; set; }
        
        [TemplateParameter("Число связанных заявок проблемы")]
        public string ProblemCallCountString { get; set; }

        [TemplateParameter("Число связанных заданий проблемы")]
        public string ProblemWorkOrderCountString { get; set; }

        [TemplateParameter("Число связей проблемы")]
        public string ProblemDependencyObjectCountString { get; set; }
        
        [TemplateParameter("Состояние задания")]
        public string WorkOrderEntityStateName { get; set; }
        
        [TemplateParameter("Номер задания")]
        public string WorkOrderNumberString { get; set; }
        
        [TemplateParameter("Название задания")]
        public string WorkOrderName { get; set; }

        [TemplateParameter("Описание задания")]
        public string WorkOrderDescription { get; set; }

        [TemplateParameter("Трудозатраты задания")]
        public string WorkOrderManHoursString { get; set; }

        [TemplateParameter("Норматив задания")]
        public string WorkOrderManhoursNormString { get; set; }
        
        [TemplateParameter("С чем связано задание")]
        public string WorkOrderWorkOrderReferenceString { get; set; }
        
        [TemplateParameter("Когда создано задание")]
        public string WorkOrderDateCreatedString { get; set; }

        [TemplateParameter("Когда изменено задание")]
        public string WorkOrderDateModifiedString { get; set; }
        
        [TemplateParameter("Когда назначено задание")]
        public string WorkOrderDateAssignedString { get; set; }

        [TemplateParameter("Когда принято задание")]
        public string WorkOrderDateAcceptedString { get; set; }

        [TemplateParameter("Когда должно быть выполнено задание")]
        public string WorkOrderDatePromisedString { get; set; }

        [TemplateParameter("Когда начато выполнение задания")]
        public string WorkOrderDateStartedString { get; set; }

        [TemplateParameter("Когда завершено задание")]
        public string WorkOrderDateAccomplishedString { get; set; }

        [TemplateParameter("Тип задания")]
        public string WorkOrderWorkOrderTypeName { get; set; }

        [TemplateParameter("Приоритет задания")]
        public string WorkOrderWorkOrderPriorityName { get; set; }

        [TemplateParameter("Бюджет задания")]
        public string WorkOrderBudgetString { get; set; }

        [TemplateParameter("Основание задания")]
        public string WorkOrderGroundString { get; set; }

        [TemplateParameter("Фамилия инициатора задания")]
        public string WorkOrderInitiatorLastName { get; set; }

        [TemplateParameter("Имя инициатора задания")]
        public string WorkOrderInitiatorFirstName { get; set; }

        [TemplateParameter("Отчество инициатора задания")]
        public string WorkOrderInitiatorPatronymic { get; set; }

        [TemplateParameter("Инициатор задания")]
        public string WorkOrderInitiatorFullName { get; set; }

        [TemplateParameter("Логин инициатора задания")]
        public string WorkOrderInitiatorLogin { get; set; }

        [TemplateParameter("Телефон инициатора задания")]
        public string WorkOrderInitiatorPhone { get; set; }

        [TemplateParameter("Табельный номер инициатора задания")]
        public string WorkOrderInitiatorNumber { get; set; }

        [TemplateParameter("E-mail инициатора задания")]
        public string WorkOrderInitiatorEmail { get; set; }

        [TemplateParameter("Факс инициатора задания")]
        public string WorkOrderInitiatorFax { get; set; }

        [TemplateParameter("Прочее инициатора задания")]
        public string WorkOrderInitiatorPager { get; set; }
        
        [TemplateParameter("Должность инициатора задания")]
        public string WorkOrderInitiatorPositionName { get; set; }
        
        [TemplateParameter("Здание инициатора задания")]
        public string WorkOrderInitiatorBuildingName { get; set; }

        [TemplateParameter("Комната инициатора задания")]
        public string WorkOrderInitiatorRoomName { get; set; }

        [TemplateParameter("Рабочее место инициатора задания")]
        public string WorkOrderInitiatorWorkplaceName { get; set; }
        
        [TemplateParameter("Организация инициатора задания")]
        public string WorkOrderInitiatorOrganizationName { get; set; }
        
        [TemplateParameter("Подразделение инициатора задания")]
        public string WorkOrderInitiatorSubdivisionName { get; set; }

        [TemplateParameter("Фамилия назначившего задание")]
        public string WorkOrderAssignorLastName { get; set; }

        [TemplateParameter("Имя назначившего задание")]
        public string WorkOrderAssignorFirstName { get; set; }

        [TemplateParameter("Отчество назначившего задание")]
        public string WorkOrderAssignorPatronymic { get; set; }

        [TemplateParameter("Назначивший задание")]
        public string WorkOrderAssignorFullName { get; set; }

        [TemplateParameter("Логин назначившего задание")]
        public string WorkOrderAssignorLogin { get; set; }

        [TemplateParameter("Телефон назначившего задание")]
        public string WorkOrderAssignorPhone { get; set; }
        
        [TemplateParameter("Табельный номер назначившего задание")]
        public string WorkOrderAssignorNumber { get; set; }

        [TemplateParameter("E-mail назначившего задание")]
        public string WorkOrderAssignorEmail { get; set; }

        [TemplateParameter("Факс назначившего задание")]
        public string WorkOrderAssignorFax { get; set; }
        
        [TemplateParameter("Прочее назначившего задание")]
        public string WorkOrderAssignorPager { get; set; }
        
        [TemplateParameter("Должность назначившего задание")]
        public string WorkOrderAssignorPositionName { get; set; }

        [TemplateParameter("Здание назначившего задание")]
        public string WorkOrderAssignorBuildingName { get; set; }

        [TemplateParameter("Комната назначившего задание")]
        public string WorkOrderAssignorRoomName { get; set; }

        [TemplateParameter("Рабочее место назначившего задание")]
        public string WorkOrderAssignorWorkplaceName { get; set; }

        [TemplateParameter("Организация назначившего задание")]
        public string WorkOrderAssignorOrganizationName { get; set; }

        [TemplateParameter("Подразделение назначившего задание")]
        public string WorkOrderAssignorSubdivisionName { get; set; }

        [TemplateParameter("Фамилия исполнителя задания")]
        public string WorkOrderExecutorLastName { get; set; }

        [TemplateParameter("Имя исполнителя задания")]
        public string WorkOrderExecutorFirstName { get; set; }

        [TemplateParameter("Отчество исполнителя задания")]
        public string WorkOrderExecutorPatronymic { get; set; }

        [TemplateParameter("Исполнитель задания")]
        public string WorkOrderExecutorFullName { get; set; }

        [TemplateParameter("Логин исполнителя задания")]
        public string WorkOrderExecutorLogin { get; set; }
        
        [TemplateParameter("Телефон исполнителя задания")]
        public string WorkOrderExecutorPhone { get; set; }

        [TemplateParameter("Табельный номер исполнителя задания")]
        public string WorkOrderExecutorNumber { get; set; }
        
        [TemplateParameter("E-mail исполнителя задания")]
        public string WorkOrderExecutorEmail { get; set; }

        [TemplateParameter("Факс исполнителя задания")]
        public string WorkOrderExecutorFax { get; set; }

        [TemplateParameter("Прочее исполнителя задания")]
        public string WorkOrderExecutorPager { get; set; }

        [TemplateParameter("Должность исполнителя задания")]
        public string WorkOrderExecutorPositionName { get; set; }

        [TemplateParameter("Здание исполнителя задания")]
        public string WorkOrderExecutorBuildingName { get; set; }

        [TemplateParameter("Комната исполнителя задания")]
        public string WorkOrderExecutorRoomName { get; set; }

        [TemplateParameter("Рабочее место исполнителя задания")]
        public string WorkOrderExecutorWorkplaceName { get; set; }

        [TemplateParameter("Организация исполнителя задания")]
        public string WorkOrderExecutorOrganizationName { get; set; }
        
        [TemplateParameter("Подразделение исполнителя задания")]
        public string WorkOrderExecutorSubdivisionName { get; set; }
        
        [TemplateParameter("Группа задания")]
        public string WorkOrderQueueName { get; set; }
        
        [TemplateParameter("Число прикрепленных документов к заданию")]
        public string WorkOrderDocumentCountString { get; set; }
        
        [TemplateParameter("Число связей задания")]
        public string WorkOrderDependencyObjectCountString { get; set; }
        
        [TemplateParameter("Состояние запроса на изменения")]
        public string RFCEntityStateName { get; set; }
        
        [TemplateParameter("Номер запроса на изменения")]
        public string RFCNumberString { get; set; }
        
        [TemplateParameter("Описание запроса на изменения")]
        public string RFCDescription { get; set; }
        
        [TemplateParameter("Краткое описание запроса на изменения")]
        public string RFCSummary { get; set; }
        
        [TemplateParameter("Когда обнаружен запрос на изменения")]
        public string RFCDateDetectedString { get; set; }

        [TemplateParameter("Когда должен быть закрыт запрос на изменения")]
        public string RFCDatePromisedString { get; set; }

        [TemplateParameter("Когда должен быть начат запрос на изменения")]
        public string RFCDateStartedString { get; set; }
        
        [TemplateParameter("Когда решен запрос на изменения")]
        public string RFCDateSolvedString { get; set; }
        
        [TemplateParameter("Когда закрыт запрос на изменения")]
        public string RFCDateClosedString { get; set; }

        [TemplateParameter("Когда изменен запрос на изменения")]
        public string RFCDateModifiedString { get; set; }
        
        [TemplateParameter("Тип запроса на изменения")]
        public string RFCTypeFullName { get; set; }

        [TemplateParameter("Категория запроса на изменения")]
        public string RFCCategoryFullName { get; set; }

        [TemplateParameter("Срочность запроса на изменения")]
        public string RFCUrgencyName { get; set; }

        [TemplateParameter("Размер финансирования запроса на изменения")]
        public string RFCFundingAmount { get; set; }

        [TemplateParameter("Влияние запроса на изменения")]
        public string RFCInfluenceName { get; set; }

        [TemplateParameter("Приоритет запроса на изменения")]
        public string RFCPriorityName { get; set; }

        [TemplateParameter("Сервис запроса на изменения")]
        public string RFCServiceName { get; set; }

        [TemplateParameter("Фамилия владельца запроса на изменения")]
        public string RFCOwnerLastName { get; set; }
        
        [TemplateParameter("Имя владельца запроса на изменения")]
        public string RFCOwnerFirstName { get; set; }
        
        [TemplateParameter("Отчество владельца запроса на изменения")]
        public string RFCOwnerPatronymic { get; set; }

        [TemplateParameter("Владелец запроса на изменения")]
        public string RFCOwnerFullName { get; set; }

        [TemplateParameter("Логин владельца запроса на изменения")]
        public string RFCOwnerLogin { get; set; }

        [TemplateParameter("Телефон владельца запроса на изменения")]
        public string RFCOwnerPhone { get; set; }

        [TemplateParameter("Табельный номер владельца запроса на изменения")]
        public string RFCOwnerNumber { get; set; }

        [TemplateParameter("E-mail владельца запроса на изменения")]
        public string RFCOwnerEmail { get; set; }

        [TemplateParameter("Факс владельца запроса на изменения")]
        public string RFCOwnerFax { get; set; }

        [TemplateParameter("Прочее владельца запроса на изменения")]
        public string RFCOwnerPager { get; set; }

        [TemplateParameter("Должность владельца запроса на изменения")]
        public string RFCOwnerPositionName { get; set; }

        [TemplateParameter("Здание владельца запроса на изменения")]
        public string RFCOwnerBuildingName { get; set; }
        
        [TemplateParameter("Комната владельца запроса на изменения")]
        public string RFCOwnerRoomName { get; set; }

        [TemplateParameter("Рабочее место владельца запроса на изменения")]
        public string RFCOwnerWorkplaceName { get; set; }

        [TemplateParameter("Организация владельца запроса на изменения")]
        public string RFCOwnerOrganizationName { get; set; }

        [TemplateParameter("Подразделение владельца запроса на изменения")]
        public string RFCOwnerSubdivisionName { get; set; }

        [TemplateParameter("Цель запроса на изменения")]
        public string RFCTarget { get; set; }

        [TemplateParameter("Число прикрепленных документов к запросу на изменения")]
        public string RFCDocumentCountString { get; set; }

        [TemplateParameter("Число связанных заданий запроса на изменения")]
        public string RFCWorkOrderCountString { get; set; }

        [TemplateParameter("Число связей запроса на изменения")]
        public string RFCDependencyObjectCountString { get; set; }

        [TemplateParameter("Трудозатраты запроса на изменения")]
        public string RFCManHoursString { get; set; }

        [TemplateParameter("Число согласований, относящихся к запросу на изменения")]
        public string NegotiationCountString { get; set; }

        [TemplateParameter("Норматив запроса на изменения")]
        public string RFCManhoursNormString { get; set; }

        [TemplateParameter("Фамилия заявителя запроса на изменения")]
        public string RFCInitiatorLastName { get; set; }

        [TemplateParameter("Имя заявителя запроса на изменения")]
        public string RFCInitiatorFirstName { get; set; }

        [TemplateParameter("Отчество заявителя запроса на изменения")]
        public string RFCInitiatorPatronymic { get; set; }

        [TemplateParameter("Заявитель запроса на изменения")]
        public string RFCInitiatorFullName { get; set; }

        [TemplateParameter("Логин заявителя запроса на изменения")]
        public string RFCInitiatorLogin { get; set; }

        [TemplateParameter("Телефон заявителя запроса на изменения")]
        public string RFCInitiatorPhone { get; set; }
        [TemplateParameter("Табельный номер заявителя запроса на изменения")]
        public string RFCInitiatorNumber { get; set; }

        [TemplateParameter("E-mail заявителя запроса на изменения")]
        public string RFCInitiatorEmail { get; set; }

        [TemplateParameter("Факс заявителя запроса на изменения")]
        public string RFCInitiatorFax { get; set; }
        
        [TemplateParameter("Прочее заявителя запроса на изменения")]
        public string RFCInitiatorPager { get; set; }

        [TemplateParameter("Должность заявителя запроса на изменения")]
        public string RFCInitiatorPositionName { get; set; }

        [TemplateParameter("Здание заявителя запроса на изменения")]
        public string RFCInitiatorBuildingName { get; set; }

        [TemplateParameter("Комната заявителя запроса на изменения")]
        public string RFCInitiatorRoomName { get; set; }
        
        [TemplateParameter("Рабочее место заявителя запроса на изменения")]
        public string RFCInitiatorWorkplaceName { get; set; }

        [TemplateParameter("Организация заявителя запроса на изменения")]
        public string RFCInitiatorOrganizationName { get; set; }
        
        [TemplateParameter("Подразделение заявителя запроса на изменения")]
        public string RFCInitiatorSubdivisionName { get; set; }
    }
}
