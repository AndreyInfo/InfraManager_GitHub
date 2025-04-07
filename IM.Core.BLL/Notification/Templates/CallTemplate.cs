using System;
using InfraManager.DAL.ServiceDesk;

namespace InfraManager.BLL.Notification.Templates
{
    [ObjectClassMapping(ObjectClass.Call)]
    public class CallTemplate : ITemplate<Call>
    {
        public Guid ID { get; init; }

        [TemplateParameter("Номер заявки")]
        public string NumberString { get; init; }

        [TemplateParameter("Способ получения заявки")]
        public string ReceiptTypeString { get; init; }

        [TemplateParameter("Описание заявки")]
        public string Description { get; init; }

        [TemplateParameter("Описание заявки в формате HTML")]
        public string HTMLDescription { get; init; }

        [TemplateParameter("Решение заявки")]
        public string Solution { get; init; }

        [TemplateParameter("Решение заявки в формате HTML")]
        public string HTMLSolution { get; init; }

        [TemplateParameter("Оценка заявки")]
        public string GradeString { get; init; }

        [TemplateParameter("Количество эскалаций заявки")]
        public string EscalationCountString { get; init; }

        [TemplateParameter("Когда зарегистрирована заявка")]
        public string DateRegisteredString { get; internal set; }

        [TemplateParameter("Когда открыта заявка")]
        public string DateOpenedString { get; internal set; }

        [TemplateParameter("Когда должна быть закрыта заявка")]
        public string DatePromisedString { get; internal set; }

        [TemplateParameter("Когда выполнена заявка")]
        public string DateAccomplishedString { get; internal set; }

        [TemplateParameter("Когда закрыта заявка")]
        public string DateClosedString { get; internal set; }

        [TemplateParameter("Когда изменена заявка")]
        public string DateModifiedString { get; internal set; }

        [TemplateParameter("Когда создана заявка")]
        public string DateCreatedString { get; internal set; }

        [TemplateParameter("Тип заявки")]
        public string CallTypeFullName { get; init; }

        [TemplateParameter("Краткое описание заявки")]
        public string CallSummaryName { get; init; }

        [TemplateParameter("Сервис заявки")]
        public string ServiceName { get; internal set; }

        [TemplateParameter("Элемент сервиса заявки")]
        public string ServiceItemFullName { get; internal set; }

        [TemplateParameter("Услуга заявки")]
        public string ServiceAttendanceFullName { get; internal set; }

        [TemplateParameter("Место оказания сервиса")]
        public string ServicePlaceFullName { get; internal set; }

        [TemplateParameter("Срочность заявки")]
        public string UrgencyName { get; init; }

        [TemplateParameter("Влияние заявки")]
        public string InfluenceName { get; init; }

        [TemplateParameter("Приоритет заявки")]
        public string PriorityName { get; init; }

        [TemplateParameter("Результат завершения инцидента заявки")]
        public string IncidentResultName { get; init; }

        [TemplateParameter("Результат запроса на услугу заявки")]
        public string RFSResultName { get; internal set; }

        [TemplateParameter("Группа заявки")]
        public string QueueName { get; init; }

        [TemplateParameter("Фамилия заявителя заявки")]
        public string InitiatorLastName { get; init; }

        [TemplateParameter("Имя заявителя заявки")]
        public string InitiatorFirstName { get; init; }

        [TemplateParameter("Отчество заявителя заявки")]
        public string InitiatorPatronymic { get; init; }

        [TemplateParameter("Заявитель заявки")]
        public string InitiatorFullName { get; init; }

        [TemplateParameter("Логин заявителя заявки")]
        public string InitiatorLogin { get; init; }

        [TemplateParameter("Телефон заявителя заявки")]
        public string InitiatorPhone { get; init; }

        [TemplateParameter("Табельный номер заявителя заявки")]
        public string InitiatorNumber { get; init; }

        [TemplateParameter("E-mail заявителя заявки")]
        public string InitiatorEmail { get; init; }

        [TemplateParameter("Факс заявителя заявки")]
        public string InitiatorFax { get; init; }

        [TemplateParameter("Прочее заявителя заявки")]
        public string InitiatorPager { get; init; }

        [TemplateParameter("Должность заявителя заявки")]
        public string InitiatorPositionName { get; init; }

        [TemplateParameter("Здание заявителя заявки")]
        public string InitiatorBuildingName { get; init; }

        [TemplateParameter("Комната заявителя заявки")]
        public string InitiatorRoomName { get; init; }

        [TemplateParameter("Рабочее место заявителя заявки")]
        public string InitiatorWorkplaceName { get; init; }

        [TemplateParameter("Организация заявителя заявки")]
        public string InitiatorOrganizationName { get; init; }

        [TemplateParameter("Подразделение заявителя заявки")]
        public string InitiatorSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия клиента заявки")]
        public string ClientLastName { get; init; }

        [TemplateParameter("Имя клиента заявки")]
        public string ClientFirstName { get; init; }

        [TemplateParameter("Отчество клиента заявки")]
        public string ClientPatronymic { get; init; }

        [TemplateParameter("Клиент заявки")]
        public string ClientFullName { get; init; }

        [TemplateParameter("Логин клиента заявки")]
        public string ClientLogin { get; init; }

        [TemplateParameter("Телефон клиента заявки")]
        public string ClientPhone { get; init; }

        [TemplateParameter("Табельный номер клиента заявки")]
        public string ClientNumber { get; init; }

        [TemplateParameter("E-mail клиента заявки")]
        public string ClientEmail { get; init; }

        [TemplateParameter("Факс клиента заявки")]
        public string ClientFax { get; init; }

        [TemplateParameter("Прочее клиента заявки")]
        public string ClientPager { get; init; }

        [TemplateParameter("Должность клиента заявки")]
        public string ClientPositionName { get; init; }

        [TemplateParameter("Здание клиента заявки")]
        public string ClientBuildingName { get; init; }

        [TemplateParameter("Комната клиента заявки")]
        public string ClientRoomName { get; init; }

        [TemplateParameter("Рабочее место клиента заявки")]
        public string ClientWorkplaceName { get; init; }

        [TemplateParameter("Организация клиента заявки")]
        public string ClientOrganizationName { get; init; }

        [TemplateParameter("Подразделение клиента заявки")]
        public string ClientSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия владельца заявки")]
        public string OwnerLastName { get; init; }

        [TemplateParameter("Имя владельца заявки")]
        public string OwnerFirstName { get; init; }

        [TemplateParameter("Отчество владельца заявки")]
        public string OwnerPatronymic { get; init; }

        [TemplateParameter("Владелец заявки")]
        public string OwnerFullName { get; init; }

        [TemplateParameter("Логин владельца заявки")]
        public string OwnerLogin { get; init; }

        [TemplateParameter("Телефон владельца заявки")]
        public string OwnerPhone { get; init; }

        [TemplateParameter("Табельный номер владельца заявки")]
        public string OwnerNumber { get; init; }

        [TemplateParameter("E-mail владельца заявки")]
        public string OwnerEmail { get; init; }

        [TemplateParameter("Факс владельца заявки")]
        public string OwnerFax { get; init; }

        [TemplateParameter("Прочее владельца заявки")]
        public string OwnerPager { get; init; }

        [TemplateParameter("Должность владельца заявки")]
        public string OwnerPositionName { get; init; }

        [TemplateParameter("Здание владельца заявки")]
        public string OwnerBuildingName { get; init; }

        [TemplateParameter("Комната владельца заявки")]
        public string OwnerRoomName { get; init; }

        [TemplateParameter("Рабочее место владельца заявки")]
        public string OwnerWorkplaceName { get; init; }

        [TemplateParameter("Организация владельца заявки")]
        public string OwnerOrganizationName { get; init; }

        [TemplateParameter("Подразделение владельца заявки")]
        public string OwnerSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия исполнителя заявки")]
        public string ExecutorLastName { get; init; }

        [TemplateParameter("Имя исполнителя заявки")]
        public string ExecutorFirstName { get; init; }

        [TemplateParameter("Отчество исполнителя заявки")]
        public string ExecutorPatronymic { get; init; }

        [TemplateParameter("Исполнитель заявки")]
        public string ExecutorFullName { get; init; }

        [TemplateParameter("Логин исполнителя заявки")]
        public string ExecutorLogin { get; init; }

        [TemplateParameter("Телефон исполнителя заявки")]
        public string ExecutorPhone { get; init; }

        [TemplateParameter("Табельный номер исполнителя заявки")]
        public string ExecutorNumber { get; init; }

        [TemplateParameter("E-mail исполнителя заявки")]
        public string ExecutorEmail { get; init; }

        [TemplateParameter("Факс исполнителя заявки")]
        public string ExecutorFax { get; init; }

        [TemplateParameter("Прочее исполнителя заявки")]
        public string ExecutorPager { get; init; }

        [TemplateParameter("Должность исполнителя заявки")]
        public string ExecutorPositionName { get; init; }

        [TemplateParameter("Здание исполнителя заявки")]
        public string ExecutorBuildingName { get; init; }

        [TemplateParameter("Комната исполнителя заявки")]
        public string ExecutorRoomName { get; init; }

        [TemplateParameter("Рабочее место исполнителя заявки")]
        public string ExecutorWorkplaceName { get; init; }

        [TemplateParameter("Организация исполнителя заявки")]
        public string ExecutorOrganizationName { get; init; }

        [TemplateParameter("Подразделение исполнителя заявки")]
        public string ExecutorSubdivisionName { get; internal set; }

        [TemplateParameter("Фамилия выполнившего заявку")]
        public string AccomplisherLastName { get; init; }

        [TemplateParameter("Имя выполнившего заявку")]
        public string AccomplisherFirstName { get; init; }

        [TemplateParameter("Отчество выполнившего заявку")]
        public string AccomplisherPatronymic { get; init; }

        [TemplateParameter("Выполнивший заявку")]
        public string AccomplisherFullName { get; init; }

        [TemplateParameter("Логин выполнившего заявку")]
        public string AccomplisherLogin { get; init; }

        [TemplateParameter("Телефон выполнившего заявку")]
        public string AccomplisherPhone { get; init; }

        [TemplateParameter("Табельный номер выполнившего заявку")]
        public string AccomplisherNumber { get; init; }

        [TemplateParameter("E-mail выполнившего заявку")]
        public string AccomplisherEmail { get; init; }

        [TemplateParameter("Факс выполнившего заявку")]
        public string AccomplisherFax { get; init; }

        [TemplateParameter("Прочее выполнившего заявку")]
        public string AccomplisherPager { get; init; }

        [TemplateParameter("Должность выполнившего заявку")]
        public string AccomplisherPositionName { get; init; }

        [TemplateParameter("Здание выполнившего заявку")]
        public string AccomplisherBuildingName { get; init; }

        [TemplateParameter("Комната выполнившего заявку")]
        public string AccomplisherRoomName { get; init; }

        [TemplateParameter("Рабочее место выполнившего заявку")]
        public string AccomplisherWorkplaceName { get; init; }

        [TemplateParameter("Организация выполнившего заявку")]
        public string AccomplisherOrganizationName { get; init; }

        [TemplateParameter("Подразделение выполнившего заявку")]
        public string AccomplisherSubdivisionName { get; internal set; }

        [TemplateParameter("Число прикрепленных документов к заявке")]
        public string DocumentCountString { get; init; }

        [TemplateParameter("Число согласований, относящихся к данной заявке")]
        public string NegotiationCountString { get; internal set; }

        [TemplateParameter("Число связанных проблем, относящихся к данной заявке")]
        public string ProblemCountString { get; init; }

        [TemplateParameter("Число связанных rfc, относящихся к данной заявке")]
        public string RFCCountString { get; internal set; }

        [TemplateParameter("Число связанных заданий заявки")]
        public string WorkOrderCountString { get; init; }

        [TemplateParameter("Число связей заявки")]
        public string DependencyObjectCountString { get; internal set; }

        [TemplateParameter("Сообщения заявки (все)")]
        public string MessageString { get; internal set; }

        [TemplateParameter("Сообщения заявки (последние 5)")]
        public string LastMessageString { get; internal set; }

        [TemplateParameter("Сообщения заявки (последнее 1)")]
        public string FinalMessageString { get; internal set; }

        [TemplateParameter("Сообщения заявки (все) в формате HTML")]
        public string HTMLMessageString { get; internal set; }

        [TemplateParameter("Сообщения заявки (последние 5) в формате HTML")]
        public string HTMLLastMessageString { get; internal set; }

        [TemplateParameter("Сообщения заявки (последнее 1) в формате HTML")]
        public string HTMLFinalMessageString { get; internal set; }

        [TemplateParameter("Трудозатраты заявки")]
        public string ManhoursString { get; init; }

        [TemplateParameter("Оценка трудозатрат заявки")]
        public string ManhoursNormString { get; init; }

        [TemplateParameter("Бюджет заявки")]
        public string BudgetString { get; init; }

        [TemplateParameter("Основание заявки")]
        public string BudgetUsageCauseString { get; init; }

        [TemplateParameter("Адрес Web сервера СервисДеска")]
        public string WebServerAddress { get; internal set; }

        [TemplateParameter("Установка оценки")]
        public string SetGrade { get; init; }
        
        [TemplateParameter("Состояние")]
        public string EntityStateName { get; init; }

        [TemplateParameter("Цифровой номер")]
        public string NumberOnly { get; init; }
    }
}